using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace FrontEnd
{
    public partial class MainWindow : Form
    {
        private Engine engine;
        public MainWindow()
        {
            InitializeComponent();
            engine = new Engine();

            PopulateSourceSystems();
        }

        public List<SourceSystem> SourceSystems { get; set; }
        public SourceSystem SelectedSourceSystem { get; private set; }

        public async Task GetStarted()
        {
            //var newSource = new SourceSystem { Name = "GalaxySiteSelector", SourceFolder = @"C:\temp\logfiles", LineTemplate = "" };
            //await engine.AddSourceSystem(newSource);

            //var existingSource = new SourceSystem { ID = 1, Name = "GalaxySiteSelector", SourceFolder = @"C:\temp\logfiles", LineTemplate = "" };
            //await engine.UpdateFilesFromSourceSystem(existingSource);

        }

        public async Task PopulateSourceSystems()
        {
            SourceSystems = await engine.GetAllSourceSystems();
            lb_SourceSystemList.DisplayMember = "Name";
            lb_SourceSystemList.DataSource = SourceSystems;
        }

        #region button handlers
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
                    SourceSystem newSource = new SourceSystem { Name= value };
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
        #endregion

        #region helpers
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
        #endregion

    }
}
