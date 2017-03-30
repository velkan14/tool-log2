namespace Log2CyclePrototype
{
    partial class Settings
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
            this.gaGroupBox = new System.Windows.Forms.GroupBox();
            this.generalSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.CheckBox_keepPopulation = new System.Windows.Forms.CheckBox();
            this.Button_ResetAlgorithm = new System.Windows.Forms.Button();
            this.trianglePanel = new System.Windows.Forms.Panel();
            this.crossOverTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.RadioButton_doublePoint = new System.Windows.Forms.RadioButton();
            this.RadioButton_fourByFour = new System.Windows.Forms.RadioButton();
            this.RadioButton_singlePoint = new System.Windows.Forms.RadioButton();
            this.RadioButton_threeByThree = new System.Windows.Forms.RadioButton();
            this.RadioButton_twoByTwo = new System.Windows.Forms.RadioButton();
            this.CheckBox_RandomPopCarryOver = new System.Windows.Forms.CheckBox();
            this.HooksGroup = new System.Windows.Forms.GroupBox();
            this.Button_runAlgorithm = new System.Windows.Forms.Button();
            this.hooksToggleLabel = new System.Windows.Forms.Label();
            this.Button_hooksToggle = new System.Windows.Forms.Button();
            this.NoveltyGroupBox = new System.Windows.Forms.GroupBox();
            this.label_elitism2 = new System.Windows.Forms.Label();
            this.label_initialPopulation2 = new System.Windows.Forms.Label();
            this.NumericUpDown_Innovation_elitismPercentage = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDown_Innovation_generations = new System.Windows.Forms.NumericUpDown();
            this.label_mutationPecentage2 = new System.Windows.Forms.Label();
            this.NumericUpDown_Innovation_mutationPercentage = new System.Windows.Forms.NumericUpDown();
            this.label_generations2 = new System.Windows.Forms.Label();
            this.NumericUpDown_Innovation_initialPopulation = new System.Windows.Forms.NumericUpDown();
            this.ObjectiveGroupBox = new System.Windows.Forms.GroupBox();
            this.label_elitismPercentage = new System.Windows.Forms.Label();
            this.NumericUpDown_Objective_elitismPercentage = new System.Windows.Forms.NumericUpDown();
            this.label_mutationPercentage = new System.Windows.Forms.Label();
            this.NumericUpDown_Objective_mutationPercentage = new System.Windows.Forms.NumericUpDown();
            this.label_initialPopulation = new System.Windows.Forms.Label();
            this.NumericUpDown_Objective_InitialPopulation = new System.Windows.Forms.NumericUpDown();
            this.label_generations = new System.Windows.Forms.Label();
            this.NumericUpDown_Objective_generations = new System.Windows.Forms.NumericUpDown();
            this.gaGroupBox.SuspendLayout();
            this.generalSettingsGroupBox.SuspendLayout();
            this.crossOverTypeGroupBox.SuspendLayout();
            this.HooksGroup.SuspendLayout();
            this.NoveltyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_elitismPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_generations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_mutationPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_initialPopulation)).BeginInit();
            this.ObjectiveGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_elitismPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_mutationPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_InitialPopulation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_generations)).BeginInit();
            this.SuspendLayout();
            // 
            // gaGroupBox
            // 
            this.gaGroupBox.Controls.Add(this.generalSettingsGroupBox);
            this.gaGroupBox.Controls.Add(this.NoveltyGroupBox);
            this.gaGroupBox.Controls.Add(this.ObjectiveGroupBox);
            this.gaGroupBox.Location = new System.Drawing.Point(12, 12);
            this.gaGroupBox.Name = "gaGroupBox";
            this.gaGroupBox.Size = new System.Drawing.Size(916, 163);
            this.gaGroupBox.TabIndex = 32;
            this.gaGroupBox.TabStop = false;
            this.gaGroupBox.Text = "GA Settings";
            // 
            // generalSettingsGroupBox
            // 
            this.generalSettingsGroupBox.Controls.Add(this.CheckBox_keepPopulation);
            this.generalSettingsGroupBox.Controls.Add(this.Button_ResetAlgorithm);
            this.generalSettingsGroupBox.Controls.Add(this.trianglePanel);
            this.generalSettingsGroupBox.Controls.Add(this.crossOverTypeGroupBox);
            this.generalSettingsGroupBox.Controls.Add(this.CheckBox_RandomPopCarryOver);
            this.generalSettingsGroupBox.Controls.Add(this.HooksGroup);
            this.generalSettingsGroupBox.Location = new System.Drawing.Point(380, 22);
            this.generalSettingsGroupBox.Name = "generalSettingsGroupBox";
            this.generalSettingsGroupBox.Size = new System.Drawing.Size(530, 135);
            this.generalSettingsGroupBox.TabIndex = 32;
            this.generalSettingsGroupBox.TabStop = false;
            this.generalSettingsGroupBox.Text = "General";
            // 
            // CheckBox_keepPopulation
            // 
            this.CheckBox_keepPopulation.AutoSize = true;
            this.CheckBox_keepPopulation.Checked = true;
            this.CheckBox_keepPopulation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_keepPopulation.Location = new System.Drawing.Point(208, 41);
            this.CheckBox_keepPopulation.Name = "CheckBox_keepPopulation";
            this.CheckBox_keepPopulation.Size = new System.Drawing.Size(104, 17);
            this.CheckBox_keepPopulation.TabIndex = 33;
            this.CheckBox_keepPopulation.Text = "Keep Population";
            this.CheckBox_keepPopulation.UseVisualStyleBackColor = true;
            this.CheckBox_keepPopulation.CheckedChanged += new System.EventHandler(this.CheckBox_keepPopulation_CheckedChanged);
            // 
            // Button_ResetAlgorithm
            // 
            this.Button_ResetAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ResetAlgorithm.Location = new System.Drawing.Point(336, 43);
            this.Button_ResetAlgorithm.Name = "Button_ResetAlgorithm";
            this.Button_ResetAlgorithm.Size = new System.Drawing.Size(65, 38);
            this.Button_ResetAlgorithm.TabIndex = 32;
            this.Button_ResetAlgorithm.Text = "Reset Algorithm";
            this.Button_ResetAlgorithm.UseVisualStyleBackColor = true;
            this.Button_ResetAlgorithm.Click += new System.EventHandler(this.Button_ResetAlgorithm_Click);
            // 
            // trianglePanel
            // 
            this.trianglePanel.Location = new System.Drawing.Point(424, 24);
            this.trianglePanel.Name = "trianglePanel";
            this.trianglePanel.Size = new System.Drawing.Size(100, 100);
            this.trianglePanel.TabIndex = 3;
            this.trianglePanel.Visible = false;
            // 
            // crossOverTypeGroupBox
            // 
            this.crossOverTypeGroupBox.Controls.Add(this.RadioButton_doublePoint);
            this.crossOverTypeGroupBox.Controls.Add(this.RadioButton_fourByFour);
            this.crossOverTypeGroupBox.Controls.Add(this.RadioButton_singlePoint);
            this.crossOverTypeGroupBox.Controls.Add(this.RadioButton_threeByThree);
            this.crossOverTypeGroupBox.Controls.Add(this.RadioButton_twoByTwo);
            this.crossOverTypeGroupBox.Location = new System.Drawing.Point(6, 21);
            this.crossOverTypeGroupBox.Name = "crossOverTypeGroupBox";
            this.crossOverTypeGroupBox.Size = new System.Drawing.Size(193, 108);
            this.crossOverTypeGroupBox.TabIndex = 31;
            this.crossOverTypeGroupBox.TabStop = false;
            this.crossOverTypeGroupBox.Text = "Crossover Type";
            // 
            // RadioButton_doublePoint
            // 
            this.RadioButton_doublePoint.AutoSize = true;
            this.RadioButton_doublePoint.Location = new System.Drawing.Point(99, 46);
            this.RadioButton_doublePoint.Name = "RadioButton_doublePoint";
            this.RadioButton_doublePoint.Size = new System.Drawing.Size(86, 17);
            this.RadioButton_doublePoint.TabIndex = 4;
            this.RadioButton_doublePoint.Tag = "dp";
            this.RadioButton_doublePoint.Text = "Double-Point";
            this.RadioButton_doublePoint.UseVisualStyleBackColor = true;
            this.RadioButton_doublePoint.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // RadioButton_fourByFour
            // 
            this.RadioButton_fourByFour.AutoSize = true;
            this.RadioButton_fourByFour.Checked = true;
            this.RadioButton_fourByFour.Location = new System.Drawing.Point(6, 73);
            this.RadioButton_fourByFour.Name = "RadioButton_fourByFour";
            this.RadioButton_fourByFour.Size = new System.Drawing.Size(79, 17);
            this.RadioButton_fourByFour.TabIndex = 3;
            this.RadioButton_fourByFour.TabStop = true;
            this.RadioButton_fourByFour.Tag = "4x4s";
            this.RadioButton_fourByFour.Text = "4x4 Square";
            this.RadioButton_fourByFour.UseVisualStyleBackColor = true;
            this.RadioButton_fourByFour.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // RadioButton_singlePoint
            // 
            this.RadioButton_singlePoint.AutoSize = true;
            this.RadioButton_singlePoint.Location = new System.Drawing.Point(99, 22);
            this.RadioButton_singlePoint.Name = "RadioButton_singlePoint";
            this.RadioButton_singlePoint.Size = new System.Drawing.Size(81, 17);
            this.RadioButton_singlePoint.TabIndex = 2;
            this.RadioButton_singlePoint.Tag = "sp";
            this.RadioButton_singlePoint.Text = "Single-Point";
            this.RadioButton_singlePoint.UseVisualStyleBackColor = true;
            this.RadioButton_singlePoint.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // RadioButton_threeByThree
            // 
            this.RadioButton_threeByThree.AutoSize = true;
            this.RadioButton_threeByThree.Location = new System.Drawing.Point(6, 48);
            this.RadioButton_threeByThree.Name = "RadioButton_threeByThree";
            this.RadioButton_threeByThree.Size = new System.Drawing.Size(79, 17);
            this.RadioButton_threeByThree.TabIndex = 1;
            this.RadioButton_threeByThree.Tag = "3x3s";
            this.RadioButton_threeByThree.Text = "3x3 Square";
            this.RadioButton_threeByThree.UseVisualStyleBackColor = true;
            this.RadioButton_threeByThree.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // RadioButton_twoByTwo
            // 
            this.RadioButton_twoByTwo.AutoSize = true;
            this.RadioButton_twoByTwo.Location = new System.Drawing.Point(6, 22);
            this.RadioButton_twoByTwo.Name = "RadioButton_twoByTwo";
            this.RadioButton_twoByTwo.Size = new System.Drawing.Size(79, 17);
            this.RadioButton_twoByTwo.TabIndex = 0;
            this.RadioButton_twoByTwo.Tag = "2x2s";
            this.RadioButton_twoByTwo.Text = "2x2 Square";
            this.RadioButton_twoByTwo.UseVisualStyleBackColor = true;
            this.RadioButton_twoByTwo.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // CheckBox_RandomPopCarryOver
            // 
            this.CheckBox_RandomPopCarryOver.AutoSize = true;
            this.CheckBox_RandomPopCarryOver.Location = new System.Drawing.Point(208, 19);
            this.CheckBox_RandomPopCarryOver.Name = "CheckBox_RandomPopCarryOver";
            this.CheckBox_RandomPopCarryOver.Size = new System.Drawing.Size(161, 17);
            this.CheckBox_RandomPopCarryOver.TabIndex = 29;
            this.CheckBox_RandomPopCarryOver.Text = "Random Population Transfer";
            this.CheckBox_RandomPopCarryOver.UseVisualStyleBackColor = true;
            this.CheckBox_RandomPopCarryOver.CheckedChanged += new System.EventHandler(this.CheckBox_RandomPopCarryOver_CheckedChanged);
            // 
            // HooksGroup
            // 
            this.HooksGroup.Controls.Add(this.Button_runAlgorithm);
            this.HooksGroup.Controls.Add(this.hooksToggleLabel);
            this.HooksGroup.Controls.Add(this.Button_hooksToggle);
            this.HooksGroup.Location = new System.Drawing.Point(207, 84);
            this.HooksGroup.Name = "HooksGroup";
            this.HooksGroup.Size = new System.Drawing.Size(211, 45);
            this.HooksGroup.TabIndex = 6;
            this.HooksGroup.TabStop = false;
            this.HooksGroup.Text = "Hooks";
            // 
            // Button_runAlgorithm
            // 
            this.Button_runAlgorithm.BackColor = System.Drawing.SystemColors.Control;
            this.Button_runAlgorithm.Enabled = false;
            this.Button_runAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_runAlgorithm.Location = new System.Drawing.Point(156, 14);
            this.Button_runAlgorithm.Name = "Button_runAlgorithm";
            this.Button_runAlgorithm.Size = new System.Drawing.Size(43, 23);
            this.Button_runAlgorithm.TabIndex = 4;
            this.Button_runAlgorithm.Text = "Run";
            this.Button_runAlgorithm.UseVisualStyleBackColor = false;
            // 
            // hooksToggleLabel
            // 
            this.hooksToggleLabel.AutoSize = true;
            this.hooksToggleLabel.Location = new System.Drawing.Point(5, 21);
            this.hooksToggleLabel.Name = "hooksToggleLabel";
            this.hooksToggleLabel.Size = new System.Drawing.Size(91, 13);
            this.hooksToggleLabel.TabIndex = 3;
            this.hooksToggleLabel.Text = "Auto-suggestions:";
            // 
            // Button_hooksToggle
            // 
            this.Button_hooksToggle.BackColor = System.Drawing.SystemColors.Control;
            this.Button_hooksToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_hooksToggle.Location = new System.Drawing.Point(101, 14);
            this.Button_hooksToggle.Name = "Button_hooksToggle";
            this.Button_hooksToggle.Size = new System.Drawing.Size(43, 23);
            this.Button_hooksToggle.TabIndex = 2;
            this.Button_hooksToggle.Text = "On";
            this.Button_hooksToggle.UseVisualStyleBackColor = false;
            // 
            // NoveltyGroupBox
            // 
            this.NoveltyGroupBox.Controls.Add(this.label_elitism2);
            this.NoveltyGroupBox.Controls.Add(this.label_initialPopulation2);
            this.NoveltyGroupBox.Controls.Add(this.NumericUpDown_Innovation_elitismPercentage);
            this.NoveltyGroupBox.Controls.Add(this.NumericUpDown_Innovation_generations);
            this.NoveltyGroupBox.Controls.Add(this.label_mutationPecentage2);
            this.NoveltyGroupBox.Controls.Add(this.NumericUpDown_Innovation_mutationPercentage);
            this.NoveltyGroupBox.Controls.Add(this.label_generations2);
            this.NoveltyGroupBox.Controls.Add(this.NumericUpDown_Innovation_initialPopulation);
            this.NoveltyGroupBox.Location = new System.Drawing.Point(194, 22);
            this.NoveltyGroupBox.Name = "NoveltyGroupBox";
            this.NoveltyGroupBox.Size = new System.Drawing.Size(180, 135);
            this.NoveltyGroupBox.TabIndex = 29;
            this.NoveltyGroupBox.TabStop = false;
            this.NoveltyGroupBox.Text = "Innovation";
            // 
            // label_elitism2
            // 
            this.label_elitism2.AutoSize = true;
            this.label_elitism2.Location = new System.Drawing.Point(108, 81);
            this.label_elitism2.Name = "label_elitism2";
            this.label_elitism2.Size = new System.Drawing.Size(47, 13);
            this.label_elitism2.TabIndex = 35;
            this.label_elitism2.Text = "Elitism %";
            // 
            // label_initialPopulation2
            // 
            this.label_initialPopulation2.AutoSize = true;
            this.label_initialPopulation2.Location = new System.Drawing.Point(6, 23);
            this.label_initialPopulation2.Name = "label_initialPopulation2";
            this.label_initialPopulation2.Size = new System.Drawing.Size(84, 13);
            this.label_initialPopulation2.TabIndex = 22;
            this.label_initialPopulation2.Text = "Initial Population";
            // 
            // NumericUpDown_Innovation_elitismPercentage
            // 
            this.NumericUpDown_Innovation_elitismPercentage.Location = new System.Drawing.Point(111, 97);
            this.NumericUpDown_Innovation_elitismPercentage.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_elitismPercentage.Name = "NumericUpDown_Innovation_elitismPercentage";
            this.NumericUpDown_Innovation_elitismPercentage.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Innovation_elitismPercentage.TabIndex = 34;
            this.NumericUpDown_Innovation_elitismPercentage.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_elitismPercentage.ValueChanged += new System.EventHandler(this.NumericUpDown_Innovation_elitismPercentage_ValueChanged);
            // 
            // NumericUpDown_Innovation_generations
            // 
            this.NumericUpDown_Innovation_generations.Location = new System.Drawing.Point(13, 97);
            this.NumericUpDown_Innovation_generations.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_generations.Name = "NumericUpDown_Innovation_generations";
            this.NumericUpDown_Innovation_generations.Size = new System.Drawing.Size(45, 20);
            this.NumericUpDown_Innovation_generations.TabIndex = 26;
            this.NumericUpDown_Innovation_generations.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_generations.ValueChanged += new System.EventHandler(this.NumericUpDown_Innovation_generations_ValueChanged);
            // 
            // label_mutationPecentage2
            // 
            this.label_mutationPecentage2.AutoSize = true;
            this.label_mutationPecentage2.Location = new System.Drawing.Point(108, 23);
            this.label_mutationPecentage2.Name = "label_mutationPecentage2";
            this.label_mutationPecentage2.Size = new System.Drawing.Size(59, 13);
            this.label_mutationPecentage2.TabIndex = 33;
            this.label_mutationPecentage2.Text = "Mutation %";
            // 
            // NumericUpDown_Innovation_mutationPercentage
            // 
            this.NumericUpDown_Innovation_mutationPercentage.Location = new System.Drawing.Point(111, 39);
            this.NumericUpDown_Innovation_mutationPercentage.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_mutationPercentage.Name = "NumericUpDown_Innovation_mutationPercentage";
            this.NumericUpDown_Innovation_mutationPercentage.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Innovation_mutationPercentage.TabIndex = 32;
            this.NumericUpDown_Innovation_mutationPercentage.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_mutationPercentage.ValueChanged += new System.EventHandler(this.NumericUpDown_Innovation_mutationPercentage_ValueChanged);
            // 
            // label_generations2
            // 
            this.label_generations2.AutoSize = true;
            this.label_generations2.Location = new System.Drawing.Point(6, 81);
            this.label_generations2.Name = "label_generations2";
            this.label_generations2.Size = new System.Drawing.Size(64, 13);
            this.label_generations2.TabIndex = 24;
            this.label_generations2.Text = "Generations";
            // 
            // NumericUpDown_Innovation_initialPopulation
            // 
            this.NumericUpDown_Innovation_initialPopulation.Location = new System.Drawing.Point(13, 39);
            this.NumericUpDown_Innovation_initialPopulation.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_initialPopulation.Name = "NumericUpDown_Innovation_initialPopulation";
            this.NumericUpDown_Innovation_initialPopulation.Size = new System.Drawing.Size(45, 20);
            this.NumericUpDown_Innovation_initialPopulation.TabIndex = 25;
            this.NumericUpDown_Innovation_initialPopulation.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.NumericUpDown_Innovation_initialPopulation.ValueChanged += new System.EventHandler(this.NumericUpDown_Innovation_initialPopulation_ValueChanged);
            // 
            // ObjectiveGroupBox
            // 
            this.ObjectiveGroupBox.Controls.Add(this.label_elitismPercentage);
            this.ObjectiveGroupBox.Controls.Add(this.NumericUpDown_Objective_elitismPercentage);
            this.ObjectiveGroupBox.Controls.Add(this.label_mutationPercentage);
            this.ObjectiveGroupBox.Controls.Add(this.NumericUpDown_Objective_mutationPercentage);
            this.ObjectiveGroupBox.Controls.Add(this.label_initialPopulation);
            this.ObjectiveGroupBox.Controls.Add(this.NumericUpDown_Objective_InitialPopulation);
            this.ObjectiveGroupBox.Controls.Add(this.label_generations);
            this.ObjectiveGroupBox.Controls.Add(this.NumericUpDown_Objective_generations);
            this.ObjectiveGroupBox.Location = new System.Drawing.Point(6, 22);
            this.ObjectiveGroupBox.Name = "ObjectiveGroupBox";
            this.ObjectiveGroupBox.Size = new System.Drawing.Size(182, 135);
            this.ObjectiveGroupBox.TabIndex = 21;
            this.ObjectiveGroupBox.TabStop = false;
            this.ObjectiveGroupBox.Text = "Objective";
            // 
            // label_elitismPercentage
            // 
            this.label_elitismPercentage.AutoSize = true;
            this.label_elitismPercentage.Location = new System.Drawing.Point(109, 80);
            this.label_elitismPercentage.Name = "label_elitismPercentage";
            this.label_elitismPercentage.Size = new System.Drawing.Size(47, 13);
            this.label_elitismPercentage.TabIndex = 31;
            this.label_elitismPercentage.Text = "Elitism %";
            // 
            // NumericUpDown_Objective_elitismPercentage
            // 
            this.NumericUpDown_Objective_elitismPercentage.Location = new System.Drawing.Point(112, 96);
            this.NumericUpDown_Objective_elitismPercentage.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Objective_elitismPercentage.Name = "NumericUpDown_Objective_elitismPercentage";
            this.NumericUpDown_Objective_elitismPercentage.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Objective_elitismPercentage.TabIndex = 30;
            this.NumericUpDown_Objective_elitismPercentage.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumericUpDown_Objective_elitismPercentage.ValueChanged += new System.EventHandler(this.NumericUpDown_Objective_elitismPercentage_ValueChanged);
            // 
            // label_mutationPercentage
            // 
            this.label_mutationPercentage.AutoSize = true;
            this.label_mutationPercentage.Location = new System.Drawing.Point(109, 24);
            this.label_mutationPercentage.Name = "label_mutationPercentage";
            this.label_mutationPercentage.Size = new System.Drawing.Size(59, 13);
            this.label_mutationPercentage.TabIndex = 29;
            this.label_mutationPercentage.Text = "Mutation %";
            // 
            // NumericUpDown_Objective_mutationPercentage
            // 
            this.NumericUpDown_Objective_mutationPercentage.Location = new System.Drawing.Point(112, 40);
            this.NumericUpDown_Objective_mutationPercentage.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Objective_mutationPercentage.Name = "NumericUpDown_Objective_mutationPercentage";
            this.NumericUpDown_Objective_mutationPercentage.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Objective_mutationPercentage.TabIndex = 28;
            this.NumericUpDown_Objective_mutationPercentage.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDown_Objective_mutationPercentage.ValueChanged += new System.EventHandler(this.NumericUpDown_Objective_mutationPercentage_ValueChanged);
            // 
            // label_initialPopulation
            // 
            this.label_initialPopulation.AutoSize = true;
            this.label_initialPopulation.Location = new System.Drawing.Point(9, 24);
            this.label_initialPopulation.Name = "label_initialPopulation";
            this.label_initialPopulation.Size = new System.Drawing.Size(84, 13);
            this.label_initialPopulation.TabIndex = 21;
            this.label_initialPopulation.Text = "Initial Population";
            // 
            // NumericUpDown_Objective_InitialPopulation
            // 
            this.NumericUpDown_Objective_InitialPopulation.Location = new System.Drawing.Point(12, 40);
            this.NumericUpDown_Objective_InitialPopulation.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericUpDown_Objective_InitialPopulation.Name = "NumericUpDown_Objective_InitialPopulation";
            this.NumericUpDown_Objective_InitialPopulation.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Objective_InitialPopulation.TabIndex = 19;
            this.NumericUpDown_Objective_InitialPopulation.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.NumericUpDown_Objective_InitialPopulation.ValueChanged += new System.EventHandler(this.NumericUpDown_Objective_InitialPopulation_ValueChanged);
            // 
            // label_generations
            // 
            this.label_generations.AutoSize = true;
            this.label_generations.Location = new System.Drawing.Point(6, 80);
            this.label_generations.Name = "label_generations";
            this.label_generations.Size = new System.Drawing.Size(64, 13);
            this.label_generations.TabIndex = 20;
            this.label_generations.Text = "Generations";
            // 
            // NumericUpDown_Objective_generations
            // 
            this.NumericUpDown_Objective_generations.Location = new System.Drawing.Point(12, 96);
            this.NumericUpDown_Objective_generations.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericUpDown_Objective_generations.Name = "NumericUpDown_Objective_generations";
            this.NumericUpDown_Objective_generations.Size = new System.Drawing.Size(46, 20);
            this.NumericUpDown_Objective_generations.TabIndex = 23;
            this.NumericUpDown_Objective_generations.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.NumericUpDown_Objective_generations.ValueChanged += new System.EventHandler(this.NumericUpDown_Objective_generations_ValueChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 183);
            this.Controls.Add(this.gaGroupBox);
            this.Name = "Settings";
            this.Text = "Setttings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.gaGroupBox.ResumeLayout(false);
            this.generalSettingsGroupBox.ResumeLayout(false);
            this.generalSettingsGroupBox.PerformLayout();
            this.crossOverTypeGroupBox.ResumeLayout(false);
            this.crossOverTypeGroupBox.PerformLayout();
            this.HooksGroup.ResumeLayout(false);
            this.HooksGroup.PerformLayout();
            this.NoveltyGroupBox.ResumeLayout(false);
            this.NoveltyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_elitismPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_generations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_mutationPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Innovation_initialPopulation)).EndInit();
            this.ObjectiveGroupBox.ResumeLayout(false);
            this.ObjectiveGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_elitismPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_mutationPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_InitialPopulation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Objective_generations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gaGroupBox;
        private System.Windows.Forms.GroupBox generalSettingsGroupBox;
        private System.Windows.Forms.CheckBox CheckBox_keepPopulation;
        private System.Windows.Forms.Button Button_ResetAlgorithm;
        private System.Windows.Forms.Panel trianglePanel;
        private System.Windows.Forms.GroupBox crossOverTypeGroupBox;
        private System.Windows.Forms.RadioButton RadioButton_doublePoint;
        private System.Windows.Forms.RadioButton RadioButton_fourByFour;
        private System.Windows.Forms.RadioButton RadioButton_singlePoint;
        private System.Windows.Forms.RadioButton RadioButton_threeByThree;
        private System.Windows.Forms.RadioButton RadioButton_twoByTwo;
        private System.Windows.Forms.CheckBox CheckBox_RandomPopCarryOver;
        private System.Windows.Forms.GroupBox HooksGroup;
        private System.Windows.Forms.Button Button_runAlgorithm;
        private System.Windows.Forms.Label hooksToggleLabel;
        private System.Windows.Forms.Button Button_hooksToggle;
        private System.Windows.Forms.GroupBox NoveltyGroupBox;
        private System.Windows.Forms.Label label_elitism2;
        private System.Windows.Forms.Label label_initialPopulation2;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Innovation_elitismPercentage;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Innovation_generations;
        private System.Windows.Forms.Label label_mutationPecentage2;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Innovation_mutationPercentage;
        private System.Windows.Forms.Label label_generations2;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Innovation_initialPopulation;
        private System.Windows.Forms.GroupBox ObjectiveGroupBox;
        private System.Windows.Forms.Label label_elitismPercentage;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Objective_elitismPercentage;
        private System.Windows.Forms.Label label_mutationPercentage;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Objective_mutationPercentage;
        private System.Windows.Forms.Label label_initialPopulation;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Objective_InitialPopulation;
        private System.Windows.Forms.Label label_generations;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Objective_generations;
    }
}