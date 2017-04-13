﻿namespace Log2CyclePrototype
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
            this.numericUpDown_characterlevel = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_maxmonsters = new System.Windows.Forms.NumericUpDown();
            this.trackBar_endpoints = new System.Windows.Forms.TrackBar();
            this.trackBar_mapobjects = new System.Windows.Forms.TrackBar();
            this.trackBar_hordes = new System.Windows.Forms.TrackBar();
            this.label_endpoints = new System.Windows.Forms.Label();
            this.label_mapobjects = new System.Windows.Forms.Label();
            this.label_hordes = new System.Windows.Forms.Label();
            this.label_expectedlevel = new System.Windows.Forms.Label();
            this.label_maxmonsters = new System.Windows.Forms.Label();
            this.textBox_logger = new System.Windows.Forms.TextBox();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.button_next = new System.Windows.Forms.Button();
            this.button_previous = new System.Windows.Forms.Button();
            this.groupBox_selection = new System.Windows.Forms.GroupBox();
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
            this.groupBox_layer_difficulty = new System.Windows.Forms.GroupBox();
            this.button_visibility_difficulty = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel_palett_difficulty = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox_layer_itens = new System.Windows.Forms.GroupBox();
            this.button_visibility_itens = new System.Windows.Forms.Button();
            this.panel_palette_itens = new System.Windows.Forms.Panel();
            this.groupBox_layer_monsters = new System.Windows.Forms.GroupBox();
            this.button_visibility_monsters = new System.Windows.Forms.Button();
            this.panel_palette_monsters = new System.Windows.Forms.Panel();
            this.groupBox_layer_resources = new System.Windows.Forms.GroupBox();
            this.button_visibility_resources = new System.Windows.Forms.Button();
            this.panel_palette_resources = new System.Windows.Forms.Panel();
            this.toolTip_panel = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_innovation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_userplacement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_objective)).BeginInit();
            this.groupBox_mainsliders.SuspendLayout();
            this.groupBox_objectives.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_characterlevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxmonsters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_endpoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mapobjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_hordes)).BeginInit();
            this.groupBox_selection.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox_layer_difficulty.SuspendLayout();
            this.groupBox_layer_itens.SuspendLayout();
            this.groupBox_layer_monsters.SuspendLayout();
            this.groupBox_layer_resources.SuspendLayout();
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
            this.trackBar_innovation.TabIndex = 3;
            this.trackBar_innovation.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_innovation.Value = 100;
            this.trackBar_innovation.Scroll += new System.EventHandler(this.trackBar_innovation_Scroll);
            // 
            // trackBar_userplacement
            // 
            this.trackBar_userplacement.Location = new System.Drawing.Point(125, 47);
            this.trackBar_userplacement.Maximum = 100;
            this.trackBar_userplacement.Name = "trackBar_userplacement";
            this.trackBar_userplacement.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_userplacement.Size = new System.Drawing.Size(45, 120);
            this.trackBar_userplacement.TabIndex = 4;
            this.trackBar_userplacement.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_userplacement.Value = 100;
            this.trackBar_userplacement.Scroll += new System.EventHandler(this.trackBar_userplacement_Scroll);
            // 
            // trackBar_objective
            // 
            this.trackBar_objective.Location = new System.Drawing.Point(218, 47);
            this.trackBar_objective.Maximum = 100;
            this.trackBar_objective.Name = "trackBar_objective";
            this.trackBar_objective.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_objective.Size = new System.Drawing.Size(45, 120);
            this.trackBar_objective.TabIndex = 5;
            this.trackBar_objective.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_objective.Value = 100;
            this.trackBar_objective.Scroll += new System.EventHandler(this.trackBar_objective_Scroll);
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
            // 
            // groupBox_objectives
            // 
            this.groupBox_objectives.Controls.Add(this.numericUpDown_characterlevel);
            this.groupBox_objectives.Controls.Add(this.numericUpDown_maxmonsters);
            this.groupBox_objectives.Controls.Add(this.trackBar_endpoints);
            this.groupBox_objectives.Controls.Add(this.trackBar_mapobjects);
            this.groupBox_objectives.Controls.Add(this.trackBar_hordes);
            this.groupBox_objectives.Controls.Add(this.label_endpoints);
            this.groupBox_objectives.Controls.Add(this.label_mapobjects);
            this.groupBox_objectives.Controls.Add(this.label_hordes);
            this.groupBox_objectives.Controls.Add(this.label_expectedlevel);
            this.groupBox_objectives.Controls.Add(this.label_maxmonsters);
            this.groupBox_objectives.Location = new System.Drawing.Point(12, 215);
            this.groupBox_objectives.Name = "groupBox_objectives";
            this.groupBox_objectives.Size = new System.Drawing.Size(274, 225);
            this.groupBox_objectives.TabIndex = 7;
            this.groupBox_objectives.TabStop = false;
            this.groupBox_objectives.Text = "Objective Parameters ";
            // 
            // numericUpDown_characterlevel
            // 
            this.numericUpDown_characterlevel.Location = new System.Drawing.Point(164, 53);
            this.numericUpDown_characterlevel.Name = "numericUpDown_characterlevel";
            this.numericUpDown_characterlevel.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown_characterlevel.TabIndex = 9;
            this.numericUpDown_characterlevel.ValueChanged += new System.EventHandler(this.numericUpDown_characterlevel_ValueChanged);
            // 
            // numericUpDown_maxmonsters
            // 
            this.numericUpDown_maxmonsters.Location = new System.Drawing.Point(164, 24);
            this.numericUpDown_maxmonsters.Name = "numericUpDown_maxmonsters";
            this.numericUpDown_maxmonsters.Size = new System.Drawing.Size(103, 20);
            this.numericUpDown_maxmonsters.TabIndex = 8;
            this.numericUpDown_maxmonsters.ValueChanged += new System.EventHandler(this.numericUpDown_maxmonsters_ValueChanged);
            // 
            // trackBar_endpoints
            // 
            this.trackBar_endpoints.Location = new System.Drawing.Point(127, 174);
            this.trackBar_endpoints.Maximum = 100;
            this.trackBar_endpoints.Name = "trackBar_endpoints";
            this.trackBar_endpoints.Size = new System.Drawing.Size(142, 45);
            this.trackBar_endpoints.TabIndex = 7;
            this.trackBar_endpoints.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_endpoints.Scroll += new System.EventHandler(this.trackBar_endpoints_Scroll);
            // 
            // trackBar_mapobjects
            // 
            this.trackBar_mapobjects.Location = new System.Drawing.Point(126, 133);
            this.trackBar_mapobjects.Maximum = 100;
            this.trackBar_mapobjects.Name = "trackBar_mapobjects";
            this.trackBar_mapobjects.Size = new System.Drawing.Size(143, 45);
            this.trackBar_mapobjects.TabIndex = 6;
            this.trackBar_mapobjects.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_mapobjects.Scroll += new System.EventHandler(this.trackBar_mapobjects_Scroll);
            // 
            // trackBar_hordes
            // 
            this.trackBar_hordes.Location = new System.Drawing.Point(125, 94);
            this.trackBar_hordes.Maximum = 100;
            this.trackBar_hordes.Name = "trackBar_hordes";
            this.trackBar_hordes.Size = new System.Drawing.Size(142, 45);
            this.trackBar_hordes.TabIndex = 5;
            this.trackBar_hordes.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_hordes.Scroll += new System.EventHandler(this.trackBar_hordes_Scroll);
            // 
            // label_endpoints
            // 
            this.label_endpoints.AutoSize = true;
            this.label_endpoints.Location = new System.Drawing.Point(11, 174);
            this.label_endpoints.Name = "label_endpoints";
            this.label_endpoints.Size = new System.Drawing.Size(58, 13);
            this.label_endpoints.TabIndex = 4;
            this.label_endpoints.Text = "End Points";
            // 
            // label_mapobjects
            // 
            this.label_mapobjects.AutoSize = true;
            this.label_mapobjects.Location = new System.Drawing.Point(11, 133);
            this.label_mapobjects.Name = "label_mapobjects";
            this.label_mapobjects.Size = new System.Drawing.Size(67, 13);
            this.label_mapobjects.TabIndex = 3;
            this.label_mapobjects.Text = "Map Objects";
            // 
            // label_hordes
            // 
            this.label_hordes.AutoSize = true;
            this.label_hordes.Location = new System.Drawing.Point(11, 94);
            this.label_hordes.Name = "label_hordes";
            this.label_hordes.Size = new System.Drawing.Size(41, 13);
            this.label_hordes.TabIndex = 2;
            this.label_hordes.Text = "Hordes";
            // 
            // label_expectedlevel
            // 
            this.label_expectedlevel.AutoSize = true;
            this.label_expectedlevel.Location = new System.Drawing.Point(11, 55);
            this.label_expectedlevel.Name = "label_expectedlevel";
            this.label_expectedlevel.Size = new System.Drawing.Size(130, 13);
            this.label_expectedlevel.TabIndex = 1;
            this.label_expectedlevel.Text = "Character Expected Level";
            // 
            // label_maxmonsters
            // 
            this.label_maxmonsters.AutoSize = true;
            this.label_maxmonsters.Location = new System.Drawing.Point(11, 26);
            this.label_maxmonsters.Name = "label_maxmonsters";
            this.label_maxmonsters.Size = new System.Drawing.Size(73, 13);
            this.label_maxmonsters.TabIndex = 0;
            this.label_maxmonsters.Text = "Max Monsters";
            // 
            // textBox_logger
            // 
            this.textBox_logger.Location = new System.Drawing.Point(12, 446);
            this.textBox_logger.Multiline = true;
            this.textBox_logger.Name = "textBox_logger";
            this.textBox_logger.ReadOnly = true;
            this.textBox_logger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_logger.Size = new System.Drawing.Size(274, 231);
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
            this.button_next.Location = new System.Drawing.Point(1028, 169);
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
            this.button_previous.Location = new System.Drawing.Point(947, 169);
            this.button_previous.Name = "button_previous";
            this.button_previous.Size = new System.Drawing.Size(75, 23);
            this.button_previous.TabIndex = 11;
            this.button_previous.Text = "Previous";
            this.button_previous.UseVisualStyleBackColor = true;
            this.button_previous.Click += new System.EventHandler(this.button_previous_Click);
            // 
            // groupBox_selection
            // 
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
            // button_export
            // 
            this.button_export.Location = new System.Drawing.Point(7, 49);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 3;
            this.button_export.Text = "Export";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // button_invert
            // 
            this.button_invert.Location = new System.Drawing.Point(88, 20);
            this.button_invert.Name = "button_invert";
            this.button_invert.Size = new System.Drawing.Size(75, 23);
            this.button_invert.TabIndex = 2;
            this.button_invert.Text = "Invert";
            this.button_invert.UseVisualStyleBackColor = true;
            this.button_invert.Click += new System.EventHandler(this.button_invert_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(7, 20);
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
            this.menuStrip1.Size = new System.Drawing.Size(1129, 24);
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
            // groupBox_layer_difficulty
            // 
            this.groupBox_layer_difficulty.Controls.Add(this.button_visibility_difficulty);
            this.groupBox_layer_difficulty.Controls.Add(this.panel_palett_difficulty);
            this.groupBox_layer_difficulty.Enabled = false;
            this.groupBox_layer_difficulty.Location = new System.Drawing.Point(947, 227);
            this.groupBox_layer_difficulty.Name = "groupBox_layer_difficulty";
            this.groupBox_layer_difficulty.Size = new System.Drawing.Size(171, 56);
            this.groupBox_layer_difficulty.TabIndex = 15;
            this.groupBox_layer_difficulty.TabStop = false;
            this.groupBox_layer_difficulty.Text = "Difficulty";
            // 
            // button_visibility_difficulty
            // 
            this.button_visibility_difficulty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_visibility_difficulty.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_visibility_difficulty.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_difficulty.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_difficulty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_visibility_difficulty.ImageIndex = 0;
            this.button_visibility_difficulty.ImageList = this.imageList1;
            this.button_visibility_difficulty.Location = new System.Drawing.Point(134, 24);
            this.button_visibility_difficulty.Name = "button_visibility_difficulty";
            this.button_visibility_difficulty.Size = new System.Drawing.Size(31, 26);
            this.button_visibility_difficulty.TabIndex = 2;
            this.button_visibility_difficulty.UseVisualStyleBackColor = true;
            this.button_visibility_difficulty.Click += new System.EventHandler(this.button_visibility_difficulty_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "eyeclosed.png");
            this.imageList1.Images.SetKeyName(1, "eye-2x.png");
            // 
            // panel_palett_difficulty
            // 
            this.panel_palett_difficulty.Location = new System.Drawing.Point(6, 26);
            this.panel_palett_difficulty.Name = "panel_palett_difficulty";
            this.panel_palett_difficulty.Size = new System.Drawing.Size(90, 25);
            this.panel_palett_difficulty.TabIndex = 0;
            this.panel_palett_difficulty.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_difficulty_click);
            // 
            // groupBox_layer_itens
            // 
            this.groupBox_layer_itens.Controls.Add(this.button_visibility_itens);
            this.groupBox_layer_itens.Controls.Add(this.panel_palette_itens);
            this.groupBox_layer_itens.Enabled = false;
            this.groupBox_layer_itens.Location = new System.Drawing.Point(947, 289);
            this.groupBox_layer_itens.Name = "groupBox_layer_itens";
            this.groupBox_layer_itens.Size = new System.Drawing.Size(171, 56);
            this.groupBox_layer_itens.TabIndex = 16;
            this.groupBox_layer_itens.TabStop = false;
            this.groupBox_layer_itens.Text = "Itens";
            // 
            // button_visibility_itens
            // 
            this.button_visibility_itens.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_visibility_itens.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_visibility_itens.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_itens.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_itens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_visibility_itens.ImageIndex = 0;
            this.button_visibility_itens.ImageList = this.imageList1;
            this.button_visibility_itens.Location = new System.Drawing.Point(134, 24);
            this.button_visibility_itens.Name = "button_visibility_itens";
            this.button_visibility_itens.Size = new System.Drawing.Size(31, 26);
            this.button_visibility_itens.TabIndex = 2;
            this.button_visibility_itens.UseVisualStyleBackColor = true;
            this.button_visibility_itens.Click += new System.EventHandler(this.button_visibility_itens_Click);
            // 
            // panel_palette_itens
            // 
            this.panel_palette_itens.Location = new System.Drawing.Point(6, 26);
            this.panel_palette_itens.Name = "panel_palette_itens";
            this.panel_palette_itens.Size = new System.Drawing.Size(90, 25);
            this.panel_palette_itens.TabIndex = 0;
            this.panel_palette_itens.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_itens_click);
            // 
            // groupBox_layer_monsters
            // 
            this.groupBox_layer_monsters.Controls.Add(this.button_visibility_monsters);
            this.groupBox_layer_monsters.Controls.Add(this.panel_palette_monsters);
            this.groupBox_layer_monsters.Enabled = false;
            this.groupBox_layer_monsters.Location = new System.Drawing.Point(947, 351);
            this.groupBox_layer_monsters.Name = "groupBox_layer_monsters";
            this.groupBox_layer_monsters.Size = new System.Drawing.Size(171, 56);
            this.groupBox_layer_monsters.TabIndex = 17;
            this.groupBox_layer_monsters.TabStop = false;
            this.groupBox_layer_monsters.Text = "Monsters";
            // 
            // button_visibility_monsters
            // 
            this.button_visibility_monsters.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_visibility_monsters.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_visibility_monsters.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_monsters.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_monsters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_visibility_monsters.ImageIndex = 0;
            this.button_visibility_monsters.ImageList = this.imageList1;
            this.button_visibility_monsters.Location = new System.Drawing.Point(134, 24);
            this.button_visibility_monsters.Name = "button_visibility_monsters";
            this.button_visibility_monsters.Size = new System.Drawing.Size(31, 26);
            this.button_visibility_monsters.TabIndex = 2;
            this.button_visibility_monsters.UseVisualStyleBackColor = true;
            this.button_visibility_monsters.Click += new System.EventHandler(this.button_visibility_monsters_Click);
            // 
            // panel_palette_monsters
            // 
            this.panel_palette_monsters.Location = new System.Drawing.Point(6, 26);
            this.panel_palette_monsters.Name = "panel_palette_monsters";
            this.panel_palette_monsters.Size = new System.Drawing.Size(90, 25);
            this.panel_palette_monsters.TabIndex = 0;
            this.panel_palette_monsters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_monsters_click);
            // 
            // groupBox_layer_resources
            // 
            this.groupBox_layer_resources.Controls.Add(this.button_visibility_resources);
            this.groupBox_layer_resources.Controls.Add(this.panel_palette_resources);
            this.groupBox_layer_resources.Enabled = false;
            this.groupBox_layer_resources.Location = new System.Drawing.Point(947, 413);
            this.groupBox_layer_resources.Name = "groupBox_layer_resources";
            this.groupBox_layer_resources.Size = new System.Drawing.Size(171, 56);
            this.groupBox_layer_resources.TabIndex = 17;
            this.groupBox_layer_resources.TabStop = false;
            this.groupBox_layer_resources.Text = "Resources";
            // 
            // button_visibility_resources
            // 
            this.button_visibility_resources.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_visibility_resources.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_visibility_resources.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_resources.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_visibility_resources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_visibility_resources.ImageIndex = 0;
            this.button_visibility_resources.ImageList = this.imageList1;
            this.button_visibility_resources.Location = new System.Drawing.Point(134, 24);
            this.button_visibility_resources.Name = "button_visibility_resources";
            this.button_visibility_resources.Size = new System.Drawing.Size(31, 26);
            this.button_visibility_resources.TabIndex = 2;
            this.button_visibility_resources.UseVisualStyleBackColor = true;
            this.button_visibility_resources.Click += new System.EventHandler(this.button_visibility_resources_Click);
            // 
            // panel_palette_resources
            // 
            this.panel_palette_resources.Location = new System.Drawing.Point(6, 26);
            this.panel_palette_resources.Name = "panel_palette_resources";
            this.panel_palette_resources.Size = new System.Drawing.Size(90, 25);
            this.panel_palette_resources.TabIndex = 0;
            this.panel_palette_resources.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_resources_click);
            // 
            // Monsters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 685);
            this.Controls.Add(this.groupBox_layer_resources);
            this.Controls.Add(this.groupBox_layer_monsters);
            this.Controls.Add(this.groupBox_layer_itens);
            this.Controls.Add(this.groupBox_layer_difficulty);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.groupBox_selection);
            this.Controls.Add(this.button_previous);
            this.Controls.Add(this.button_undo);
            this.Controls.Add(this.button_next);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.textBox_logger);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_characterlevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxmonsters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_endpoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mapobjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_hordes)).EndInit();
            this.groupBox_selection.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox_layer_difficulty.ResumeLayout(false);
            this.groupBox_layer_itens.ResumeLayout(false);
            this.groupBox_layer_monsters.ResumeLayout(false);
            this.groupBox_layer_resources.ResumeLayout(false);
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
        private System.Windows.Forms.NumericUpDown numericUpDown_characterlevel;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxmonsters;
        private System.Windows.Forms.TrackBar trackBar_endpoints;
        private System.Windows.Forms.TrackBar trackBar_mapobjects;
        private System.Windows.Forms.TrackBar trackBar_hordes;
        private System.Windows.Forms.Label label_endpoints;
        private System.Windows.Forms.Label label_mapobjects;
        private System.Windows.Forms.Label label_hordes;
        private System.Windows.Forms.Label label_expectedlevel;
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
        private System.Windows.Forms.GroupBox groupBox_layer_difficulty;
        private System.Windows.Forms.Panel panel_palett_difficulty;
        private System.Windows.Forms.Button button_visibility_difficulty;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox_layer_itens;
        private System.Windows.Forms.Button button_visibility_itens;
        private System.Windows.Forms.Panel panel_palette_itens;
        private System.Windows.Forms.GroupBox groupBox_layer_monsters;
        private System.Windows.Forms.Button button_visibility_monsters;
        private System.Windows.Forms.Panel panel_palette_monsters;
        private System.Windows.Forms.GroupBox groupBox_layer_resources;
        private System.Windows.Forms.Button button_visibility_resources;
        private System.Windows.Forms.Panel panel_palette_resources;
        private System.Windows.Forms.ToolTip toolTip_panel;
    }
}