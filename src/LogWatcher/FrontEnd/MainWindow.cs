using Domain;
using Domain.API;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            engine = new Engine();

            PopulateSourceSystems();
            EnableSourceModifierButtons(false);
        }

        #region fields
        private Engine engine;
        #endregion

        #region properties
        public List<SourceSystem> SourceSystems { get; set; }

        private SourceSystem selectedSourceSystem;
        public SourceSystem SelectedSourceSystem
        {
            get { return selectedSourceSystem; }
            private set
            {
                selectedSourceSystem = value;
                var enable = selectedSourceSystem == null ? false : true;
                EnableSourceModifierButtons(enable);
            }
        }

        public List<LogLine> SearchResults { get; set; }
        #endregion

        #region methods
        public async Task PopulateSourceSystems()
        {
            SourceSystems = await engine.GetAllSourceSystems();
            lb_SourceSystemList.DisplayMember = "Name";
            lb_SourceSystemList.DataSource = SourceSystems;
        }

        public async Task RetrieveResult()
        {


            // query API for result at interval until result != None
            SearchResults = await engine.RetrieveResultsFromParser();

            // populate listbox
            lb_SearchResults.DataSource = SearchResults;
            lb_SearchResults.DisplayMember = "EventDescription";
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
                    SourceSystem newSource = new SourceSystem { Name = value };
                    await engine.AddSourceSystem(newSource);
                    await PopulateSourceSystems();
                }
            }
        }

        private async void btn_DeleteSource_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null) return;

            SelectedSourceSystem = lb_SourceSystemList.SelectedItem as SourceSystem;
            await engine.RemoveSourceSystem(SelectedSourceSystem);

            await PopulateSourceSystems();
        }

        private async void btn_UpdateSource_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null) return;
            SelectedSourceSystem = lb_SourceSystemList.SelectedItem as SourceSystem;
            await engine.UpdateSourceSystem(SelectedSourceSystem);
        }

        private void lb_SourceSystemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItem == null)
            {
                SelectedSourceSystem = null;
                return;
            }

            if (lb_SourceSystemList.SelectedItem != SelectedSourceSystem)
            {
                EnableSourceModifierButtons(false);
            }
        }

        private void lb_SearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_SearchResults.SelectedIndex < 0) return;

            var selectedLine = (LogLine)lb_SearchResults.SelectedItem;
            tb_SelectedLine.Text = selectedLine.Rawtext;
        }

        private async void btn_ExecuteSearch_Click(object sender, EventArgs e)
        {
            if (lb_SourceSystemList.SelectedItems.Count < 1) return;

            var result = BuildSearchSet();

            // Display "Searching.." in UI
            lbl_SearchResults.Text = "Søger ...";
            tb_SelectedLine.Text = "";

            // Ensure result-list is cleared
            lb_SearchResults.DataSource = null;

            await engine.SendQueryToParser(result);
            await RetrieveResult();

            lbl_SearchResults.Text = $"Resultater ({SearchResults.Count})";
        }

        #endregion

        #region helpers
        private void EnableSourceModifierButtons(bool enable)
        {
            btn_DeleteSource.Enabled = enable;
            btn_SourceFocus_Browse.Enabled = enable;
            btn_UpdateSource.Enabled = enable;
            btn_SourceFocus_DirectorySave.Enabled = enable;
            gb_Search.Enabled = enable;
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