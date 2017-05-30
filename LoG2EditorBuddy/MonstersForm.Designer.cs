namespace EditorBuddyMonster
{
    partial class Monsters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monsters));
            this.label_innovation = new System.Windows.Forms.Label();
            this.label_userplacement = new System.Windows.Forms.Label();
            this.label_objective = new System.Windows.Forms.Label();
            this.trackBar_innovation = new System.Windows.Forms.TrackBar();
            this.trackBar_userplacement = new System.Windows.Forms.TrackBar();
            this.trackBar_objective = new System.Windows.Forms.TrackBar();
            this.groupBox_mainsliders = new System.Windows.Forms.GroupBox();
            this.groupBox_objectives = new System.Windows.Forms.GroupBox();
            this.numericUpDown_numberItens = new System.Windows.Forms.NumericUpDown();
            this.label_maxitens = new System.Windows.Forms.Label();
            this.numericUpDown_maxmonsters = new System.Windows.Forms.NumericUpDown();
            this.trackBar_hordes = new System.Windows.Forms.TrackBar();
            this.label_hordes = new System.Windows.Forms.Label();
            this.label_maxmonsters = new System.Windows.Forms.Label();
            this.textBox_logger = new System.Windows.Forms.TextBox();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.button_next = new System.Windows.Forms.Button();
            this.button_previous = new System.Windows.Forms.Button();
            this.groupBox_selection = new System.Windows.Forms.GroupBox();
            this.button_select = new System.Windows.Forms.Button();
            this.button_export = new System.Windows.Forms.Button();
            this.button_invert = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_undo = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectProjectDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uISettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip_panel = new System.Windows.Forms.ToolTip(this.components);
            this.button_newSuggestion = new System.Windows.Forms.Button();
            this.trackBar_history = new System.Windows.Forms.TrackBar();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.difficultyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.itemAccessibilityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.areaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolTip_Parameters = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_innovation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_userplacement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_objective)).BeginInit();
            this.groupBox_mainsliders.SuspendLayout();
            this.groupBox_objectives.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberItens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxmonsters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_hordes)).BeginInit();
            this.groupBox_selection.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_history)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label_innovation
            // 
            this.label_innovation.AutoSize = true;
            this.label_innovation.Location = new System.Drawing.Point(16, 31);
            this.label_innovation.Name = "label_innovation";
            this.label_innovation.Size = new System.Drawing.Size(57, 13);
            this.label_innovation.TabIndex = 0;
            this.label_innovation.Text = "Innovation";
            // 
            // label_userplacement
            // 
            this.label_userplacement.AutoSize = true;
            this.label_userplacement.Location = new System.Drawing.Point(98, 31);
            this.label_userplacement.Name = "label_userplacement";
            this.label_userplacement.Size = new System.Drawing.Size(82, 13);
            this.label_userplacement.TabIndex = 1;
            this.label_userplacement.Text = "User Placement";
            // 
            // label_objective
            // 
            this.label_objective.AutoSize = true;
            this.label_objective.Location = new System.Drawing.Point(215, 31);
            this.label_objective.Name = "label_objective";
            this.label_objective.Size = new System.Drawing.Size(52, 13);
            this.label_objective.TabIndex = 2;
            this.label_objective.Text = "Objective";
            // 
            // trackBar_innovation
            // 
            this.trackBar_innovation.Location = new System.Drawing.Point(33, 47);
            this.trackBar_innovation.Maximum = 100;
            this.trackBar_innovation.Name = "trackBar_innovation";
            this.trackBar_innovation.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_innovation.Size = new System.Drawing.Size(45, 120);
            this.trackBar_innovation.SmallChange = 5;
            this.trackBar_innovation.TabIndex = 5;
            this.trackBar_innovation.Tag = "";
            this.trackBar_innovation.TickFrequency = 25;
            this.toolTip_Parameters.SetToolTip(this.trackBar_innovation, "Innovation: a pool that gives suggestions with different positions and types of m" +
        "onsters.");
            this.trackBar_innovation.Value = 100;
            // 
            // trackBar_userplacement
            // 
            this.trackBar_userplacement.Location = new System.Drawing.Point(125, 47);
            this.trackBar_userplacement.Maximum = 100;
            this.trackBar_userplacement.Name = "trackBar_userplacement";
            this.trackBar_userplacement.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_userplacement.Size = new System.Drawing.Size(45, 120);
            this.trackBar_userplacement.TabIndex = 4;
            this.trackBar_userplacement.TickFrequency = 25;
            this.toolTip_Parameters.SetToolTip(this.trackBar_userplacement, "User placement pool gives suggestions that are closer with the base suggestion.");
            this.trackBar_userplacement.Value = 100;
            // 
            // trackBar_objective
            // 
            this.trackBar_objective.Location = new System.Drawing.Point(218, 47);
            this.trackBar_objective.Maximum = 100;
            this.trackBar_objective.Name = "trackBar_objective";
            this.trackBar_objective.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_objective.Size = new System.Drawing.Size(45, 120);
            this.trackBar_objective.TabIndex = 5;
            this.trackBar_objective.TickFrequency = 25;
            this.toolTip_Parameters.SetToolTip(this.trackBar_objective, "Objective is a pool that gives suggestions based on the Objective Paramenters inf" +
        "ormation.");
            this.trackBar_objective.Value = 100;
            // 
            // groupBox_mainsliders
            // 
            this.groupBox_mainsliders.Controls.Add(this.label_innovation);
            this.groupBox_mainsliders.Controls.Add(this.label_objective);
            this.groupBox_mainsliders.Controls.Add(this.trackBar_objective);
            this.groupBox_mainsliders.Controls.Add(this.label_userplacement);
            this.groupBox_mainsliders.Controls.Add(this.trackBar_innovation);
            this.groupBox_mainsliders.Controls.Add(this.trackBar_userplacement);
            this.groupBox_mainsliders.Location = new System.Drawing.Point(12, 36);
            this.groupBox_mainsliders.Name = "groupBox_mainsliders";
            this.groupBox_mainsliders.Size = new System.Drawing.Size(275, 173);
            this.groupBox_mainsliders.TabIndex = 6;
            this.groupBox_mainsliders.TabStop = false;
            this.groupBox_mainsliders.Text = "Algorithm Behaviour";
            this.toolTip_Parameters.SetToolTip(this.groupBox_mainsliders, "Defines the percentage usage of each pool when generating the final suggestion.");
            // 
            // groupBox_objectives
            // 
            this.groupBox_objectives.Controls.Add(this.numericUpDown_numberItens);
            this.groupBox_objectives.Controls.Add(this.label_maxitens);
            this.groupBox_objectives.Controls.Add(this.numericUpDown_maxmonsters);
            this.groupBox_objectives.Controls.Add(this.trackBar_hordes);
            this.groupBox_objectives.Controls.Add(this.label_hordes);
            this.groupBox_objectives.Controls.Add(this.label_maxmonsters);
            this.groupBox_objectives.Location = new System.Drawing.Point(12, 215);
            this.groupBox_objectives.Name = "groupBox_objectives";
            this.groupBox_objectives.Size = new System.Drawing.Size(274, 130);
            this.groupBox_objectives.TabIndex = 7;
            this.groupBox_objectives.TabStop = false;
            this.groupBox_objectives.Text = "Objective Parameters ";
            this.toolTip_Parameters.SetToolTip(this.groupBox_objectives, "Defines parameters that will be used to generate suggestions on the Objective poo" +
        "l, was well on the final suggestion that is presented to the user.");
            // 
            // numericUpDown_numberItens
            // 
            this.numericUpDown_numberItens.Location = new System.Drawing.Point(160, 50);
            this.numericUpDown_numberItens.Name = "numericUpDown_numberItens";
            this.numericUpDown_numberItens.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown_numberItens.TabIndex = 10;
            this.toolTip_panel.SetToolTip(this.numericUpDown_numberItens, "Defines the ideal number of itens that the suggestions should have.");
            this.numericUpDown_numberItens.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label_maxitens
            // 
            this.label_maxitens.AutoSize = true;
            this.label_maxitens.Location = new System.Drawing.Point(11, 52);
            this.label_maxitens.Name = "label_maxitens";
            this.label_maxitens.Size = new System.Drawing.Size(70, 13);
            this.label_maxitens.TabIndex = 9;
            this.label_maxitens.Text = "Number Itens";
            this.toolTip_Parameters.SetToolTip(this.label_maxitens, "Defines the ideal number of itens that the suggestions should have.");
            // 
            // numericUpDown_maxmonsters
            // 
            this.numericUpDown_maxmonsters.Location = new System.Drawing.Point(160, 24);
            this.numericUpDown_maxmonsters.Name = "numericUpDown_maxmonsters";
            this.numericUpDown_maxmonsters.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown_maxmonsters.TabIndex = 8;
            this.toolTip_panel.SetToolTip(this.numericUpDown_maxmonsters, "Defines the ideal number of monsters that the suggestions should have.");
            this.numericUpDown_maxmonsters.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // trackBar_hordes
            // 
            this.trackBar_hordes.Location = new System.Drawing.Point(126, 76);
            this.trackBar_hordes.Maximum = 100;
            this.trackBar_hordes.Name = "trackBar_hordes";
            this.trackBar_hordes.Size = new System.Drawing.Size(142, 45);
            this.trackBar_hordes.TabIndex = 5;
            this.trackBar_hordes.TickFrequency = 25;
            this.toolTip_panel.SetToolTip(this.trackBar_hordes, "Defines the ideal percentage of hordes that the suggestions should have. Hordes a" +
        "re monsters that spaw close to each other.");
            this.trackBar_hordes.Value = 50;
            // 
            // label_hordes
            // 
            this.label_hordes.AutoSize = true;
            this.label_hordes.Location = new System.Drawing.Point(11, 76);
            this.label_hordes.Name = "label_hordes";
            this.label_hordes.Size = new System.Drawing.Size(41, 13);
            this.label_hordes.TabIndex = 2;
            this.label_hordes.Text = "Hordes";
            this.toolTip_Parameters.SetToolTip(this.label_hordes, "Defines the ideal percentage of hordes that the suggestions should have. Hordes a" +
        "re monsters that spaw close to each other.");
            // 
            // label_maxmonsters
            // 
            this.label_maxmonsters.AutoSize = true;
            this.label_maxmonsters.Location = new System.Drawing.Point(11, 26);
            this.label_maxmonsters.Name = "label_maxmonsters";
            this.label_maxmonsters.Size = new System.Drawing.Size(90, 13);
            this.label_maxmonsters.TabIndex = 0;
            this.label_maxmonsters.Text = "Number Monsters";
            this.toolTip_Parameters.SetToolTip(this.label_maxmonsters, "Defines the ideal number of monsters that the suggestions should have.");
            // 
            // textBox_logger
            // 
            this.textBox_logger.Location = new System.Drawing.Point(940, 446);
            this.textBox_logger.Multiline = true;
            this.textBox_logger.Name = "textBox_logger";
            this.textBox_logger.ReadOnly = true;
            this.textBox_logger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_logger.Size = new System.Drawing.Size(213, 231);
            this.textBox_logger.TabIndex = 8;
            // 
            // gridPanel
            // 
            this.gridPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.gridPanel.Location = new System.Drawing.Point(293, 36);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Size = new System.Drawing.Size(641, 641);
            this.gridPanel.TabIndex = 9;
            this.gridPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridPanel_MouseClick);
            // 
            // button_next
            // 
            this.button_next.Enabled = false;
            this.button_next.Location = new System.Drawing.Point(1078, 212);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(75, 23);
            this.button_next.TabIndex = 10;
            this.button_next.Text = "Next";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_previous
            // 
            this.button_previous.Enabled = false;
            this.button_previous.Location = new System.Drawing.Point(947, 212);
            this.button_previous.Name = "button_previous";
            this.button_previous.Size = new System.Drawing.Size(75, 23);
            this.button_previous.TabIndex = 11;
            this.button_previous.Text = "Previous";
            this.button_previous.UseVisualStyleBackColor = true;
            this.button_previous.Click += new System.EventHandler(this.button_previous_Click);
            // 
            // groupBox_selection
            // 
            this.groupBox_selection.Controls.Add(this.button_select);
            this.groupBox_selection.Controls.Add(this.button_export);
            this.groupBox_selection.Controls.Add(this.button_invert);
            this.groupBox_selection.Controls.Add(this.button_clear);
            this.groupBox_selection.Enabled = false;
            this.groupBox_selection.Location = new System.Drawing.Point(940, 36);
            this.groupBox_selection.Name = "groupBox_selection";
            this.groupBox_selection.Size = new System.Drawing.Size(178, 84);
            this.groupBox_selection.TabIndex = 12;
            this.groupBox_selection.TabStop = false;
            this.groupBox_selection.Text = "Selection";
            // 
            // button_select
            // 
            this.button_select.Enabled = false;
            this.button_select.Location = new System.Drawing.Point(7, 20);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 4;
            this.button_select.Text = "Select";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // button_export
            // 
            this.button_export.Enabled = false;
            this.button_export.Location = new System.Drawing.Point(88, 49);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 3;
            this.button_export.Text = "Export";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // button_invert
            // 
            this.button_invert.Location = new System.Drawing.Point(7, 49);
            this.button_invert.Name = "button_invert";
            this.button_invert.Size = new System.Drawing.Size(75, 23);
            this.button_invert.TabIndex = 2;
            this.button_invert.Text = "Invert";
            this.button_invert.UseVisualStyleBackColor = true;
            this.button_invert.Click += new System.EventHandler(this.button_invert_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(88, 20);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 1;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_undo
            // 
            this.button_undo.Enabled = false;
            this.button_undo.Location = new System.Drawing.Point(947, 126);
            this.button_undo.Name = "button_undo";
            this.button_undo.Size = new System.Drawing.Size(75, 23);
            this.button_undo.TabIndex = 0;
            this.button_undo.Text = "Undo";
            this.button_undo.UseVisualStyleBackColor = true;
            // 
            // button_settings
            // 
            this.button_settings.Enabled = false;
            this.button_settings.Location = new System.Drawing.Point(1028, 126);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(75, 23);
            this.button_settings.TabIndex = 13;
            this.button_settings.Text = "Settings";
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.uISettingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1165, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProjectDirectoryToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectProjectDirectoryToolStripMenuItem
            // 
            this.selectProjectDirectoryToolStripMenuItem.Name = "selectProjectDirectoryToolStripMenuItem";
            this.selectProjectDirectoryToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.selectProjectDirectoryToolStripMenuItem.Text = "Select Project Directory";
            this.selectProjectDirectoryToolStripMenuItem.Click += new System.EventHandler(this.selectProjectDirectoryToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // uISettingsToolStripMenuItem
            // 
            this.uISettingsToolStripMenuItem.Name = "uISettingsToolStripMenuItem";
            this.uISettingsToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.uISettingsToolStripMenuItem.Text = "UI Settings";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.creditsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.creditsToolStripMenuItem.Text = "Credits";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "eyeclosed.png");
            this.imageList1.Images.SetKeyName(1, "eye-2x.png");
            // 
            // button_newSuggestion
            // 
            this.button_newSuggestion.Enabled = false;
            this.button_newSuggestion.Location = new System.Drawing.Point(947, 241);
            this.button_newSuggestion.Name = "button_newSuggestion";
            this.button_newSuggestion.Size = new System.Drawing.Size(206, 23);
            this.button_newSuggestion.TabIndex = 18;
            this.button_newSuggestion.Text = "New Suggestion";
            this.button_newSuggestion.UseVisualStyleBackColor = true;
            this.button_newSuggestion.Click += new System.EventHandler(this.button_newRun_Click);
            // 
            // trackBar_history
            // 
            this.trackBar_history.Enabled = false;
            this.trackBar_history.Location = new System.Drawing.Point(947, 164);
            this.trackBar_history.Maximum = 1;
            this.trackBar_history.Name = "trackBar_history";
            this.trackBar_history.Size = new System.Drawing.Size(206, 45);
            this.trackBar_history.TabIndex = 19;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.difficultyDataGridViewTextBoxColumn,
            this.itemAccessibilityDataGridViewTextBoxColumn,
            this.dataGridViewCheckBoxColumn1});
            this.dataGridView.DataSource = this.areaBindingSource;
            this.dataGridView.Location = new System.Drawing.Point(12, 351);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(274, 284);
            this.dataGridView.TabIndex = 20;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView_CurrentCellDirtyStateChanged);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // difficultyDataGridViewTextBoxColumn
            // 
            this.difficultyDataGridViewTextBoxColumn.DataPropertyName = "Difficulty";
            this.difficultyDataGridViewTextBoxColumn.HeaderText = "Dangerous";
            this.difficultyDataGridViewTextBoxColumn.Name = "difficultyDataGridViewTextBoxColumn";
            this.difficultyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.difficultyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // itemAccessibilityDataGridViewTextBoxColumn
            // 
            this.itemAccessibilityDataGridViewTextBoxColumn.DataPropertyName = "ItemAccessibility";
            this.itemAccessibilityDataGridViewTextBoxColumn.HeaderText = "Item Accessibility";
            this.itemAccessibilityDataGridViewTextBoxColumn.Name = "itemAccessibilityDataGridViewTextBoxColumn";
            this.itemAccessibilityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemAccessibilityDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Visible";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Visible";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // areaBindingSource
            // 
            this.areaBindingSource.DataSource = typeof(EditorBuddyMonster.Layers.Area);
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "Difficulty";
            this.dataGridViewComboBoxColumn1.HeaderText = "Difficulty";
            this.dataGridViewComboBoxColumn1.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.DataPropertyName = "Difficulty";
            this.dataGridViewComboBoxColumn2.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewComboBoxColumn2.HeaderText = "Difficulty";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "Visible";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(947, 270);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(206, 15);
            this.progressBar1.TabIndex = 21;
            // 
            // Monsters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 685);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.trackBar_history);
            this.Controls.Add(this.textBox_logger);
            this.Controls.Add(this.button_newSuggestion);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.groupBox_selection);
            this.Controls.Add(this.button_previous);
            this.Controls.Add(this.button_undo);
            this.Controls.Add(this.button_next);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.groupBox_objectives);
            this.Controls.Add(this.groupBox_mainsliders);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Monsters";
            this.Text = "Monsters Placement Prototype";
            this.Load += new System.EventHandler(this.Monsters_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_innovation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_userplacement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_objective)).EndInit();
            this.groupBox_mainsliders.ResumeLayout(false);
            this.groupBox_mainsliders.PerformLayout();
            this.groupBox_objectives.ResumeLayout(false);
            this.groupBox_objectives.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberItens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxmonsters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_hordes)).EndInit();
            this.groupBox_selection.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_history)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_innovation;
        private System.Windows.Forms.Label label_userplacement;
        private System.Windows.Forms.Label label_objective;
        private System.Windows.Forms.TrackBar trackBar_innovation;
        private System.Windows.Forms.TrackBar trackBar_userplacement;
        private System.Windows.Forms.TrackBar trackBar_objective;
        private System.Windows.Forms.GroupBox groupBox_mainsliders;
        private System.Windows.Forms.GroupBox groupBox_objectives;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxmonsters;
        private System.Windows.Forms.TrackBar trackBar_hordes;
        private System.Windows.Forms.Label label_hordes;
        private System.Windows.Forms.Label label_maxmonsters;
        private System.Windows.Forms.TextBox textBox_logger;
        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_previous;
        private System.Windows.Forms.GroupBox groupBox_selection;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.Button button_invert;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_undo;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectProjectDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uISettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip_panel;
        private System.Windows.Forms.Button button_newSuggestion;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.TrackBar trackBar_history;
        private System.Windows.Forms.NumericUpDown numericUpDown_numberItens;
        private System.Windows.Forms.Label label_maxitens;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.BindingSource areaBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn difficultyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn itemAccessibilityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.ToolTip toolTip_Parameters;
    }
}