using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd
{
    public partial class MainWindow : Form
    {
        public MainWindow(Engine engine)
        {
            InitializeComponent();
            this.engine = engine;
            engine.UIEvent += HandleUiEvent;

            PopulateSourceSystems();
            DisplaySelectedLine();
            UpdateButtonState();
        }

        #region fields
        private Engine engine;
        #endregion

        #region properties
        public List<SourceSystem> SourceSystemCollection { get; set; }
        public SearchSet SearchSet { get; private set; }

        private SourceSystem selectedSourceSystem;
        public SourceSystem SelectedSourceSystem
        {
            get { return selectedSourceSystem; }
            private set
            {
                selectedSourceSystem = value;
                var enable = selectedSourceSystem == null ? false : true;
                UpdateButtonState();
            }
        }

        private List<LogLine> searchResults;
        public List<LogLine> SearchResults { get { return searchResults; } 
            private set
            {
                searchResults = value;
                var enable = searchResults?.Count > 0 ? true : false;
                UpdateButtonState();
            }
        }
        #endregion

        #region methods
        public async Task PopulateSourceSystems()
        {
            SourceSystemCollection = await engine.GetAllSourceSystems();
            lb_SourceSystemList.DisplayMember = "Name";
            lb_SourceSystemList.DataSource = SourceSystemCollection;
        }

        public void HandleUiEvent(object sender, UIEventArg eventArg)
        {
            UpdateButtonState();
        }

        #endregion

        #region button event-handlers
        private void btn_SourceFocus_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tb_SourceFocus_Directory.Text = dialog.SelectedPath;
            }
        }

        private async void btn_SourceFocus_DirectorySave_Click(object sender, EventArgs e)
        {
            var userPath = tb_SourceFocus_Directory.Text;
            if (Directory.Exists(userPath) && SelectedSourceSystem != null)
            {
                SelectedSourceSystem.SourceFolder = userPath;
                await engine.UpdateSourceSystem(SelectedSourceSystem);
            }
        }

        private void btn_SelectSource_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null) return;

            SelectedSourceSystem = lb_SourceSystemList.SelectedItem as SourceSystem;
            tb_SourceFocus_Directory.Text = SelectedSourceSystem.SourceFolder;
            tb_SourceFocus_LineTemplate.Text = SelectedSourceSystem.LineTemplate;
        }

        private async void btn_AddNewSource_Click(object sender, EventArgs e)
        {
            string value = "";
            if (InputBox("Navngiv nyt kildesystem", "Skriv et nyt navn", ref value) == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    lb_SearchResults.DataSource = null;

                    SourceSystem newSource = new SourceSystem { Name = value };
                    await engine.AddSourceSystem(newSource);
                    await PopulateSourceSystems();

                    // refocus on newly created item
                    lb_SourceSystemList.ClearSelected();
                    var newSS = SourceSystemCollection.Where(system => system.Name == value).FirstOrDefault();
                    var index = SourceSystemCollection.IndexOf(newSS);
                    lb_SourceSystemList.SelectedIndex = index;
                }
            }
        }

        private async void btn_DeleteSource_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null) return;

            lb_SearchResults.DataSource = null;

            SelectedSourceSystem = lb_SourceSystemList.SelectedItem as SourceSystem;
            await engine.RemoveSourceSystem(SelectedSourceSystem);

            await PopulateSourceSystems();
        }

        private async void btn_UpdateSource_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null) return;

            lb_SearchResults.DataSource = null;

            engine.ResetOldSearch();

            SelectedSourceSystem = lb_SourceSystemList.SelectedItem as SourceSystem;
            await engine.UpdateSourceSystem(SelectedSourceSystem);
        }

        private async void btn_ExecuteSearch_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItems.Count < 1) return;

            // Ensure result-list is cleared
            lb_SearchResults.DataSource = null;

            var search = BuildSearchSet();

            // Display "Searching.." in UI
            lbl_SearchResults.Text = "Søger ...";
            tb_SearchResults_SelectedLine.Text = "";
            UpdateButtonState();

            if (!string.IsNullOrEmpty(search.KeyWordList))
            {
                await engine.SendQueryToParser(search);
                SearchResults = await engine.RetrieveResultsFromParser();
            } else
            {
                SearchResults = await engine.RetrieveResultsFromDatabase(search);
            }

            // populate listbox
            lbl_SearchResults.Text = $"Resultater ({SearchResults.Count})";
            lb_SearchResults.DataSource = SearchResults;
            lb_SearchResults.DisplayMember = "EventDescription";
            UpdateButtonState();
        }

        private async void btn_OpenLogFile_Click(object sender, EventArgs e)
        {
            var selectedLine = lb_SearchResults.SelectedItem as LogLine;
            await engine.OpenLogFile(selectedLine);
        }
        #endregion

        #region listbox event-handlers
        private void lb_SourceSystemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_SourceFocus_Directory.Text = "";
            SelectedSourceSystem = null;

            UpdateButtonState();
        }

        private async void lb_SearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_SearchResults.SelectedIndex < 0)
            {
                tb_SearchResults_SelectedLine.Text = "";
                return;
            }

            var selectedLine = (LogLine)lb_SearchResults.SelectedItem;
            tb_SearchResults_SelectedLine.Text = selectedLine.Rawtext;

            await DisplaySelectedLine();
            UpdateButtonState();
        }
        #endregion

        #region helpers
        private void UpdateButtonState()
        {
            var uiEnable = engine.UiEnable;
            var ss_UserHasSelected = SelectedSourceSystem != null;
            var ss_ItemSelected = lb_SourceSystemList.SelectedItem != null;
            var results_ItemSelected = lb_SearchResults.SelectedItem != null;

            btn_SelectSource.Enabled = ss_ItemSelected && uiEnable;
            btn_AddNewSource.Enabled = uiEnable;
            btn_DeleteSource.Enabled = ss_ItemSelected && ss_UserHasSelected && uiEnable;
            btn_UpdateSource.Enabled = ss_ItemSelected && ss_UserHasSelected && uiEnable;

            btn_ExecuteSearch.Enabled = ss_ItemSelected && ss_UserHasSelected && uiEnable;
            btn_OpenLogFile.Enabled = results_ItemSelected;

            btn_SourceFocus_DirectorySave.Enabled = string.IsNullOrEmpty(tb_SourceFocus_Directory.Text);

            if (!results_ItemSelected) lbl_SearchResults_SelectedLine.Text = "";
        }

        private async Task DisplaySelectedLine()
        {
            if (lb_SearchResults.SelectedItem == null) 
            {
                lbl_SearchResults_SelectedLine.Text = "";
                return; 
            }

            var sourceSystems = await engine.GetAllSourceSystems();
            var ss = sourceSystems.Where(ss => ss.ID == (lb_SearchResults.SelectedItem as LogLine).SourceSystemID).FirstOrDefault();

            lbl_SearchResults_SelectedLine.Text = $"System: {ss.Name}" ?? "";
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOK = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.Text = title;
            label.Text = promptText;

            buttonOK.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOK.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(36, 36, 372, 13);
            textBox.SetBounds(36, 86, 700, 20);
            buttonOK.SetBounds(228, 160, 160, 60);
            buttonCancel.SetBounds(400, 160, 160, 60);

            label.AutoSize = true;
            form.ClientSize = new Size(796, 307);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            form.Controls.AddRange(new Control[] { label, textBox, buttonOK, buttonCancel });
            form.AcceptButton = buttonOK;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;
        }

        public static DateTime ComposeTimeElements((DateTime YearMonthDay, int Hour, int Min) dateHourMin)
        {
            var _year = dateHourMin.YearMonthDay.Year;
            var _month = dateHourMin.YearMonthDay.Month;
            var _day = dateHourMin.YearMonthDay.Day;
            var _resultTime = new DateTime(_year, _month, _day, dateHourMin.Hour, dateHourMin.Min, 0);

            return _resultTime;
        }

        public SearchSet BuildSearchSet()
        {
            var searchSet = new SearchSet();

            foreach (var element in lb_SourceSystemList.SelectedItems)
            {
                searchSet.SourceSystems.Add((SourceSystem)element);
            }

            (DateTime YearMonthDay, int Hour, int Min) start = (dt_DayStart.Value, (int)nud_StartHours.Value, (int)nud_StartMin.Value);
            (DateTime YearMonthDay, int Hour, int Min) end = (dt_DayEnd.Value, (int)nud_EndHours.Value, (int)nud_EndMin.Value);

            searchSet.SearchPeriod = (ComposeTimeElements(start), ComposeTimeElements(end));

            searchSet.KeyWordList = tb_SearchTerms.Text.Trim();

            return searchSet;
        }
        #endregion

    }
}