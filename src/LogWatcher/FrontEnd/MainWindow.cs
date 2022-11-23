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

            GetStarted();
        }

        public async Task GetStarted()
        {
            var newSource = new SourceSystem { Name = "GalaxySiteSelector", SourceFolder = @"C:\temp\logfiles", LineTemplate = "" };
            await engine.AddSourceSystem(newSource);

            //await engine.UpdateFilesFromSourceSystem(sourceTest);

        }



    }
}
