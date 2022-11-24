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
            lb_SourceSystemList.Items.Clear();
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
        #endregion
    }
}
