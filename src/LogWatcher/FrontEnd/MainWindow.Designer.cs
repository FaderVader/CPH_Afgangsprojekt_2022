namespace FrontEnd
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_SourceSystemList = new System.Windows.Forms.ListBox();
            this.tbc_SourceFocus = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_SourceFocus_Browse = new System.Windows.Forms.Button();
            this.btn_SourceFocus_DirectorySave = new System.Windows.Forms.Button();
            this.tb_SourceFocus_Directory = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_SourceFocus_LineTemplate = new System.Windows.Forms.TextBox();
            this.btn_SelectSource = new System.Windows.Forms.Button();
            this.btn_AddNewSource = new System.Windows.Forms.Button();
            this.btn_DeleteSource = new System.Windows.Forms.Button();
            this.btn_UpdateSource = new System.Windows.Forms.Button();
            this.gb_Search = new System.Windows.Forms.GroupBox();
            this.lbl_EndMin = new System.Windows.Forms.Label();
            this.lbl_EndHours = new System.Windows.Forms.Label();
            this.lbl_StartMin = new System.Windows.Forms.Label();
            this.lbl_StartHours = new System.Windows.Forms.Label();
            this.btn_ExecuteSearch = new System.Windows.Forms.Button();
            this.lbl_PeriodEnd = new System.Windows.Forms.Label();
            this.nud_EndMin = new System.Windows.Forms.NumericUpDown();
            this.nud_EndHours = new System.Windows.Forms.NumericUpDown();
            this.dt_DayEnd = new System.Windows.Forms.DateTimePicker();
            this.lbl_PeriodStart = new System.Windows.Forms.Label();
            this.nud_StartMin = new System.Windows.Forms.NumericUpDown();
            this.nud_StartHours = new System.Windows.Forms.NumericUpDown();
            this.dt_DayStart = new System.Windows.Forms.DateTimePicker();
            this.lbl_SearchTermsInput = new System.Windows.Forms.Label();
            this.tb_SearchTerms = new System.Windows.Forms.TextBox();
            this.lbl_SourceSelect = new System.Windows.Forms.Label();
            this.lb_SearchResults = new System.Windows.Forms.ListBox();
            this.lbl_SearchResults = new System.Windows.Forms.Label();
            this.tb_SelectedLine = new System.Windows.Forms.TextBox();
            this.btn_OpenLogFile = new System.Windows.Forms.Button();
            this.lbl_SelectedLine = new System.Windows.Forms.Label();
            this.tbc_SourceFocus.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gb_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_EndMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_EndHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_StartMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_StartHours)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_SourceSystemList
            // 
            this.lb_SourceSystemList.FormattingEnabled = true;
            this.lb_SourceSystemList.ItemHeight = 15;
            this.lb_SourceSystemList.Location = new System.Drawing.Point(12, 53);
            this.lb_SourceSystemList.Name = "lb_SourceSystemList";
            this.lb_SourceSystemList.ScrollAlwaysVisible = true;
            this.lb_SourceSystemList.Size = new System.Drawing.Size(318, 289);
            this.lb_SourceSystemList.TabIndex = 0;
            // 
            // tbc_SourceFocus
            // 
            this.tbc_SourceFocus.Controls.Add(this.tabPage1);
            this.tbc_SourceFocus.Controls.Add(this.tabPage2);
            this.tbc_SourceFocus.Location = new System.Drawing.Point(12, 412);
            this.tbc_SourceFocus.Name = "tbc_SourceFocus";
            this.tbc_SourceFocus.SelectedIndex = 0;
            this.tbc_SourceFocus.Size = new System.Drawing.Size(318, 115);
            this.tbc_SourceFocus.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_SourceFocus_Browse);
            this.tabPage1.Controls.Add(this.btn_SourceFocus_DirectorySave);
            this.tabPage1.Controls.Add(this.tb_SourceFocus_Directory);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(310, 87);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kildefolder";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_SourceFocus_Browse
            // 
            this.btn_SourceFocus_Browse.Location = new System.Drawing.Point(6, 35);
            this.btn_SourceFocus_Browse.Name = "btn_SourceFocus_Browse";
            this.btn_SourceFocus_Browse.Size = new System.Drawing.Size(59, 23);
            this.btn_SourceFocus_Browse.TabIndex = 2;
            this.btn_SourceFocus_Browse.Text = "Browse";
            this.btn_SourceFocus_Browse.UseVisualStyleBackColor = true;
            this.btn_SourceFocus_Browse.Click += new System.EventHandler(this.btn_SourceFocus_Browse_Click);
            // 
            // btn_SourceFocus_DirectorySave
            // 
            this.btn_SourceFocus_DirectorySave.Location = new System.Drawing.Point(229, 53);
            this.btn_SourceFocus_DirectorySave.Name = "btn_SourceFocus_DirectorySave";
            this.btn_SourceFocus_DirectorySave.Size = new System.Drawing.Size(75, 23);
            this.btn_SourceFocus_DirectorySave.TabIndex = 1;
            this.btn_SourceFocus_DirectorySave.Text = "Gem";
            this.btn_SourceFocus_DirectorySave.UseVisualStyleBackColor = true;
            this.btn_SourceFocus_DirectorySave.Click += new System.EventHandler(this.btn_SourceFocus_DirectorySave_Click);
            // 
            // tb_SourceFocus_Directory
            // 
            this.tb_SourceFocus_Directory.Location = new System.Drawing.Point(6, 6);
            this.tb_SourceFocus_Directory.Name = "tb_SourceFocus_Directory";
            this.tb_SourceFocus_Directory.Size = new System.Drawing.Size(298, 23);
            this.tb_SourceFocus_Directory.TabIndex = 0;
            this.tb_SourceFocus_Directory.Text = "Vælg folder ...";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_SourceFocus_LineTemplate);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(310, 87);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Linje skabelon";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_SourceFocus_LineTemplate
            // 
            this.tb_SourceFocus_LineTemplate.Location = new System.Drawing.Point(9, 11);
            this.tb_SourceFocus_LineTemplate.Name = "tb_SourceFocus_LineTemplate";
            this.tb_SourceFocus_LineTemplate.Size = new System.Drawing.Size(295, 23);
            this.tb_SourceFocus_LineTemplate.TabIndex = 0;
            // 
            // btn_SelectSource
            // 
            this.btn_SelectSource.Location = new System.Drawing.Point(63, 348);
            this.btn_SelectSource.Name = "btn_SelectSource";
            this.btn_SelectSource.Size = new System.Drawing.Size(58, 23);
            this.btn_SelectSource.TabIndex = 2;
            this.btn_SelectSource.Text = "Vælg";
            this.btn_SelectSource.UseVisualStyleBackColor = true;
            this.btn_SelectSource.Click += new System.EventHandler(this.btn_SelectSource_Click);
            // 
            // btn_AddNewSource
            // 
            this.btn_AddNewSource.Location = new System.Drawing.Point(127, 348);
            this.btn_AddNewSource.Name = "btn_AddNewSource";
            this.btn_AddNewSource.Size = new System.Drawing.Size(53, 23);
            this.btn_AddNewSource.TabIndex = 3;
            this.btn_AddNewSource.Text = "Ny";
            this.btn_AddNewSource.UseVisualStyleBackColor = true;
            // 
            // btn_DeleteSource
            // 
            this.btn_DeleteSource.Location = new System.Drawing.Point(186, 348);
            this.btn_DeleteSource.Name = "btn_DeleteSource";
            this.btn_DeleteSource.Size = new System.Drawing.Size(57, 23);
            this.btn_DeleteSource.TabIndex = 4;
            this.btn_DeleteSource.Text = "Slet";
            this.btn_DeleteSource.UseVisualStyleBackColor = true;
            // 
            // btn_UpdateSource
            // 
            this.btn_UpdateSource.Location = new System.Drawing.Point(251, 349);
            this.btn_UpdateSource.Name = "btn_UpdateSource";
            this.btn_UpdateSource.Size = new System.Drawing.Size(75, 23);
            this.btn_UpdateSource.TabIndex = 5;
            this.btn_UpdateSource.Text = "Opdater";
            this.btn_UpdateSource.UseVisualStyleBackColor = true;
            // 
            // gb_Search
            // 
            this.gb_Search.Controls.Add(this.lbl_EndMin);
            this.gb_Search.Controls.Add(this.lbl_EndHours);
            this.gb_Search.Controls.Add(this.lbl_StartMin);
            this.gb_Search.Controls.Add(this.lbl_StartHours);
            this.gb_Search.Controls.Add(this.btn_ExecuteSearch);
            this.gb_Search.Controls.Add(this.lbl_PeriodEnd);
            this.gb_Search.Controls.Add(this.nud_EndMin);
            this.gb_Search.Controls.Add(this.nud_EndHours);
            this.gb_Search.Controls.Add(this.dt_DayEnd);
            this.gb_Search.Controls.Add(this.lbl_PeriodStart);
            this.gb_Search.Controls.Add(this.nud_StartMin);
            this.gb_Search.Controls.Add(this.nud_StartHours);
            this.gb_Search.Controls.Add(this.dt_DayStart);
            this.gb_Search.Controls.Add(this.lbl_SearchTermsInput);
            this.gb_Search.Controls.Add(this.tb_SearchTerms);
            this.gb_Search.Location = new System.Drawing.Point(353, 31);
            this.gb_Search.Name = "gb_Search";
            this.gb_Search.Size = new System.Drawing.Size(285, 496);
            this.gb_Search.TabIndex = 6;
            this.gb_Search.TabStop = false;
            this.gb_Search.Text = "Søgning";
            // 
            // lbl_EndMin
            // 
            this.lbl_EndMin.AutoSize = true;
            this.lbl_EndMin.Location = new System.Drawing.Point(159, 372);
            this.lbl_EndMin.Name = "lbl_EndMin";
            this.lbl_EndMin.Size = new System.Drawing.Size(53, 15);
            this.lbl_EndMin.TabIndex = 14;
            this.lbl_EndMin.Text = "Minutter";
            // 
            // lbl_EndHours
            // 
            this.lbl_EndHours.AutoSize = true;
            this.lbl_EndHours.Location = new System.Drawing.Point(5, 372);
            this.lbl_EndHours.Name = "lbl_EndHours";
            this.lbl_EndHours.Size = new System.Drawing.Size(37, 15);
            this.lbl_EndHours.TabIndex = 13;
            this.lbl_EndHours.Text = "Timer";
            // 
            // lbl_StartMin
            // 
            this.lbl_StartMin.AutoSize = true;
            this.lbl_StartMin.Location = new System.Drawing.Point(159, 246);
            this.lbl_StartMin.Name = "lbl_StartMin";
            this.lbl_StartMin.Size = new System.Drawing.Size(53, 15);
            this.lbl_StartMin.TabIndex = 12;
            this.lbl_StartMin.Text = "Minutter";
            // 
            // lbl_StartHours
            // 
            this.lbl_StartHours.AutoSize = true;
            this.lbl_StartHours.Location = new System.Drawing.Point(5, 246);
            this.lbl_StartHours.Name = "lbl_StartHours";
            this.lbl_StartHours.Size = new System.Drawing.Size(37, 15);
            this.lbl_StartHours.TabIndex = 11;
            this.lbl_StartHours.Text = "Timer";
            // 
            // btn_ExecuteSearch
            // 
            this.btn_ExecuteSearch.Location = new System.Drawing.Point(52, 458);
            this.btn_ExecuteSearch.Name = "btn_ExecuteSearch";
            this.btn_ExecuteSearch.Size = new System.Drawing.Size(178, 23);
            this.btn_ExecuteSearch.TabIndex = 10;
            this.btn_ExecuteSearch.Text = "Søg nu";
            this.btn_ExecuteSearch.UseVisualStyleBackColor = true;
            // 
            // lbl_PeriodEnd
            // 
            this.lbl_PeriodEnd.AutoSize = true;
            this.lbl_PeriodEnd.Location = new System.Drawing.Point(5, 326);
            this.lbl_PeriodEnd.Name = "lbl_PeriodEnd";
            this.lbl_PeriodEnd.Size = new System.Drawing.Size(78, 15);
            this.lbl_PeriodEnd.TabIndex = 9;
            this.lbl_PeriodEnd.Text = "Periode - Slut";
            // 
            // nud_EndMin
            // 
            this.nud_EndMin.Location = new System.Drawing.Point(159, 390);
            this.nud_EndMin.Name = "nud_EndMin";
            this.nud_EndMin.Size = new System.Drawing.Size(120, 23);
            this.nud_EndMin.TabIndex = 8;
            // 
            // nud_EndHours
            // 
            this.nud_EndHours.Location = new System.Drawing.Point(6, 390);
            this.nud_EndHours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nud_EndHours.Name = "nud_EndHours";
            this.nud_EndHours.Size = new System.Drawing.Size(120, 23);
            this.nud_EndHours.TabIndex = 7;
            // 
            // dt_DayEnd
            // 
            this.dt_DayEnd.Location = new System.Drawing.Point(6, 344);
            this.dt_DayEnd.Name = "dt_DayEnd";
            this.dt_DayEnd.Size = new System.Drawing.Size(273, 23);
            this.dt_DayEnd.TabIndex = 6;
            // 
            // lbl_PeriodStart
            // 
            this.lbl_PeriodStart.AutoSize = true;
            this.lbl_PeriodStart.Location = new System.Drawing.Point(5, 199);
            this.lbl_PeriodStart.Name = "lbl_PeriodStart";
            this.lbl_PeriodStart.Size = new System.Drawing.Size(82, 15);
            this.lbl_PeriodStart.TabIndex = 5;
            this.lbl_PeriodStart.Text = "Periode - Start";
            // 
            // nud_StartMin
            // 
            this.nud_StartMin.Location = new System.Drawing.Point(159, 264);
            this.nud_StartMin.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nud_StartMin.Name = "nud_StartMin";
            this.nud_StartMin.Size = new System.Drawing.Size(120, 23);
            this.nud_StartMin.TabIndex = 4;
            // 
            // nud_StartHours
            // 
            this.nud_StartHours.Location = new System.Drawing.Point(6, 264);
            this.nud_StartHours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nud_StartHours.Name = "nud_StartHours";
            this.nud_StartHours.Size = new System.Drawing.Size(120, 23);
            this.nud_StartHours.TabIndex = 3;
            // 
            // dt_DayStart
            // 
            this.dt_DayStart.Location = new System.Drawing.Point(6, 218);
            this.dt_DayStart.Name = "dt_DayStart";
            this.dt_DayStart.Size = new System.Drawing.Size(273, 23);
            this.dt_DayStart.TabIndex = 2;
            // 
            // lbl_SearchTermsInput
            // 
            this.lbl_SearchTermsInput.AutoSize = true;
            this.lbl_SearchTermsInput.Location = new System.Drawing.Point(6, 26);
            this.lbl_SearchTermsInput.Name = "lbl_SearchTermsInput";
            this.lbl_SearchTermsInput.Size = new System.Drawing.Size(51, 15);
            this.lbl_SearchTermsInput.TabIndex = 1;
            this.lbl_SearchTermsInput.Text = "Søgeord";
            // 
            // tb_SearchTerms
            // 
            this.tb_SearchTerms.Location = new System.Drawing.Point(6, 47);
            this.tb_SearchTerms.Multiline = true;
            this.tb_SearchTerms.Name = "tb_SearchTerms";
            this.tb_SearchTerms.Size = new System.Drawing.Size(273, 91);
            this.tb_SearchTerms.TabIndex = 0;
            // 
            // lbl_SourceSelect
            // 
            this.lbl_SourceSelect.AutoSize = true;
            this.lbl_SourceSelect.Location = new System.Drawing.Point(12, 31);
            this.lbl_SourceSelect.Name = "lbl_SourceSelect";
            this.lbl_SourceSelect.Size = new System.Drawing.Size(74, 15);
            this.lbl_SourceSelect.TabIndex = 7;
            this.lbl_SourceSelect.Text = "Vælg System";
            // 
            // lb_SearchResults
            // 
            this.lb_SearchResults.FormattingEnabled = true;
            this.lb_SearchResults.ItemHeight = 15;
            this.lb_SearchResults.Location = new System.Drawing.Point(656, 53);
            this.lb_SearchResults.Name = "lb_SearchResults";
            this.lb_SearchResults.ScrollAlwaysVisible = true;
            this.lb_SearchResults.Size = new System.Drawing.Size(364, 334);
            this.lb_SearchResults.TabIndex = 8;
            // 
            // lbl_SearchResults
            // 
            this.lbl_SearchResults.AutoSize = true;
            this.lbl_SearchResults.Location = new System.Drawing.Point(656, 31);
            this.lbl_SearchResults.Name = "lbl_SearchResults";
            this.lbl_SearchResults.Size = new System.Drawing.Size(59, 15);
            this.lbl_SearchResults.TabIndex = 9;
            this.lbl_SearchResults.Text = "Resultater";
            // 
            // tb_SelectedLine
            // 
            this.tb_SelectedLine.Location = new System.Drawing.Point(658, 425);
            this.tb_SelectedLine.Multiline = true;
            this.tb_SelectedLine.Name = "tb_SelectedLine";
            this.tb_SelectedLine.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tb_SelectedLine.Size = new System.Drawing.Size(362, 57);
            this.tb_SelectedLine.TabIndex = 10;
            // 
            // btn_OpenLogFile
            // 
            this.btn_OpenLogFile.Location = new System.Drawing.Point(915, 489);
            this.btn_OpenLogFile.Name = "btn_OpenLogFile";
            this.btn_OpenLogFile.Size = new System.Drawing.Size(105, 23);
            this.btn_OpenLogFile.TabIndex = 11;
            this.btn_OpenLogFile.Text = "Åbn logfil";
            this.btn_OpenLogFile.UseVisualStyleBackColor = true;
            // 
            // lbl_SelectedLine
            // 
            this.lbl_SelectedLine.AutoSize = true;
            this.lbl_SelectedLine.Location = new System.Drawing.Point(658, 403);
            this.lbl_SelectedLine.Name = "lbl_SelectedLine";
            this.lbl_SelectedLine.Size = new System.Drawing.Size(58, 15);
            this.lbl_SelectedLine.TabIndex = 12;
            this.lbl_SelectedLine.Text = "Valgt linje";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 555);
            this.Controls.Add(this.lbl_SelectedLine);
            this.Controls.Add(this.btn_OpenLogFile);
            this.Controls.Add(this.tb_SelectedLine);
            this.Controls.Add(this.lbl_SearchResults);
            this.Controls.Add(this.lb_SearchResults);
            this.Controls.Add(this.lbl_SourceSelect);
            this.Controls.Add(this.gb_Search);
            this.Controls.Add(this.btn_UpdateSource);
            this.Controls.Add(this.btn_DeleteSource);
            this.Controls.Add(this.btn_AddNewSource);
            this.Controls.Add(this.btn_SelectSource);
            this.Controls.Add(this.tbc_SourceFocus);
            this.Controls.Add(this.lb_SourceSystemList);
            this.Name = "MainWindow";
            this.Text = "LogWatcher";
            this.tbc_SourceFocus.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.gb_Search.ResumeLayout(false);
            this.gb_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_EndMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_EndHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_StartMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_StartHours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_SourceSystemList;
        private System.Windows.Forms.TabControl tbc_SourceFocus;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_SelectSource;
        private System.Windows.Forms.Button btn_AddNewSource;
        private System.Windows.Forms.Button btn_DeleteSource;
        private System.Windows.Forms.Button btn_UpdateSource;
        private System.Windows.Forms.GroupBox gb_Search;
        private System.Windows.Forms.DateTimePicker dt_DayStart;
        private System.Windows.Forms.Label lbl_SearchTermsInput;
        private System.Windows.Forms.TextBox tb_SearchTerms;
        private System.Windows.Forms.Label lbl_SourceSelect;
        private System.Windows.Forms.Label lbl_PeriodStart;
        private System.Windows.Forms.NumericUpDown nud_StartMin;
        private System.Windows.Forms.NumericUpDown nud_StartHours;
        private System.Windows.Forms.Label lbl_StartMin;
        private System.Windows.Forms.Label lbl_StartHours;
        private System.Windows.Forms.Button btn_ExecuteSearch;
        private System.Windows.Forms.Label lbl_PeriodEnd;
        private System.Windows.Forms.NumericUpDown nud_EndMin;
        private System.Windows.Forms.NumericUpDown nud_EndHours;
        private System.Windows.Forms.DateTimePicker dt_DayEnd;
        private System.Windows.Forms.Label lbl_EndMin;
        private System.Windows.Forms.Label lbl_EndHours;
        private System.Windows.Forms.ListBox lb_SearchResults;
        private System.Windows.Forms.Label lbl_SearchResults;
        private System.Windows.Forms.TextBox tb_SelectedLine;
        private System.Windows.Forms.Button btn_OpenLogFile;
        private System.Windows.Forms.Label lbl_SelectedLine;
        private System.Windows.Forms.Button btn_SourceFocus_Browse;
        private System.Windows.Forms.Button btn_SourceFocus_DirectorySave;
        private System.Windows.Forms.TextBox tb_SourceFocus_Directory;
        private System.Windows.Forms.TextBox tb_SourceFocus_LineTemplate;
    }
}
