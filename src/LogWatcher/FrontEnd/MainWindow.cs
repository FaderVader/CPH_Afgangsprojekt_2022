using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private async Task InitializeWindow()
        {

        }

        public List<SourceSystem> SourceSystems { get; set; }

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
    }
}
