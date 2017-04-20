using System;
using System.Windows.Forms;
using gma.System.Windows;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Security.Permissions;
using GAF;
using System.Collections.Generic;
using Log2CyclePrototype.Utilities;
using System.Threading;
using Log2CyclePrototype.Exceptions;
using System.Reflection;
using Log2CyclePrototype.WinAPI;
using System.Drawing.Drawing2D;
using Log2CyclePrototype.LoG2API.Elements;
using Log2CyclePrototype.LoG2API;

namespace Log2CyclePrototype
{

    class MainForm : System.Windows.Forms.Form
    {

        enum CellHighlight
        {
            None,
            All,
            Added,
            Removed
        }

        private const int EXPANDED_SIZE = 870;
        private const int COLLAPSED_SIZE = 700;

        private int userSliderDefaultVal = 10;
        private int objSliderDefaultVal = 70;
        private int novSliderDefaultVal = 20;

        public static Population PreviousObjectivePopulation;
        public static Population PreviousNoveltyPopulation;
        public static bool LoG2ProcessFound = false;

        private delegate void AlgorithmRunComplete(List<Gene> s);

        private ProcessMonitor _procMon;
        private UserActivityHook _actHook;
        private ObjectiveAlgorithmTestClass _objAlgTest;
        private NoveltyAlgorithmTestClass _novAlgTest;
        private bool _algorithmsInitialized = false;
        private CrossoverType CurrentCrossoverSelection { get; set; }
        public static object fileLock = new object();

        public static EventWaitHandle noveltySyncHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public static EventWaitHandle objectiveSyncHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public static int syncCounter = 0;
        public static bool objRunning = false, novRunning = false;
        private FileSystemWatcher fsw;
        private bool autoSuggestions = true;

        private Point trianglePoint1, trianglePoint2, trianglePoint3;
        private PointF triangleDotPoint;
        private float triangleDotRadius;

        private Graphics _cellPanelGraphics, _trianglePickerGraphics;
        private Bitmap _cellBitmap, _trianglePickerBitmap;
        private SolidBrush _groundTile = new SolidBrush(Color.FromArgb(125, 125, 125));
        private Color _addedCellColorSelected = Color.FromArgb(102, 141, 255),
                      _removedCellColorSelected = Color.FromArgb(255, 102, 102),
                      _addedCellColorUnselected = Color.FromArgb(125, 102, 141, 255),
                      _removedCellColorUnselected = Color.FromArgb(125, 255, 102, 102),
                      _groundTileColorSelected = Color.FromArgb(125, 125, 125),
                      _groundTileColorUnselected = Color.FromArgb(125, 125, 125, 125),
                      _cellBorderColor = Color.FromArgb(70, 70, 70),
                      _emptyCellColor = Color.LightGray;
        private CellHighlight _cellHighlightSettings = CellHighlight.All;
        private List<Point> _userSelectedPoints, _userSelectedCellsToErase;
        private int cellWidth, cellHeight;
        private List<Cell> _cellsToDraw;
        private Map currentMap, previousMap, solutionChromosomeMap;
        private List<Map> solutionHistory;
        private int _currentSolutionIndex = -1;
        private int _hitmeCount = 2;
        private float _userSketchInfluencePercent, _objectiveInfluencePercent, _noveltyInfluencePercent;
        private Panel gridPanel;
        private GroupBox HooksGroup;
        private System.ComponentModel.IContainer components;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private ContextMenuStrip gridPanelContextMenu;
        private ToolStripMenuItem gridPanelContextMenuExport;
        private ToolStripMenuItem gridPanelContextMenuClear;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem gASettingsToolStripMenuItem;
        private ToolStripMenuItem standardModeToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem creditsToolStripMenuItem;
        private ToolStripMenuItem controlsToolStripMenuItem;
        private ToolStripMenuItem selectProjectDirToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Button ApplyButton;
        private NumericUpDown ObjGenInput;
        private Label ObjGenLabel;
        private NumericUpDown ObjPopInput;
        private Label ObjPopLable;
        private CheckBox RandomPopCarryOverCheckBox;
        private GroupBox ObjectiveGroupBox;
        private NumericUpDown NovPopInput;
        private Label NovGenLabel;
        private NumericUpDown NovGenInput;
        private Label NovPopLabel;
        private GroupBox NoveltyGroupBox;
        private GroupBox gaGroupBox;
        private Button RevertButton;
        private Panel advancedSettingsPanel;
        private Button advancedSettingsButton;
        private Button PreviousSolutionButton;
        private Button NextSolutionButton;
        private Button HitmeButton;
        private NumericUpDown ObjMutInput;
        private Label ObjMutLabel;
        private NumericUpDown ObjEliInput;
        private Label ObjEliLabel;
        private NumericUpDown NovMutInput;
        private Label NovMutLabel;
        private NumericUpDown NovEliInput;
        private Label NovEliLabel;
        private Label hooksToggleLabel;
        private Button hooksToggleButton;
        private Button runAlgorithmButton;
        private GroupBox generalSettingsGroupBox;
        private GroupBox crossOverTypeGroupBox;
        private RadioButton doublePointRadioButton;
        private RadioButton fourByFourRadioButton;
        private RadioButton singlePointRadioButton;
        private RadioButton threeByThreeRadioButton;
        private RadioButton twoByTwoRadioButton;
        private ToolTip TooltipControl;
        private Button ResetAlgorithmButton;
        private ToolStripMenuItem lockSelectionToolStripMenuItem;
        private ToolStripMenuItem invertSelectionToolStripMenuItem;
        private ToolStripMenuItem unlockSelectionToolStripMenuItem;
        private Panel trianglePanel;
        private CheckBox keepPopCheckBox;
        private Button InvertSelectionButton;
        private Button ClearSelectionButton;
        private Button SugestionVisibilityButton;
        private CheckBox highlightRemovedCheckbox;
        private CheckBox highlightAddedCheckbox;
        private GroupBox groupBox1;
        private ToolTip sliderTooltip;
        private Label label1;
        private Label label2;
        private Label label3;
        private Winamp.Components.WinampTrackBar userSketchSlider;
        private Winamp.Components.WinampTrackBar objectiveSlider;
        private Winamp.Components.WinampTrackBar noveltySlider;
        private Label sliderGroupLabel;
        private Button novInfoButton;
        private Button objInfoButton;
        private Button userInfoButton;
        private ComboBox objectiveCombobox;
        private Label comboLabel;
        private JCS.ToggleSwitch noveltyToggleSwitch;
        private JCS.ToggleSwitch objectiveToggleSwitch;
        private JCS.ToggleSwitch userToggleSwitch;
        private GroupBox sliderGroupBox;
        private ToolStripMenuItem expertModeToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox;
        //private System.Windows.Forms.UserControl cell;

        public MainForm()
        {
            InitializeComponent();
            this.Height = COLLAPSED_SIZE;
        }

        // THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
        // DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox = new System.Windows.Forms.TextBox();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.gridPanelContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lockSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlockSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridPanelContextMenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.gridPanelContextMenuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.HooksGroup = new System.Windows.Forms.GroupBox();
            this.runAlgorithmButton = new System.Windows.Forms.Button();
            this.hooksToggleLabel = new System.Windows.Forms.Label();
            this.hooksToggleButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.trianglePanel = new System.Windows.Forms.Panel();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectProjectDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gASettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expertModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gaGroupBox = new System.Windows.Forms.GroupBox();
            this.generalSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.keepPopCheckBox = new System.Windows.Forms.CheckBox();
            this.ResetAlgorithmButton = new System.Windows.Forms.Button();
            this.crossOverTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.doublePointRadioButton = new System.Windows.Forms.RadioButton();
            this.fourByFourRadioButton = new System.Windows.Forms.RadioButton();
            this.singlePointRadioButton = new System.Windows.Forms.RadioButton();
            this.threeByThreeRadioButton = new System.Windows.Forms.RadioButton();
            this.twoByTwoRadioButton = new System.Windows.Forms.RadioButton();
            this.RandomPopCarryOverCheckBox = new System.Windows.Forms.CheckBox();
            this.NoveltyGroupBox = new System.Windows.Forms.GroupBox();
            this.NovEliLabel = new System.Windows.Forms.Label();
            this.NovPopLabel = new System.Windows.Forms.Label();
            this.NovEliInput = new System.Windows.Forms.NumericUpDown();
            this.NovGenInput = new System.Windows.Forms.NumericUpDown();
            this.NovMutLabel = new System.Windows.Forms.Label();
            this.NovMutInput = new System.Windows.Forms.NumericUpDown();
            this.NovGenLabel = new System.Windows.Forms.Label();
            this.NovPopInput = new System.Windows.Forms.NumericUpDown();
            this.ObjectiveGroupBox = new System.Windows.Forms.GroupBox();
            this.ObjEliLabel = new System.Windows.Forms.Label();
            this.ObjEliInput = new System.Windows.Forms.NumericUpDown();
            this.ObjMutLabel = new System.Windows.Forms.Label();
            this.ObjMutInput = new System.Windows.Forms.NumericUpDown();
            this.ObjPopLable = new System.Windows.Forms.Label();
            this.ObjPopInput = new System.Windows.Forms.NumericUpDown();
            this.ObjGenLabel = new System.Windows.Forms.Label();
            this.ObjGenInput = new System.Windows.Forms.NumericUpDown();
            this.advancedSettingsPanel = new System.Windows.Forms.Panel();
            this.TooltipControl = new System.Windows.Forms.ToolTip(this.components);
            this.SugestionVisibilityButton = new System.Windows.Forms.Button();
            this.ClearSelectionButton = new System.Windows.Forms.Button();
            this.InvertSelectionButton = new System.Windows.Forms.Button();
            this.HitmeButton = new System.Windows.Forms.Button();
            this.NextSolutionButton = new System.Windows.Forms.Button();
            this.PreviousSolutionButton = new System.Windows.Forms.Button();
            this.advancedSettingsButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.RevertButton = new System.Windows.Forms.Button();
            this.highlightRemovedCheckbox = new System.Windows.Forms.CheckBox();
            this.highlightAddedCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sliderTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.userInfoButton = new System.Windows.Forms.Button();
            this.objInfoButton = new System.Windows.Forms.Button();
            this.novInfoButton = new System.Windows.Forms.Button();
            this.sliderGroupBox = new System.Windows.Forms.GroupBox();
            this.userToggleSwitch = new JCS.ToggleSwitch();
            this.objectiveToggleSwitch = new JCS.ToggleSwitch();
            this.noveltyToggleSwitch = new JCS.ToggleSwitch();
            this.comboLabel = new System.Windows.Forms.Label();
            this.objectiveCombobox = new System.Windows.Forms.ComboBox();
            this.sliderGroupLabel = new System.Windows.Forms.Label();
            this.noveltySlider = new Winamp.Components.WinampTrackBar();
            this.objectiveSlider = new Winamp.Components.WinampTrackBar();
            this.userSketchSlider = new Winamp.Components.WinampTrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gridPanelContextMenu.SuspendLayout();
            this.HooksGroup.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.gaGroupBox.SuspendLayout();
            this.generalSettingsGroupBox.SuspendLayout();
            this.crossOverTypeGroupBox.SuspendLayout();
            this.NoveltyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NovEliInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovGenInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovMutInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovPopInput)).BeginInit();
            this.ObjectiveGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjEliInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjMutInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjPopInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjGenInput)).BeginInit();
            this.advancedSettingsPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.sliderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.textBox.Location = new System.Drawing.Point(18, 442);
            this.textBox.MaximumSize = new System.Drawing.Size(560, 560);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(371, 120);
            this.textBox.TabIndex = 3;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // gridPanel
            // 
            this.gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPanel.BackColor = System.Drawing.Color.Silver;
            this.gridPanel.Location = new System.Drawing.Point(404, 49);
            this.gridPanel.MaximumSize = new System.Drawing.Size(513, 513);
            this.gridPanel.MinimumSize = new System.Drawing.Size(513, 513);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Size = new System.Drawing.Size(513, 513);
            this.gridPanel.TabIndex = 4;
            this.gridPanel.Click += new System.EventHandler(this.gridPanel_Click);
            this.gridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.gridPanel_Paint);
            this.gridPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridPanel_MouseDown);
            this.gridPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridPanel_MouseMove);
            this.gridPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridPanel_MouseUp);
            // 
            // gridPanelContextMenu
            // 
            this.gridPanelContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lockSelectionToolStripMenuItem,
            this.unlockSelectionToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.gridPanelContextMenuExport,
            this.gridPanelContextMenuClear});
            this.gridPanelContextMenu.Name = "gridPanelContextMenu";
            this.gridPanelContextMenu.Size = new System.Drawing.Size(162, 114);
            // 
            // lockSelectionToolStripMenuItem
            // 
            this.lockSelectionToolStripMenuItem.Enabled = false;
            this.lockSelectionToolStripMenuItem.Name = "lockSelectionToolStripMenuItem";
            this.lockSelectionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.lockSelectionToolStripMenuItem.Text = "Lock selection";
            this.lockSelectionToolStripMenuItem.Visible = false;
            this.lockSelectionToolStripMenuItem.Click += new System.EventHandler(this.lockSelectionToolStripMenuItem_Click);
            // 
            // unlockSelectionToolStripMenuItem
            // 
            this.unlockSelectionToolStripMenuItem.Enabled = false;
            this.unlockSelectionToolStripMenuItem.Name = "unlockSelectionToolStripMenuItem";
            this.unlockSelectionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.unlockSelectionToolStripMenuItem.Text = "Unlock selection";
            this.unlockSelectionToolStripMenuItem.Visible = false;
            this.unlockSelectionToolStripMenuItem.Click += new System.EventHandler(this.unlockSelectionToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Enabled = false;
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // gridPanelContextMenuExport
            // 
            this.gridPanelContextMenuExport.Enabled = false;
            this.gridPanelContextMenuExport.Name = "gridPanelContextMenuExport";
            this.gridPanelContextMenuExport.Size = new System.Drawing.Size(161, 22);
            this.gridPanelContextMenuExport.Text = "Export selection";
            this.gridPanelContextMenuExport.Click += new System.EventHandler(this.gridPanelContextMenuExport_Click);
            // 
            // gridPanelContextMenuClear
            // 
            this.gridPanelContextMenuClear.Enabled = false;
            this.gridPanelContextMenuClear.Name = "gridPanelContextMenuClear";
            this.gridPanelContextMenuClear.Size = new System.Drawing.Size(161, 22);
            this.gridPanelContextMenuClear.Text = "Clear selection";
            this.gridPanelContextMenuClear.Click += new System.EventHandler(this.gridPanelContextMenuClear_Click);
            // 
            // HooksGroup
            // 
            this.HooksGroup.Controls.Add(this.runAlgorithmButton);
            this.HooksGroup.Controls.Add(this.hooksToggleLabel);
            this.HooksGroup.Controls.Add(this.hooksToggleButton);
            this.HooksGroup.Location = new System.Drawing.Point(207, 84);
            this.HooksGroup.Name = "HooksGroup";
            this.HooksGroup.Size = new System.Drawing.Size(211, 45);
            this.HooksGroup.TabIndex = 6;
            this.HooksGroup.TabStop = false;
            this.HooksGroup.Text = "Hooks";
            // 
            // runAlgorithmButton
            // 
            this.runAlgorithmButton.BackColor = System.Drawing.SystemColors.Control;
            this.runAlgorithmButton.Enabled = false;
            this.runAlgorithmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runAlgorithmButton.Location = new System.Drawing.Point(156, 14);
            this.runAlgorithmButton.Name = "runAlgorithmButton";
            this.runAlgorithmButton.Size = new System.Drawing.Size(43, 23);
            this.runAlgorithmButton.TabIndex = 4;
            this.runAlgorithmButton.Text = "Run";
            this.runAlgorithmButton.UseVisualStyleBackColor = false;
            this.runAlgorithmButton.Click += new System.EventHandler(this.runAlgorithmButton_Click);
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
            // hooksToggleButton
            // 
            this.hooksToggleButton.BackColor = System.Drawing.SystemColors.Control;
            this.hooksToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hooksToggleButton.Location = new System.Drawing.Point(101, 14);
            this.hooksToggleButton.Name = "hooksToggleButton";
            this.hooksToggleButton.Size = new System.Drawing.Size(43, 23);
            this.hooksToggleButton.TabIndex = 2;
            this.hooksToggleButton.Text = "On";
            this.hooksToggleButton.UseVisualStyleBackColor = false;
            this.hooksToggleButton.Click += new System.EventHandler(this.hooksToggleButton_Click);
            // 
            // trianglePanel
            // 
            this.trianglePanel.Location = new System.Drawing.Point(424, 24);
            this.trianglePanel.Name = "trianglePanel";
            this.trianglePanel.Size = new System.Drawing.Size(100, 100);
            this.trianglePanel.TabIndex = 3;
            this.trianglePanel.Visible = false;
            this.trianglePanel.Click += new System.EventHandler(this.trianglePanel_Click);
            this.trianglePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trianglePanel_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.gASettingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(940, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProjectDirToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // selectProjectDirToolStripMenuItem
            // 
            this.selectProjectDirToolStripMenuItem.Name = "selectProjectDirToolStripMenuItem";
            this.selectProjectDirToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.selectProjectDirToolStripMenuItem.Text = "Select Project Dir";
            this.selectProjectDirToolStripMenuItem.Click += new System.EventHandler(this.selectProjectDirToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gASettingsToolStripMenuItem
            // 
            this.gASettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardModeToolStripMenuItem,
            this.expertModeToolStripMenuItem});
            this.gASettingsToolStripMenuItem.Name = "gASettingsToolStripMenuItem";
            this.gASettingsToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.gASettingsToolStripMenuItem.Text = "UI Settings";
            // 
            // standardModeToolStripMenuItem
            // 
            this.standardModeToolStripMenuItem.Checked = true;
            this.standardModeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.standardModeToolStripMenuItem.Name = "standardModeToolStripMenuItem";
            this.standardModeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.standardModeToolStripMenuItem.Tag = "standard";
            this.standardModeToolStripMenuItem.Text = "Standard Mode";
            this.standardModeToolStripMenuItem.Click += new System.EventHandler(this.inputPickerCheckBox_CheckedChanged);
            // 
            // expertModeToolStripMenuItem
            // 
            this.expertModeToolStripMenuItem.Name = "expertModeToolStripMenuItem";
            this.expertModeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.expertModeToolStripMenuItem.Tag = "expert";
            this.expertModeToolStripMenuItem.Text = "Expert Mode";
            this.expertModeToolStripMenuItem.Click += new System.EventHandler(this.inputPickerCheckBox_CheckedChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlsToolStripMenuItem,
            this.creditsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.controlsToolStripMenuItem.Text = "Controls";
            this.controlsToolStripMenuItem.Click += new System.EventHandler(this.controlsToolStripMenuItem_Click);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // gaGroupBox
            // 
            this.gaGroupBox.Controls.Add(this.generalSettingsGroupBox);
            this.gaGroupBox.Controls.Add(this.NoveltyGroupBox);
            this.gaGroupBox.Controls.Add(this.ObjectiveGroupBox);
            this.gaGroupBox.Location = new System.Drawing.Point(12, 13);
            this.gaGroupBox.Name = "gaGroupBox";
            this.gaGroupBox.Size = new System.Drawing.Size(916, 163);
            this.gaGroupBox.TabIndex = 31;
            this.gaGroupBox.TabStop = false;
            this.gaGroupBox.Text = "GA Settings";
            // 
            // generalSettingsGroupBox
            // 
            this.generalSettingsGroupBox.Controls.Add(this.keepPopCheckBox);
            this.generalSettingsGroupBox.Controls.Add(this.ResetAlgorithmButton);
            this.generalSettingsGroupBox.Controls.Add(this.trianglePanel);
            this.generalSettingsGroupBox.Controls.Add(this.crossOverTypeGroupBox);
            this.generalSettingsGroupBox.Controls.Add(this.RandomPopCarryOverCheckBox);
            this.generalSettingsGroupBox.Controls.Add(this.HooksGroup);
            this.generalSettingsGroupBox.Location = new System.Drawing.Point(380, 22);
            this.generalSettingsGroupBox.Name = "generalSettingsGroupBox";
            this.generalSettingsGroupBox.Size = new System.Drawing.Size(530, 135);
            this.generalSettingsGroupBox.TabIndex = 32;
            this.generalSettingsGroupBox.TabStop = false;
            this.generalSettingsGroupBox.Text = "General";
            // 
            // keepPopCheckBox
            // 
            this.keepPopCheckBox.AutoSize = true;
            this.keepPopCheckBox.Checked = true;
            this.keepPopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepPopCheckBox.Location = new System.Drawing.Point(208, 41);
            this.keepPopCheckBox.Name = "keepPopCheckBox";
            this.keepPopCheckBox.Size = new System.Drawing.Size(104, 17);
            this.keepPopCheckBox.TabIndex = 33;
            this.keepPopCheckBox.Text = "Keep Population";
            this.keepPopCheckBox.UseVisualStyleBackColor = true;
            // 
            // ResetAlgorithmButton
            // 
            this.ResetAlgorithmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetAlgorithmButton.Location = new System.Drawing.Point(336, 43);
            this.ResetAlgorithmButton.Name = "ResetAlgorithmButton";
            this.ResetAlgorithmButton.Size = new System.Drawing.Size(65, 38);
            this.ResetAlgorithmButton.TabIndex = 32;
            this.ResetAlgorithmButton.Text = "Reset Algorithm";
            this.ResetAlgorithmButton.UseVisualStyleBackColor = true;
            this.ResetAlgorithmButton.Click += new System.EventHandler(this.resetAlgorithmCallback);
            // 
            // crossOverTypeGroupBox
            // 
            this.crossOverTypeGroupBox.Controls.Add(this.doublePointRadioButton);
            this.crossOverTypeGroupBox.Controls.Add(this.fourByFourRadioButton);
            this.crossOverTypeGroupBox.Controls.Add(this.singlePointRadioButton);
            this.crossOverTypeGroupBox.Controls.Add(this.threeByThreeRadioButton);
            this.crossOverTypeGroupBox.Controls.Add(this.twoByTwoRadioButton);
            this.crossOverTypeGroupBox.Location = new System.Drawing.Point(6, 21);
            this.crossOverTypeGroupBox.Name = "crossOverTypeGroupBox";
            this.crossOverTypeGroupBox.Size = new System.Drawing.Size(193, 108);
            this.crossOverTypeGroupBox.TabIndex = 31;
            this.crossOverTypeGroupBox.TabStop = false;
            this.crossOverTypeGroupBox.Text = "Crossover Type";
            // 
            // doublePointRadioButton
            // 
            this.doublePointRadioButton.AutoSize = true;
            this.doublePointRadioButton.Location = new System.Drawing.Point(99, 46);
            this.doublePointRadioButton.Name = "doublePointRadioButton";
            this.doublePointRadioButton.Size = new System.Drawing.Size(86, 17);
            this.doublePointRadioButton.TabIndex = 4;
            this.doublePointRadioButton.Tag = "dp";
            this.doublePointRadioButton.Text = "Double-Point";
            this.doublePointRadioButton.UseVisualStyleBackColor = true;
            this.doublePointRadioButton.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // fourByFourRadioButton
            // 
            this.fourByFourRadioButton.AutoSize = true;
            this.fourByFourRadioButton.Checked = true;
            this.fourByFourRadioButton.Location = new System.Drawing.Point(6, 73);
            this.fourByFourRadioButton.Name = "fourByFourRadioButton";
            this.fourByFourRadioButton.Size = new System.Drawing.Size(79, 17);
            this.fourByFourRadioButton.TabIndex = 3;
            this.fourByFourRadioButton.TabStop = true;
            this.fourByFourRadioButton.Tag = "4x4s";
            this.fourByFourRadioButton.Text = "4x4 Square";
            this.fourByFourRadioButton.UseVisualStyleBackColor = true;
            this.fourByFourRadioButton.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // singlePointRadioButton
            // 
            this.singlePointRadioButton.AutoSize = true;
            this.singlePointRadioButton.Location = new System.Drawing.Point(99, 22);
            this.singlePointRadioButton.Name = "singlePointRadioButton";
            this.singlePointRadioButton.Size = new System.Drawing.Size(81, 17);
            this.singlePointRadioButton.TabIndex = 2;
            this.singlePointRadioButton.Tag = "sp";
            this.singlePointRadioButton.Text = "Single-Point";
            this.singlePointRadioButton.UseVisualStyleBackColor = true;
            this.singlePointRadioButton.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // threeByThreeRadioButton
            // 
            this.threeByThreeRadioButton.AutoSize = true;
            this.threeByThreeRadioButton.Location = new System.Drawing.Point(6, 48);
            this.threeByThreeRadioButton.Name = "threeByThreeRadioButton";
            this.threeByThreeRadioButton.Size = new System.Drawing.Size(79, 17);
            this.threeByThreeRadioButton.TabIndex = 1;
            this.threeByThreeRadioButton.Tag = "3x3s";
            this.threeByThreeRadioButton.Text = "3x3 Square";
            this.threeByThreeRadioButton.UseVisualStyleBackColor = true;
            this.threeByThreeRadioButton.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // twoByTwoRadioButton
            // 
            this.twoByTwoRadioButton.AutoSize = true;
            this.twoByTwoRadioButton.Location = new System.Drawing.Point(6, 22);
            this.twoByTwoRadioButton.Name = "twoByTwoRadioButton";
            this.twoByTwoRadioButton.Size = new System.Drawing.Size(79, 17);
            this.twoByTwoRadioButton.TabIndex = 0;
            this.twoByTwoRadioButton.Tag = "2x2s";
            this.twoByTwoRadioButton.Text = "2x2 Square";
            this.twoByTwoRadioButton.UseVisualStyleBackColor = true;
            this.twoByTwoRadioButton.CheckedChanged += new System.EventHandler(this.CrossOverChangeCallback);
            // 
            // RandomPopCarryOverCheckBox
            // 
            this.RandomPopCarryOverCheckBox.AutoSize = true;
            this.RandomPopCarryOverCheckBox.Location = new System.Drawing.Point(208, 19);
            this.RandomPopCarryOverCheckBox.Name = "RandomPopCarryOverCheckBox";
            this.RandomPopCarryOverCheckBox.Size = new System.Drawing.Size(161, 17);
            this.RandomPopCarryOverCheckBox.TabIndex = 29;
            this.RandomPopCarryOverCheckBox.Text = "Random Population Transfer";
            this.RandomPopCarryOverCheckBox.UseVisualStyleBackColor = true;
            // 
            // NoveltyGroupBox
            // 
            this.NoveltyGroupBox.Controls.Add(this.NovEliLabel);
            this.NoveltyGroupBox.Controls.Add(this.NovPopLabel);
            this.NoveltyGroupBox.Controls.Add(this.NovEliInput);
            this.NoveltyGroupBox.Controls.Add(this.NovGenInput);
            this.NoveltyGroupBox.Controls.Add(this.NovMutLabel);
            this.NoveltyGroupBox.Controls.Add(this.NovMutInput);
            this.NoveltyGroupBox.Controls.Add(this.NovGenLabel);
            this.NoveltyGroupBox.Controls.Add(this.NovPopInput);
            this.NoveltyGroupBox.Location = new System.Drawing.Point(194, 22);
            this.NoveltyGroupBox.Name = "NoveltyGroupBox";
            this.NoveltyGroupBox.Size = new System.Drawing.Size(180, 135);
            this.NoveltyGroupBox.TabIndex = 29;
            this.NoveltyGroupBox.TabStop = false;
            this.NoveltyGroupBox.Text = "Novelty";
            // 
            // NovEliLabel
            // 
            this.NovEliLabel.AutoSize = true;
            this.NovEliLabel.Location = new System.Drawing.Point(108, 81);
            this.NovEliLabel.Name = "NovEliLabel";
            this.NovEliLabel.Size = new System.Drawing.Size(47, 13);
            this.NovEliLabel.TabIndex = 35;
            this.NovEliLabel.Text = "Elitism %";
            // 
            // NovPopLabel
            // 
            this.NovPopLabel.AutoSize = true;
            this.NovPopLabel.Location = new System.Drawing.Point(6, 23);
            this.NovPopLabel.Name = "NovPopLabel";
            this.NovPopLabel.Size = new System.Drawing.Size(84, 13);
            this.NovPopLabel.TabIndex = 22;
            this.NovPopLabel.Text = "Initial Population";
            // 
            // NovEliInput
            // 
            this.NovEliInput.Location = new System.Drawing.Point(111, 97);
            this.NovEliInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NovEliInput.Name = "NovEliInput";
            this.NovEliInput.Size = new System.Drawing.Size(46, 20);
            this.NovEliInput.TabIndex = 34;
            this.NovEliInput.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // NovGenInput
            // 
            this.NovGenInput.Location = new System.Drawing.Point(13, 97);
            this.NovGenInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NovGenInput.Name = "NovGenInput";
            this.NovGenInput.Size = new System.Drawing.Size(45, 20);
            this.NovGenInput.TabIndex = 26;
            this.NovGenInput.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // NovMutLabel
            // 
            this.NovMutLabel.AutoSize = true;
            this.NovMutLabel.Location = new System.Drawing.Point(108, 23);
            this.NovMutLabel.Name = "NovMutLabel";
            this.NovMutLabel.Size = new System.Drawing.Size(59, 13);
            this.NovMutLabel.TabIndex = 33;
            this.NovMutLabel.Text = "Mutation %";
            // 
            // NovMutInput
            // 
            this.NovMutInput.Location = new System.Drawing.Point(111, 39);
            this.NovMutInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NovMutInput.Name = "NovMutInput";
            this.NovMutInput.Size = new System.Drawing.Size(46, 20);
            this.NovMutInput.TabIndex = 32;
            this.NovMutInput.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // NovGenLabel
            // 
            this.NovGenLabel.AutoSize = true;
            this.NovGenLabel.Location = new System.Drawing.Point(6, 81);
            this.NovGenLabel.Name = "NovGenLabel";
            this.NovGenLabel.Size = new System.Drawing.Size(64, 13);
            this.NovGenLabel.TabIndex = 24;
            this.NovGenLabel.Text = "Generations";
            // 
            // NovPopInput
            // 
            this.NovPopInput.Location = new System.Drawing.Point(13, 39);
            this.NovPopInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NovPopInput.Name = "NovPopInput";
            this.NovPopInput.Size = new System.Drawing.Size(45, 20);
            this.NovPopInput.TabIndex = 25;
            this.NovPopInput.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // ObjectiveGroupBox
            // 
            this.ObjectiveGroupBox.Controls.Add(this.ObjEliLabel);
            this.ObjectiveGroupBox.Controls.Add(this.ObjEliInput);
            this.ObjectiveGroupBox.Controls.Add(this.ObjMutLabel);
            this.ObjectiveGroupBox.Controls.Add(this.ObjMutInput);
            this.ObjectiveGroupBox.Controls.Add(this.ObjPopLable);
            this.ObjectiveGroupBox.Controls.Add(this.ObjPopInput);
            this.ObjectiveGroupBox.Controls.Add(this.ObjGenLabel);
            this.ObjectiveGroupBox.Controls.Add(this.ObjGenInput);
            this.ObjectiveGroupBox.Location = new System.Drawing.Point(6, 22);
            this.ObjectiveGroupBox.Name = "ObjectiveGroupBox";
            this.ObjectiveGroupBox.Size = new System.Drawing.Size(182, 135);
            this.ObjectiveGroupBox.TabIndex = 21;
            this.ObjectiveGroupBox.TabStop = false;
            this.ObjectiveGroupBox.Text = "Objective";
            // 
            // ObjEliLabel
            // 
            this.ObjEliLabel.AutoSize = true;
            this.ObjEliLabel.Location = new System.Drawing.Point(109, 80);
            this.ObjEliLabel.Name = "ObjEliLabel";
            this.ObjEliLabel.Size = new System.Drawing.Size(47, 13);
            this.ObjEliLabel.TabIndex = 31;
            this.ObjEliLabel.Text = "Elitism %";
            // 
            // ObjEliInput
            // 
            this.ObjEliInput.Location = new System.Drawing.Point(112, 96);
            this.ObjEliInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ObjEliInput.Name = "ObjEliInput";
            this.ObjEliInput.Size = new System.Drawing.Size(46, 20);
            this.ObjEliInput.TabIndex = 30;
            this.ObjEliInput.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ObjMutLabel
            // 
            this.ObjMutLabel.AutoSize = true;
            this.ObjMutLabel.Location = new System.Drawing.Point(109, 24);
            this.ObjMutLabel.Name = "ObjMutLabel";
            this.ObjMutLabel.Size = new System.Drawing.Size(59, 13);
            this.ObjMutLabel.TabIndex = 29;
            this.ObjMutLabel.Text = "Mutation %";
            // 
            // ObjMutInput
            // 
            this.ObjMutInput.Location = new System.Drawing.Point(112, 40);
            this.ObjMutInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ObjMutInput.Name = "ObjMutInput";
            this.ObjMutInput.Size = new System.Drawing.Size(46, 20);
            this.ObjMutInput.TabIndex = 28;
            this.ObjMutInput.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ObjPopLable
            // 
            this.ObjPopLable.AutoSize = true;
            this.ObjPopLable.Location = new System.Drawing.Point(9, 24);
            this.ObjPopLable.Name = "ObjPopLable";
            this.ObjPopLable.Size = new System.Drawing.Size(84, 13);
            this.ObjPopLable.TabIndex = 21;
            this.ObjPopLable.Text = "Initial Population";
            // 
            // ObjPopInput
            // 
            this.ObjPopInput.Location = new System.Drawing.Point(12, 40);
            this.ObjPopInput.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ObjPopInput.Name = "ObjPopInput";
            this.ObjPopInput.Size = new System.Drawing.Size(46, 20);
            this.ObjPopInput.TabIndex = 19;
            this.ObjPopInput.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // ObjGenLabel
            // 
            this.ObjGenLabel.AutoSize = true;
            this.ObjGenLabel.Location = new System.Drawing.Point(6, 80);
            this.ObjGenLabel.Name = "ObjGenLabel";
            this.ObjGenLabel.Size = new System.Drawing.Size(64, 13);
            this.ObjGenLabel.TabIndex = 20;
            this.ObjGenLabel.Text = "Generations";
            // 
            // ObjGenInput
            // 
            this.ObjGenInput.Location = new System.Drawing.Point(12, 96);
            this.ObjGenInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ObjGenInput.Name = "ObjGenInput";
            this.ObjGenInput.Size = new System.Drawing.Size(46, 20);
            this.ObjGenInput.TabIndex = 23;
            this.ObjGenInput.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // advancedSettingsPanel
            // 
            this.advancedSettingsPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.advancedSettingsPanel.Controls.Add(this.gaGroupBox);
            this.advancedSettingsPanel.Location = new System.Drawing.Point(0, 555);
            this.advancedSettingsPanel.Name = "advancedSettingsPanel";
            this.advancedSettingsPanel.Size = new System.Drawing.Size(940, 179);
            this.advancedSettingsPanel.TabIndex = 33;
            this.advancedSettingsPanel.Visible = false;
            // 
            // SugestionVisibilityButton
            // 
            this.SugestionVisibilityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SugestionVisibilityButton.Image = global::Log2CyclePrototype.Properties.Resources.visibility_icon;
            this.SugestionVisibilityButton.Location = new System.Drawing.Point(753, 600);
            this.SugestionVisibilityButton.Name = "SugestionVisibilityButton";
            this.SugestionVisibilityButton.Size = new System.Drawing.Size(40, 40);
            this.SugestionVisibilityButton.TabIndex = 41;
            this.TooltipControl.SetToolTip(this.SugestionVisibilityButton, "Toggle suggestion visibility");
            this.SugestionVisibilityButton.UseVisualStyleBackColor = true;
            this.SugestionVisibilityButton.Click += new System.EventHandler(this.SugestionVisibilityButton_Click);
            // 
            // ClearSelectionButton
            // 
            this.ClearSelectionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearSelectionButton.Image = global::Log2CyclePrototype.Properties.Resources.eraser_filled;
            this.ClearSelectionButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ClearSelectionButton.Location = new System.Drawing.Point(600, 600);
            this.ClearSelectionButton.Name = "ClearSelectionButton";
            this.ClearSelectionButton.Size = new System.Drawing.Size(40, 40);
            this.ClearSelectionButton.TabIndex = 40;
            this.TooltipControl.SetToolTip(this.ClearSelectionButton, "Clear selection");
            this.ClearSelectionButton.UseVisualStyleBackColor = true;
            this.ClearSelectionButton.Click += new System.EventHandler(this.gridPanelContextMenuClear_Click);
            // 
            // InvertSelectionButton
            // 
            this.InvertSelectionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InvertSelectionButton.Image = global::Log2CyclePrototype.Properties.Resources.invert_selection;
            this.InvertSelectionButton.Location = new System.Drawing.Point(702, 600);
            this.InvertSelectionButton.Name = "InvertSelectionButton";
            this.InvertSelectionButton.Size = new System.Drawing.Size(40, 40);
            this.InvertSelectionButton.TabIndex = 39;
            this.TooltipControl.SetToolTip(this.InvertSelectionButton, "Invert Current Selection");
            this.InvertSelectionButton.UseVisualStyleBackColor = true;
            this.InvertSelectionButton.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // HitmeButton
            // 
            this.HitmeButton.Enabled = false;
            this.HitmeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HitmeButton.Image = global::Log2CyclePrototype.Properties.Resources.hitme_icon_small;
            this.HitmeButton.Location = new System.Drawing.Point(831, 600);
            this.HitmeButton.Name = "HitmeButton";
            this.HitmeButton.Size = new System.Drawing.Size(40, 40);
            this.HitmeButton.TabIndex = 38;
            this.TooltipControl.SetToolTip(this.HitmeButton, "Next best solution (disabled)");
            this.HitmeButton.UseVisualStyleBackColor = true;
            this.HitmeButton.Visible = false;
            this.HitmeButton.Click += new System.EventHandler(this.HitmeButton_Click);
            // 
            // NextSolutionButton
            // 
            this.NextSolutionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextSolutionButton.Image = global::Log2CyclePrototype.Properties.Resources.next_icon_small;
            this.NextSolutionButton.Location = new System.Drawing.Point(877, 600);
            this.NextSolutionButton.Name = "NextSolutionButton";
            this.NextSolutionButton.Size = new System.Drawing.Size(40, 40);
            this.NextSolutionButton.TabIndex = 37;
            this.TooltipControl.SetToolTip(this.NextSolutionButton, "Next suggestion");
            this.NextSolutionButton.UseVisualStyleBackColor = true;
            this.NextSolutionButton.Click += new System.EventHandler(this.NextSolutionButton_Click);
            // 
            // PreviousSolutionButton
            // 
            this.PreviousSolutionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviousSolutionButton.Image = global::Log2CyclePrototype.Properties.Resources.previous_icon_small;
            this.PreviousSolutionButton.Location = new System.Drawing.Point(404, 600);
            this.PreviousSolutionButton.Name = "PreviousSolutionButton";
            this.PreviousSolutionButton.Size = new System.Drawing.Size(40, 40);
            this.PreviousSolutionButton.TabIndex = 36;
            this.TooltipControl.SetToolTip(this.PreviousSolutionButton, "Previous suggestion");
            this.PreviousSolutionButton.UseVisualStyleBackColor = true;
            this.PreviousSolutionButton.Click += new System.EventHandler(this.PreviousSolutionButton_Click);
            // 
            // advancedSettingsButton
            // 
            this.advancedSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.advancedSettingsButton.Image = global::Log2CyclePrototype.Properties.Resources.wrench_icon_small;
            this.advancedSettingsButton.Location = new System.Drawing.Point(18, 600);
            this.advancedSettingsButton.Name = "advancedSettingsButton";
            this.advancedSettingsButton.Size = new System.Drawing.Size(40, 40);
            this.advancedSettingsButton.TabIndex = 34;
            this.TooltipControl.SetToolTip(this.advancedSettingsButton, "Advanced Settings");
            this.advancedSettingsButton.UseVisualStyleBackColor = true;
            this.advancedSettingsButton.Click += new System.EventHandler(this.ToggleAdvancedSettings);
            // 
            // ApplyButton
            // 
            this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyButton.Image = global::Log2CyclePrototype.Properties.Resources.apply_icon_small;
            this.ApplyButton.Location = new System.Drawing.Point(651, 600);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(40, 40);
            this.ApplyButton.TabIndex = 29;
            this.TooltipControl.SetToolTip(this.ApplyButton, "Export selection/suggestion");
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.gridPanelContextMenuExport_Click);
            // 
            // RevertButton
            // 
            this.RevertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RevertButton.Image = global::Log2CyclePrototype.Properties.Resources.undo_icon_small;
            this.RevertButton.Location = new System.Drawing.Point(545, 600);
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(40, 40);
            this.RevertButton.TabIndex = 32;
            this.TooltipControl.SetToolTip(this.RevertButton, "Undo changes");
            this.RevertButton.UseVisualStyleBackColor = true;
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // highlightRemovedCheckbox
            // 
            this.highlightRemovedCheckbox.AutoSize = true;
            this.highlightRemovedCheckbox.Checked = true;
            this.highlightRemovedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.highlightRemovedCheckbox.Location = new System.Drawing.Point(15, 27);
            this.highlightRemovedCheckbox.Name = "highlightRemovedCheckbox";
            this.highlightRemovedCheckbox.Size = new System.Drawing.Size(97, 17);
            this.highlightRemovedCheckbox.TabIndex = 42;
            this.highlightRemovedCheckbox.Text = "Removed Cells";
            this.highlightRemovedCheckbox.UseVisualStyleBackColor = true;
            this.highlightRemovedCheckbox.CheckedChanged += new System.EventHandler(this.defaultHighlight_CheckedChanged);
            // 
            // highlightAddedCheckbox
            // 
            this.highlightAddedCheckbox.AutoSize = true;
            this.highlightAddedCheckbox.Checked = true;
            this.highlightAddedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.highlightAddedCheckbox.Location = new System.Drawing.Point(128, 27);
            this.highlightAddedCheckbox.Name = "highlightAddedCheckbox";
            this.highlightAddedCheckbox.Size = new System.Drawing.Size(82, 17);
            this.highlightAddedCheckbox.TabIndex = 43;
            this.highlightAddedCheckbox.Text = "Added Cells";
            this.highlightAddedCheckbox.UseVisualStyleBackColor = true;
            this.highlightAddedCheckbox.CheckedChanged += new System.EventHandler(this.defaultHighlight_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.highlightAddedCheckbox);
            this.groupBox1.Controls.Add(this.highlightRemovedCheckbox);
            this.groupBox1.Location = new System.Drawing.Point(143, 590);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 54);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Suggestion Highlight";
            // 
            // sliderTooltip
            // 
            this.sliderTooltip.AutomaticDelay = 0;
            this.sliderTooltip.AutoPopDelay = 15000;
            this.sliderTooltip.InitialDelay = 1;
            this.sliderTooltip.ReshowDelay = 100;
            // 
            // userInfoButton
            // 
            this.userInfoButton.BackColor = System.Drawing.SystemColors.Control;
            this.userInfoButton.FlatAppearance.BorderSize = 0;
            this.userInfoButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.userInfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.userInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userInfoButton.Image = ((System.Drawing.Image)(resources.GetObject("userInfoButton.Image")));
            this.userInfoButton.Location = new System.Drawing.Point(340, 81);
            this.userInfoButton.Name = "userInfoButton";
            this.userInfoButton.Size = new System.Drawing.Size(20, 20);
            this.userInfoButton.TabIndex = 18;
            this.userInfoButton.Tag = "user";
            this.sliderTooltip.SetToolTip(this.userInfoButton, "Enables/Disables the generation of content based on the user map");
            this.userInfoButton.UseVisualStyleBackColor = false;
            // 
            // objInfoButton
            // 
            this.objInfoButton.BackColor = System.Drawing.SystemColors.Control;
            this.objInfoButton.FlatAppearance.BorderSize = 0;
            this.objInfoButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.objInfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.objInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.objInfoButton.Image = ((System.Drawing.Image)(resources.GetObject("objInfoButton.Image")));
            this.objInfoButton.Location = new System.Drawing.Point(215, 81);
            this.objInfoButton.Name = "objInfoButton";
            this.objInfoButton.Size = new System.Drawing.Size(20, 20);
            this.objInfoButton.TabIndex = 17;
            this.objInfoButton.Tag = "obj";
            this.sliderTooltip.SetToolTip(this.objInfoButton, "Enable/Disable the generation of objective oriented content");
            this.objInfoButton.UseVisualStyleBackColor = false;
            // 
            // novInfoButton
            // 
            this.novInfoButton.BackColor = System.Drawing.SystemColors.Control;
            this.novInfoButton.FlatAppearance.BorderSize = 0;
            this.novInfoButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.novInfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.novInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.novInfoButton.Image = ((System.Drawing.Image)(resources.GetObject("novInfoButton.Image")));
            this.novInfoButton.Location = new System.Drawing.Point(90, 81);
            this.novInfoButton.Name = "novInfoButton";
            this.novInfoButton.Size = new System.Drawing.Size(20, 20);
            this.novInfoButton.TabIndex = 16;
            this.novInfoButton.Tag = "nov";
            this.sliderTooltip.SetToolTip(this.novInfoButton, "Enables/Disables the generation of innovative content");
            this.novInfoButton.UseVisualStyleBackColor = false;
            // 
            // sliderGroupBox
            // 
            this.sliderGroupBox.Controls.Add(this.userToggleSwitch);
            this.sliderGroupBox.Controls.Add(this.objectiveToggleSwitch);
            this.sliderGroupBox.Controls.Add(this.noveltyToggleSwitch);
            this.sliderGroupBox.Controls.Add(this.comboLabel);
            this.sliderGroupBox.Controls.Add(this.objectiveCombobox);
            this.sliderGroupBox.Controls.Add(this.userInfoButton);
            this.sliderGroupBox.Controls.Add(this.objInfoButton);
            this.sliderGroupBox.Controls.Add(this.novInfoButton);
            this.sliderGroupBox.Controls.Add(this.sliderGroupLabel);
            this.sliderGroupBox.Controls.Add(this.noveltySlider);
            this.sliderGroupBox.Controls.Add(this.objectiveSlider);
            this.sliderGroupBox.Controls.Add(this.userSketchSlider);
            this.sliderGroupBox.Controls.Add(this.label3);
            this.sliderGroupBox.Controls.Add(this.label2);
            this.sliderGroupBox.Controls.Add(this.label1);
            this.sliderGroupBox.Location = new System.Drawing.Point(18, 43);
            this.sliderGroupBox.Name = "sliderGroupBox";
            this.sliderGroupBox.Size = new System.Drawing.Size(371, 393);
            this.sliderGroupBox.TabIndex = 18;
            this.sliderGroupBox.TabStop = false;
            this.sliderGroupBox.Text = "Algorithm behaviour";
            // 
            // userToggleSwitch
            // 
            this.userToggleSwitch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.userToggleSwitch.Checked = true;
            this.userToggleSwitch.Location = new System.Drawing.Point(268, 169);
            this.userToggleSwitch.Name = "userToggleSwitch";
            this.userToggleSwitch.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.userToggleSwitch.OffForeColor = System.Drawing.Color.LightGray;
            this.userToggleSwitch.OffSideScaleImageToFit = true;
            this.userToggleSwitch.OffText = "Off";
            this.userToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.userToggleSwitch.OnForeColor = System.Drawing.Color.White;
            this.userToggleSwitch.OnSideScaleImageToFit = true;
            this.userToggleSwitch.OnText = "On";
            this.userToggleSwitch.Size = new System.Drawing.Size(90, 30);
            this.userToggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Carbon;
            this.userToggleSwitch.TabIndex = 23;
            this.userToggleSwitch.Tag = "user";
            this.userToggleSwitch.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.toggleSwitchValueChanged_Callback);
            // 
            // objectiveToggleSwitch
            // 
            this.objectiveToggleSwitch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.objectiveToggleSwitch.Checked = true;
            this.objectiveToggleSwitch.Location = new System.Drawing.Point(141, 169);
            this.objectiveToggleSwitch.Name = "objectiveToggleSwitch";
            this.objectiveToggleSwitch.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.objectiveToggleSwitch.OffForeColor = System.Drawing.Color.LightGray;
            this.objectiveToggleSwitch.OffSideScaleImageToFit = true;
            this.objectiveToggleSwitch.OffText = "Off";
            this.objectiveToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.objectiveToggleSwitch.OnForeColor = System.Drawing.Color.White;
            this.objectiveToggleSwitch.OnSideScaleImageToFit = true;
            this.objectiveToggleSwitch.OnText = "On";
            this.objectiveToggleSwitch.Size = new System.Drawing.Size(90, 30);
            this.objectiveToggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Carbon;
            this.objectiveToggleSwitch.TabIndex = 22;
            this.objectiveToggleSwitch.Tag = "obj";
            this.objectiveToggleSwitch.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.toggleSwitchValueChanged_Callback);
            // 
            // noveltyToggleSwitch
            // 
            this.noveltyToggleSwitch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.noveltyToggleSwitch.Checked = true;
            this.noveltyToggleSwitch.Location = new System.Drawing.Point(13, 169);
            this.noveltyToggleSwitch.Name = "noveltyToggleSwitch";
            this.noveltyToggleSwitch.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.noveltyToggleSwitch.OffForeColor = System.Drawing.Color.LightGray;
            this.noveltyToggleSwitch.OffSideScaleImageToFit = true;
            this.noveltyToggleSwitch.OffText = "Off";
            this.noveltyToggleSwitch.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.noveltyToggleSwitch.OnForeColor = System.Drawing.Color.White;
            this.noveltyToggleSwitch.OnSideScaleImageToFit = true;
            this.noveltyToggleSwitch.OnText = "On";
            this.noveltyToggleSwitch.Size = new System.Drawing.Size(90, 30);
            this.noveltyToggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Carbon;
            this.noveltyToggleSwitch.TabIndex = 21;
            this.noveltyToggleSwitch.Tag = "nov";
            this.noveltyToggleSwitch.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.toggleSwitchValueChanged_Callback);
            // 
            // comboLabel
            // 
            this.comboLabel.AutoSize = true;
            this.comboLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.comboLabel.Location = new System.Drawing.Point(24, 343);
            this.comboLabel.Name = "comboLabel";
            this.comboLabel.Size = new System.Drawing.Size(78, 20);
            this.comboLabel.TabIndex = 20;
            this.comboLabel.Text = "Objective:";
            // 
            // objectiveCombobox
            // 
            this.objectiveCombobox.FormattingEnabled = true;
            this.objectiveCombobox.Location = new System.Drawing.Point(112, 343);
            this.objectiveCombobox.Name = "objectiveCombobox";
            this.objectiveCombobox.Size = new System.Drawing.Size(223, 21);
            this.objectiveCombobox.TabIndex = 19;
            // 
            // sliderGroupLabel
            // 
            this.sliderGroupLabel.AutoSize = true;
            this.sliderGroupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.sliderGroupLabel.Location = new System.Drawing.Point(68, 30);
            this.sliderGroupLabel.Name = "sliderGroupLabel";
            this.sliderGroupLabel.Size = new System.Drawing.Size(239, 20);
            this.sliderGroupLabel.TabIndex = 15;
            this.sliderGroupLabel.Text = "Show suggestions based on:";
            this.sliderGroupLabel.Click += new System.EventHandler(this.sliderGroupLabel_Click);
            // 
            // noveltySlider
            // 
            this.noveltySlider.BackColor = System.Drawing.SystemColors.Control;
            this.noveltySlider.EmptyTrackColor = System.Drawing.Color.Black;
            this.noveltySlider.Location = new System.Drawing.Point(32, 127);
            this.noveltySlider.Name = "noveltySlider";
            this.noveltySlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.noveltySlider.ScaleFieldEqualizeHeights = true;
            this.noveltySlider.ScaleFieldMaxHeight = 20;
            this.noveltySlider.ScaleFieldSpacing = 3;
            this.noveltySlider.ScaleFieldWidth = 5;
            this.noveltySlider.Size = new System.Drawing.Size(70, 179);
            this.noveltySlider.SliderButtonSize = new System.Drawing.Size(15, 25);
            this.noveltySlider.SmallChange = 5;
            this.noveltySlider.TabIndex = 14;
            this.noveltySlider.Tag = "nov";
            this.noveltySlider.TickEmphasizedHeight = 6;
            this.noveltySlider.TickHeight = 2;
            this.noveltySlider.TickSpacing = 2;
            this.noveltySlider.UseSeeking = false;
            this.noveltySlider.Value = 20;
            this.noveltySlider.Visible = false;
            this.noveltySlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sliderValueChanged_Callback);
            // 
            // objectiveSlider
            // 
            this.objectiveSlider.BackColor = System.Drawing.SystemColors.Control;
            this.objectiveSlider.EmptyTrackColor = System.Drawing.Color.Black;
            this.objectiveSlider.Location = new System.Drawing.Point(156, 127);
            this.objectiveSlider.Name = "objectiveSlider";
            this.objectiveSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.objectiveSlider.ScaleFieldEqualizeHeights = true;
            this.objectiveSlider.ScaleFieldMaxHeight = 20;
            this.objectiveSlider.ScaleFieldSpacing = 3;
            this.objectiveSlider.ScaleFieldWidth = 5;
            this.objectiveSlider.Size = new System.Drawing.Size(70, 179);
            this.objectiveSlider.SliderButtonSize = new System.Drawing.Size(15, 25);
            this.objectiveSlider.SmallChange = 5;
            this.objectiveSlider.TabIndex = 13;
            this.objectiveSlider.Tag = "obj";
            this.objectiveSlider.TickEmphasizedHeight = 6;
            this.objectiveSlider.TickHeight = 2;
            this.objectiveSlider.TickSpacing = 2;
            this.objectiveSlider.UseSeeking = false;
            this.objectiveSlider.Value = 20;
            this.objectiveSlider.Visible = false;
            this.objectiveSlider.ValueChanged += new Winamp.Components.WinampTrackBar.ValueChangedDelegate(this.objectiveSlider_ValueChanged);
            this.objectiveSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sliderValueChanged_Callback);
            // 
            // userSketchSlider
            // 
            this.userSketchSlider.BackColor = System.Drawing.SystemColors.Control;
            this.userSketchSlider.EmptyTrackColor = System.Drawing.Color.Black;
            this.userSketchSlider.Location = new System.Drawing.Point(274, 127);
            this.userSketchSlider.Name = "userSketchSlider";
            this.userSketchSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.userSketchSlider.ScaleFieldEqualizeHeights = true;
            this.userSketchSlider.ScaleFieldMaxHeight = 20;
            this.userSketchSlider.ScaleFieldSpacing = 3;
            this.userSketchSlider.ScaleFieldWidth = 5;
            this.userSketchSlider.Size = new System.Drawing.Size(70, 179);
            this.userSketchSlider.SliderButtonSize = new System.Drawing.Size(15, 25);
            this.userSketchSlider.SmallChange = 5;
            this.userSketchSlider.TabIndex = 12;
            this.userSketchSlider.Tag = "user";
            this.userSketchSlider.TickEmphasizedHeight = 6;
            this.userSketchSlider.TickHeight = 2;
            this.userSketchSlider.TickSpacing = 2;
            this.userSketchSlider.UseSeeking = false;
            this.userSketchSlider.Value = 20;
            this.userSketchSlider.Visible = false;
            this.userSketchSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sliderValueChanged_Callback);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Innovation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(142, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Objective";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(264, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "User Map";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(940, 733);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sliderGroupBox);
            this.Controls.Add(this.SugestionVisibilityButton);
            this.Controls.Add(this.ClearSelectionButton);
            this.Controls.Add(this.InvertSelectionButton);
            this.Controls.Add(this.HitmeButton);
            this.Controls.Add(this.NextSolutionButton);
            this.Controls.Add(this.PreviousSolutionButton);
            this.Controls.Add(this.advancedSettingsButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.RevertButton);
            this.Controls.Add(this.advancedSettingsPanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LoG2 Editor Cycle Prototype";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.gridPanelContextMenu.ResumeLayout(false);
            this.HooksGroup.ResumeLayout(false);
            this.HooksGroup.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gaGroupBox.ResumeLayout(false);
            this.generalSettingsGroupBox.ResumeLayout(false);
            this.generalSettingsGroupBox.PerformLayout();
            this.crossOverTypeGroupBox.ResumeLayout(false);
            this.crossOverTypeGroupBox.PerformLayout();
            this.NoveltyGroupBox.ResumeLayout(false);
            this.NoveltyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NovEliInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovGenInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovMutInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NovPopInput)).EndInit();
            this.ObjectiveGroupBox.ResumeLayout(false);
            this.ObjectiveGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjEliInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjMutInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjPopInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjGenInput)).EndInit();
            this.advancedSettingsPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.sliderGroupBox.ResumeLayout(false);
            this.sliderGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #region CALLBACKS



        #region BUTTONS


        void ButtonStartClick(object sender, System.EventArgs e)
        {
            _actHook.Start(true, false);
        }

        void ButtonStopClick(object sender, System.EventArgs e)
        {
            _actHook.Stop(true, false, false);
        }

        void ButtonReloadClick(object sender, System.EventArgs e)
        {
            _actHook.SendReloadCommand();
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            if (APIClass._emergencyRestoreMap != null)
            {

                double dif = APIClass.CalculateDifference(APIClass._emergencyRestoreMap, currentMap);
                if (dif > 0)
                {
                    //solutionChromosomeMap = APIClass.MapObjectFromChromosome(solutionHistory[solutionHistory.Count-1]);
                    previousMap = currentMap = APIClass._emergencyRestoreMap;
                    bool res = APIClass.SaveMapFile(currentMap);
                    ReDraw();
                }
                else
                {
                    Debug.WriteLine("Nothing to revert.");
                    Logger.AppendText("Nothing to revert");
                }
            }
            else
            {
                Debug.WriteLine("No previous Map to restore");
                Logger.AppendText("No previous Map to restore!");
            }
        }

        void ButtonSaveClick(object sender, System.EventArgs e)
        {

            if (solutionChromosomeMap != null)
            {
                var res = APIClass.SaveMapFile(solutionChromosomeMap);
                if (res)
                {

                    //currentMap = APIClass.ParseMapFile();
                    //previousMap = currentMap;
                    //_actHook.SendReloadCommand();
                    //ReDraw();
                }
            }
            else Debug.WriteLine("Cant save, solution map null");
        }

        private void hooksToggleButton_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            if (b.Text == "On") //turn off
            {
                autoSuggestions = false;
                runAlgorithmButton.Enabled = true;
                _actHook.Stop();

                b.Text = "Off";
                Logger.AppendText("Auto-suggestions are now off");
            }
            else //turn on
            {
                autoSuggestions = true;
                runAlgorithmButton.Enabled = false;
                _actHook.Start(true, false); //installs only the mouse hook

                b.Text = "On";
                Logger.AppendText("Auto-suggestions are now on");

                Logger.AppendText("Executing algorithm...");
                ParseMapAndRunAlgorithm();

            }
        }

        private void runAlgorithmButton_Click(object sender, EventArgs e)
        {
            ParseMapAndRunAlgorithm();
        }

        void TestButton_Click(object sender, System.EventArgs e)
        {

            if (objRunning || novRunning)
                return;

            //_apiClass.FillRandomCell();
            //ReDraw();
            //_apiClass.SaveMapFile();

            //MazeSolver m = new MazeSolver(32, 32);

            //foreach (Cell c in currentMap.Cells)
            //    if (c.IsWalkable)
            //        m[c.X, c.Y] = 0;
            //    else
            //        m[c.X, c.Y] = 1;
            //int[,] iSolvedMaze = null;
            //try
            //{
            //    iSolvedMaze = m.FindPath(16, 16, 31, 31);
            //}
            //catch (Exception ex) { DebugUtilities.DebugException(ex); }

            //DebugUtilities.Debug2DMazeArray(iSolvedMaze, 32, 32);


            //int i = 0;
            //foreach (var c in solutionHistory)
            //{
            //    WriteUtilities.WriteSolution(i++, DirectoryManager.ProjectDir + @"\solution_history", c, APIClass.UnwalkableCellValue);
            //}

            //if (gridPanel.InvokeRequired)
            //{
            //    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
            //}
            //else
            //{
            //    ReDraw();
            //}

            //var form = new Form();
            //var r = form.ShowDialog();
            //if (r == DialogResult.OK)
            //{
            //    Logger.AppendText("derp");
            //}
        }

        private void PreviousSolutionButton_Click(object sender, EventArgs e)
        {
            if (_currentSolutionIndex < 0)
                return;
            else if (_currentSolutionIndex == 0)
            {
                //PreviousSolutionButton.Enabled = false;
                Logger.AppendText("No previous solution to show");
            }
            else
            {
                _currentSolutionIndex--;
                solutionChromosomeMap = solutionHistory[_currentSolutionIndex];
                _cellsToDraw = solutionChromosomeMap.Cells;
                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    ReDraw();
                }
            }
        }

        private void HitmeButton_Click(object sender, EventArgs e)
        {
            var nextSolGenes = _objAlgTest.GetBestNthSolution(_hitmeCount++);

            if (nextSolGenes == null)
            {
                Logger.AppendText("No different suggestions left");
            }
            else
            {
                var nextSol = new Chromosome(nextSolGenes);
                solutionChromosomeMap = APIClass.MapObjectFromChromosome(nextSol);
                solutionHistory.Add(solutionChromosomeMap.CloneJson());
                _currentSolutionIndex = solutionHistory.Count - 1;
                _cellsToDraw = solutionChromosomeMap.Cells;
                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    ReDraw();
                }
            }
        }

        private void NextSolutionButton_Click(object sender, EventArgs e)
        {
            if (_currentSolutionIndex < 0)
                return;
            else if (_currentSolutionIndex == solutionHistory.Count - 1)
            {
                //PreviousSolutionButton.Enabled = false;
                Logger.AppendText("No next solution to show");
            }
            else
            {
                _currentSolutionIndex++;
                solutionChromosomeMap = solutionHistory[_currentSolutionIndex];
                _cellsToDraw = solutionChromosomeMap.Cells;
                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    ReDraw();
                }
            }
        }

        bool t = false;
        private void ToggleAdvancedSettings(object sender, EventArgs e)
        {
            if (t)
            {
                this.Height = COLLAPSED_SIZE;
                advancedSettingsPanel.Visible = false;
            }
            else {
                this.Height = EXPANDED_SIZE;
                advancedSettingsPanel.Visible = true;
            }
            t = !t;
        }

        #endregion

        #region TOGGLE_SWITCHES

        float userSketchPrevVal = 0;
        private float _totalPercents = 100f;

        private void toggleSwitchValueChanged_Callback(object sender, EventArgs e)
        {
            var toggle = (JCS.ToggleSwitch)sender;
            _totalPercents = (userToggleSwitch.Checked ? userSliderDefaultVal : 0) + (objectiveToggleSwitch.Checked ? objSliderDefaultVal : 0) + (noveltyToggleSwitch.Checked ? novSliderDefaultVal : 0);
            switch (toggle.Tag.ToString())
            {
                case "user":
                    if (userToggleSwitch.Checked){
                        _userSketchInfluencePercent = userSliderDefaultVal / _totalPercents;
                        userSketchSlider.Value = userSliderDefaultVal;
                    }
                    else {
                        _userSketchInfluencePercent = 0;
                        userSketchSlider.Value = 0;
                    }
                    //ResetAlgorithm();
                    break;
                case "obj":
                    if (objectiveToggleSwitch.Checked)
                    {
                        _objectiveInfluencePercent = objSliderDefaultVal / _totalPercents;
                        objectiveSlider.Value = objSliderDefaultVal;
                    }
                    else {
                        _objectiveInfluencePercent = 0;
                        objectiveSlider.Value = 0;
                    }
                    break;
                case "nov":
                    if (noveltyToggleSwitch.Checked)
                    {
                        _noveltyInfluencePercent = novSliderDefaultVal / _totalPercents;
                        noveltySlider.Value = novSliderDefaultVal;
                    }
                    else {
                        _noveltyInfluencePercent = 0;
                        noveltySlider.Value = 0;
                    }
                    break;
                default:
                    break;
            }
            ResetAlgorithm();

        }

        #endregion

        private bool simpleControls = true;
        private int userSliderLastVal, objSliderLastVal, novSliderLastVal;
        private void inputPickerCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            string tag = ((ToolStripMenuItem)sender).Tag.ToString();
            //if ( (tag == "standard" && !simpleControls) || (tag == "expert" && simpleControls))
            //    return;

            if (!simpleControls && tag=="standard")
            {
                standardModeToolStripMenuItem.Checked = true;
                expertModeToolStripMenuItem.Checked = false;

                userSketchSlider.Visible = false;
                objectiveSlider.Visible = false;
                noveltySlider.Visible = false;

                userSliderLastVal = userSketchSlider.Value;
                objSliderLastVal = objectiveSlider.Value;
                novSliderLastVal = noveltySlider.Value;

                userToggleSwitch.Visible = true;
                objectiveToggleSwitch.Visible = true;
                noveltyToggleSwitch.Visible = true;

                if (_userSketchInfluencePercent > 0)
                    userToggleSwitch.Checked = true;
                else
                    userToggleSwitch.Checked = false;

                if (_objectiveInfluencePercent > 0)
                    objectiveToggleSwitch.Checked = true;
                else
                    objectiveToggleSwitch.Checked = false;

                if (_noveltyInfluencePercent > 0)
                    noveltyToggleSwitch.Checked = true;
                else
                    noveltyToggleSwitch.Checked = false;
            }
            else if (simpleControls && tag == "expert")
            {
                standardModeToolStripMenuItem.Checked = false;
                expertModeToolStripMenuItem.Checked = true;

                userSketchSlider.Visible = true;
                objectiveSlider.Visible = true;
                noveltySlider.Visible = true;

                userToggleSwitch.Visible = false;
                objectiveToggleSwitch.Visible = false;
                noveltyToggleSwitch.Visible = false;
            }
            simpleControls = !simpleControls;
            
        }


        #region SLIDERS

        private void sliderValueChanged_Callback(object sender, MouseEventArgs e)
        {
            var slider = (Winamp.Components.WinampTrackBar)sender;
            _totalPercents = (float)userSketchSlider.Value + (float)objectiveSlider.Value + (float)noveltySlider.Value;
            switch (slider.Tag.ToString())
            {
                case "user":
                    _userSketchInfluencePercent = userSketchSlider.Value / _totalPercents;
                    //if (_userSketchInfluencePercent > 0)
                    //    userToggleSwitch.Checked = true;
                    //else
                    //    userToggleSwitch.Checked = false;
                    if ((userSketchSlider.Value == 0 && userSketchPrevVal != 0) || (userSketchSlider.Value > 0 && userSketchPrevVal == 0))
                    {
                        ResetAlgorithm();
                        userSketchPrevVal = userSketchSlider.Value;
                    }
                    break;
                case "obj":
                    _objectiveInfluencePercent = objectiveSlider.Value / _totalPercents;
                    //if (_objectiveInfluencePercent > 0)
                    //    objectiveToggleSwitch.Checked = true;
                    //else
                    //    objectiveToggleSwitch.Checked = false;
                    break;
                case "nov":
                    _noveltyInfluencePercent = noveltySlider.Value / _totalPercents;
                    //if (_noveltyInfluencePercent > 0)
                    //    noveltyToggleSwitch.Checked = true;
                    //else
                    //    noveltyToggleSwitch.Checked = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region RADIOBUTTONS


        void defaultHighlight_CheckedChanged(object sender, EventArgs e)
        {
            
            if (highlightRemovedCheckbox != null && highlightAddedCheckbox != null)
            {
                if (!highlightRemovedCheckbox.Checked && !highlightAddedCheckbox.Checked)
                    _cellHighlightSettings = CellHighlight.None;
                else if (highlightRemovedCheckbox.Checked && highlightAddedCheckbox.Checked)
                    _cellHighlightSettings = CellHighlight.All;
                else if (highlightRemovedCheckbox.Checked && !highlightAddedCheckbox.Checked)
                    _cellHighlightSettings = CellHighlight.Removed;
                else if (!highlightRemovedCheckbox.Checked && highlightAddedCheckbox.Checked)
                    _cellHighlightSettings = CellHighlight.Added;
                }
                if(currentMap != null && _cellsToDraw.Count > 0)
                    ReDraw();
        }

        //menu bar crossovertype selection
        void CrossOverChangeCallback(object sender, EventArgs e)
        {
            //var itemTag = ((ToolStripMenuItem)sender).Tag;
            var item = (RadioButton)sender;
            var itemTag = item.Tag;

            if (item.Checked)
            {
                if (itemTag.ToString().Contains("sp"))
                {
                    CurrentCrossoverSelection = CrossoverType.SinglePoint;
                    Logger.AppendText("Crossover Type changed to Single Point");
                }
                else if (itemTag.ToString().Contains("dp"))
                {
                    CurrentCrossoverSelection = CrossoverType.DoublePoint;
                    Logger.AppendText("Crossover Type changed to Double Point");
                }
                else if (itemTag.ToString().Contains("2x2s"))
                {
                    CurrentCrossoverSelection = CrossoverType.TwoByTwoSquare;
                    Logger.AppendText("Crossover Type changed to custom 2x2 Square shape");
                }
                else if (itemTag.ToString().Contains("3x3s"))
                {
                    CurrentCrossoverSelection = CrossoverType.ThreeByThreeSquare;
                    Logger.AppendText("Crossover Type changed to custom 3x3 Square shape");
                }
                else if (itemTag.ToString().Contains("4x4s"))
                {
                    CurrentCrossoverSelection = CrossoverType.FourByFourSquare;
                    Logger.AppendText("Crossover Type changed to custom 4x4 Square shape");
                }
            }
            //foreach (ToolStripMenuItem m in CrossOverTypeMenuItem.DropDownItems)
            //    m.Checked = false;

            //((ToolStripMenuItem)sender).Checked = true;

        }


        #endregion

        #region MENUBAR


        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show(StringResources.ControlHelpString, "Program Controls");
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(StringResources.CreditsHelpString, "Credits");
        }

        bool validDir = false;
        private void selectProjectDirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var fd = new FolderBrowserDialog();
            //fd.SelectedPath = DirectoryManager.GogGamesDir;
            //fd.RootFolder = Environment.SpecialFolder.MyDocuments;
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                var folderName = fd.SelectedPath;
                var dirContents = new DirectoryInfo(folderName);
                var files = dirContents.GetFiles();
                foreach (FileInfo f in files)
                    if (f.ToString().Contains(".dungeon_editor"))
                    {
                        Logger.AppendText("Directory is a valid LoG2 Project directory.");
                        validDir = true;
                        break;
                    }

                if (validDir)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                    {

                        DirectoryManager.ProjectDir = fd.SelectedPath;
                        //DirectoryManager.StoreLastProjDir();
                        //_actHook.Start(true, false);
                        //_actHook = new UserActivityHook(true, false); // crate an instance with global hooks (mouse on / keyboard on)
                        //_actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
                        Invoke((MethodInvoker)(() => { _actHook.Start(true,false); }));
                        InitWatcher();
                        previousMap = currentMap = APIClass.ParseMapFile();
                        _objAlgTest = new ObjectiveAlgorithmTestClass();
                        _novAlgTest = new NoveltyAlgorithmTestClass();
                        _cellsToDraw = APIClass.CurrentMap.Cells;
                        ReDraw();
                        NoveltyAlgorithmTestClass.OnNoveltyAlgorithmFinished += ObjectiveAlgorithmTestClass.NoveltyAlgorithm_Finished;
                        ResetAlgorithm();
                        Logger.AppendText("Starting first algorithm run...");
                        RunAlgorithm();
                    }));
                }
                else Logger.AppendText(@"Directory is not valid. Please pick a project directory containing a "".dungeon_editor"" file");

            }
            else Logger.AppendText("Project directory not picked, please pick a LoG2 Project Directory");

        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
        private void resetAlgorithmCallback(object sender, EventArgs e)
        {
            ResetAlgorithm();
        }


        #endregion


        private void AttachHooks(object sender, EventArgs e)
        {
            //Logger.AppendText("Received monitor event");
            //if(DirectoryManager.ProjectDir != null || DirectoryManager.ProjectDir != "")
            //    _actHook.Start(true, false);

            _actHook = new UserActivityHook(true, false); // crate an instance with global hooks (mouse on / keyboard on)
            // hang on events
            _actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
        }


        //public static List<Point> _lockedUserSelection = new List<Point>();
        public static List<Cell> _lockedCellList = new List<Cell>();
        private void lockSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_userSelectedPoints != null && _userSelectedPoints.Count > 0)
            {

                foreach(var selectedPoint in _userSelectedPoints)
                {
                    if (solutionChromosomeMap == null) { //if no solution was presented yet
                        var tmpC = currentMap.Cells.Find(c => (c.X == selectedPoint.X && c.Y == selectedPoint.Y));
                        _lockedCellList.Add(tmpC.CloneJson());
                    }
                    else{
                        var tmpC = solutionChromosomeMap.Cells.Find(c => (c.X == selectedPoint.X && c.Y == selectedPoint.Y));
                        _lockedCellList.Add(tmpC.CloneJson());
                    }

                }
                //_lockedUserSelection = _userSelectedPoints.CloneJson();
                //if (solutionChromosomeMap == null) //if no solution was presented yet
                //    _lockedCellList = currentMap.Cells.CloneJson();
                //else
                //    _lockedCellList = solutionChromosomeMap.Cells.CloneJson();
                ClearSelectedUserCells();
                ((ToolStripItem)sender).Visible = false;
                unlockSelectionToolStripMenuItem.Visible = true;
                if (gridPanel.InvokeRequired)
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                else
                    ReDraw();
            }
            else
            {
                Logger.AppendText("No selection to invert.");
            }
        }
        
        private void unlockSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_lockedCellList != null && _lockedCellList.Count > 0)
            {
                _lockedCellList.Clear();
                //_lockedUserSelection.Clear();
                ((ToolStripItem)sender).Visible = false;
                lockSelectionToolStripMenuItem.Visible = true;
                if (gridPanel.InvokeRequired)
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                else
                    ReDraw();
            }
            else
            {
                Logger.AppendText("No selection to unlock.");
            }
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
                return;

            if(_userSelectedPoints != null && _userSelectedPoints.Count > 0)
            {
                var invertion = new List<Point>();
                for (int y = 0; y < APIClass.CurrentMap.Height; y++) {
                    for (int x = 0; x < APIClass.CurrentMap.Width; x++) {
                        if (_userSelectedPoints.FindIndex(c => (c.X == x && c.Y == y)) == -1)
                            invertion.Add(new Point(x,y));
                    }
                }
                _userSelectedPoints = invertion;
                if (gridPanel.InvokeRequired)
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                else
                    ReDraw();
            }
            else
            {
                Logger.AppendText("No selection to invert.");
            }
        }

        private void gridPanelContextMenuExport_Click(object sender, EventArgs e)
        {
            if (APIClass.CurrentMap == null)
                return;

            try
            {
                if (solutionChromosomeMap != null && _cellsToDraw != null && _userSelectedPoints != null)
                {
                    if (_userSelectedPoints.Count == 0)
                    {
                        Logger.AppendText("No selection, exporting entire solution.");
                        if (solutionChromosomeMap != null)
                        {
                            var res = APIClass.SaveMapFile(solutionChromosomeMap);
                        }
                        else Debug.WriteLine("Cant save, solution map null");
                        //return;
                    }
                    else {
                        Logger.AppendText("Exporting solution.");
                        bool result = APIClass.ExportSelection(solutionChromosomeMap, APIClass.CurrentMap, _userSelectedPoints); //use map vs list of genes
                        if (result)
                            _actHook.SendReloadCommand();
                        ClearSelectedUserCells();
                    }
                    if (gridPanel.InvokeRequired)
                    {
                        Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                    }
                    else
                    {
                        ReDraw();
                    }
                }
            }
            catch (CurrentMapNullException nulle)
            {
                Debug.WriteLine(nulle.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void gridPanelContextMenuClear_Click(object sender, EventArgs e)
        {
            ClearSelectedUserCells();
            ReDraw();
        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            //labelMousePosition.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0)
            {
                //LogWrite("MouseButton 	- " + e.Button.ToString());
                //LogWrite(actHook.GetApplicationMouseIsOver()); //get app name on mouse click
                //LogWrite(actHook.sendKeystroke('a', actHook.GetHandleOfWindowMouseIsOver())); //send simple key
                if (_actHook.GetApplicationMouseIsOver() == "grimrock2.exe")
                {
                    if(currentMap != null)
                        _actHook.SendSaveCommand(_actHook.GetHandleOfWindowMouseIsOver());
                }
            }
        }

        void Logger_EntryWritten(object sender, LogEntryEventArgs args)
        {
            if (textBox.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { textBox.AppendText(args.Message); textBox.AppendText(Environment.NewLine); }));
            }
            else
            {
                textBox.AppendText(args.Message);
                textBox.AppendText(Environment.NewLine);
            }
        }

        #endregion

        void InitCombobox()
        {
            try
            {
                List<ComboItem> userData = new List<ComboItem>();
                userData.Add(new ComboItem("Narrow Paths", Objective.NarrowPaths));
                userData.Add(new ComboItem("Rooms", Objective.Rooms));

                BindingSource bs = new BindingSource();
                bs.DataSource = userData;

                objectiveCombobox.DataSource = bs;
                objectiveCombobox.DisplayMember = "Key";
                objectiveCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
                
                objectiveCombobox.SelectedIndex = 0;
                _currObjective = Objective.NarrowPaths;

                objectiveCombobox.SelectedValueChanged += objectiveItemChanged;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);
            }
        }

        private Objective _currObjective;
        void objectiveItemChanged(object sender, EventArgs e)
        {
            ComboItem item = (ComboItem)((ComboBox)sender).SelectedItem;
            Logger.AppendText("Current Objective: " + item.Key.ToString());

            if (item.Value != _currObjective)
                ResetAlgorithm();

            _currObjective = item.Value;
        }

        void InitSlidersAndSwitches()
        {
            userSketchSlider.Value = userSliderDefaultVal;
            objectiveSlider.Value = objSliderDefaultVal;
            noveltySlider.Value = novSliderDefaultVal;

            userToggleSwitch.Checked = true;
            objectiveToggleSwitch.Checked = true;
            noveltyToggleSwitch.Checked = true;

            _totalPercents = userSketchSlider.Value + objectiveSlider.Value + noveltySlider.Value;            
            
            _userSketchInfluencePercent = userSketchSlider.Value/ _totalPercents;
            userSketchPrevVal = userSketchSlider.Value;

            _objectiveInfluencePercent = objectiveSlider.Value/ _totalPercents;
            _noveltyInfluencePercent = noveltySlider.Value/ _totalPercents;
        }
        
        void MainFormLoad(object sender, System.EventArgs e)
        {
            //init graphics
            _cellBitmap = new Bitmap(gridPanel.Width, gridPanel.Height);
            _cellPanelGraphics = Graphics.FromImage(_cellBitmap);
            _trianglePickerBitmap = new Bitmap(trianglePanel.Width, trianglePanel.Height);
            _trianglePickerGraphics = Graphics.FromImage(_trianglePickerBitmap);
            SetupTriangleControl();

            //Solves flickering when redrawing gridPanel and triangle Panel
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, gridPanel, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, trianglePanel, new object[] { true });
            
            Logger.EntryWritten += Logger_EntryWritten;

            DrawTriangle();
            InitSlidersAndSwitches();
            InitCombobox();

            solutionHistory = new List<Map>();
            _userSelectedPoints = _userSelectedCellsToErase = new List<Point>();                        
            CurrentCrossoverSelection = CrossoverType.FourByFourSquare;
            

            //_actHook = new UserActivityHook(true, false); // crate an instance with global hooks (mouse on / keyboard on)
            //// hang on events
            //_actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            //actHook.KeyDown+=new KeyEventHandler(MyKeyDown);
            //actHook.KeyPress+=new KeyPressEventHandler(MyKeyPress);
            //actHook.KeyUp+=new KeyEventHandler(MyKeyUp);
            AttachHooks(null,null);

            if (!LoG2ProcessFound)
            {
                //Logger.AppendText(StringResources.StartGrimrockString);
                //_actHook.Stop(true, false, false);
                _procMon = new ProcessMonitor();
                //ProcessMonitor.FoundLoG2Process += AttachHooks;
                _procMon.StartWatcher();
                return;
            }


            //This sets up the entire directory tree for the current project
            //DirectoryManager.ProjectDir = _defaultProjectDir;

            //Set the last dir as the current dir
            if (File.Exists("PersistentData.pd"))
            {
                DirectoryManager.RetrieveLastProjectDir();                
            }


            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                //If all is ok, setup file watcher and parse map and instanciate and initialize algorithms
                if (DirectoryManager.ProjectDir != null)
                {
                    //AttachHooks(null,null);

                    //Initialize file watchers
                    InitWatcher();

                    currentMap = APIClass.ParseMapFile();// APIC_apiClass.ParseMapFile2();            
                    previousMap = currentMap.CloneJson();

                    _cellsToDraw = APIClass.CurrentMap.Cells;

                    //Call redraw after map is parsed
                    ReDraw();

                    _objAlgTest = new ObjectiveAlgorithmTestClass();
                    _novAlgTest = new NoveltyAlgorithmTestClass();
                    NoveltyAlgorithmTestClass.OnNoveltyAlgorithmFinished += ObjectiveAlgorithmTestClass.NoveltyAlgorithm_Finished;

                    //Debug.WriteLine("Running Algorithms once");
                    RunAlgorithm();
                    //Debug.WriteLine("Donezo");
                    //Test();
                }
                else
                {
                    Debug.WriteLine(StringResources.PickDirString);
                    Logger.AppendText(StringResources.PickDirString);
                }
            }));


        }
        

        #region GRID PANEL SELECTION

        private Point RectStartPoint;
        private Rectangle SelectionRect = new Rectangle();        
        private Brush selectionBrushAdd = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        private Brush selectionBrushRemove = new SolidBrush(Color.FromArgb(128, 255, 59, 78));


        private void ClearSelectedUserCells()
        {
            _userSelectedPoints = new List<Point>();
            RectStartPoint = new Point(-1, -1);
        }

        bool selectionAdd = true;

        // Start Rectangle
        //
        private void gridPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Determine the initial rectangle coordinates...
            //Debug.WriteLine(e.Button.ToString()+" down @ ["+e.Location.X.ToString()+","+e.Location.Y.ToString()+"]");

            //if (_userSelectedCells.Count > 0)
            //{
            //    ClearSelectedUserCells();
            //    return;
            //}

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) //*&& (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control)*/)
            {
                RectStartPoint = e.Location;
                Invalidate();
                
                if (e.Button == MouseButtons.Left)
                {
                    CursorManager.Instance.SetCursor(CursorType.Plus);
                    selectionAdd = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    CursorManager.Instance.SetCursor(CursorType.Minus);
                    selectionAdd = false;
                }
 
            }

        }

        // Draw Rectangle
        //
        private void gridPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)//*&& (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control)*/)
            {
                Point tempEndPoint = e.Location;
                SelectionRect.Location = new Point(
                    System.Math.Min(RectStartPoint.X, tempEndPoint.X),
                    System.Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                SelectionRect.Size = new Size(
                    System.Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    System.Math.Abs(RectStartPoint.Y - tempEndPoint.Y));

                DrawSelectionPreviewRectangle();

            }

        }

        private void gridPanel_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (RectStartPoint.X == -1 && RectStartPoint.Y == -1)
            //    {
            //        ReDraw();
            //        return;
            //    }
            //}

            if (currentMap == null)
                return;

            try
            {

            //DELETE
            if (e.Button == MouseButtons.Right /*&& Control.ModifierKeys == Keys.Control*/)
            {

                cellWidth = gridPanel.Width / APIClass.CurrentMap.Width;
                cellHeight = gridPanel.Height / APIClass.CurrentMap.Height;
                Point topLeft = new Point(), bottomRight = new Point();
                Point startCellCoord = new Point(RectStartPoint.X / cellWidth, RectStartPoint.Y / cellHeight);
                Point endCellCoord = new Point(e.Location.X / cellWidth, e.Location.Y / cellHeight);

                //if 1 click on same cell
                if (startCellCoord.Equals(endCellCoord))
                {
                    Debug.WriteLine("Click @ [" + startCellCoord.X.ToString() + "," + startCellCoord.Y.ToString() + "]");
                    var tmpP = new Point(startCellCoord.X, startCellCoord.Y);
                    if (_userSelectedPoints.Contains(tmpP))
                        _userSelectedPoints.Remove(tmpP);
                    //_userSelectedCells.Add(new Point(startCellCoord.X, startCellCoord.Y));
                }
                else //if ended on different cell
                {

                    if (startCellCoord.X <= endCellCoord.X && startCellCoord.Y <= endCellCoord.Y) { topLeft = startCellCoord; bottomRight = endCellCoord; }
                    else if (startCellCoord.X < endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = new Point(startCellCoord.X, endCellCoord.Y); bottomRight = new Point(endCellCoord.X, startCellCoord.Y); }
                    else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y < endCellCoord.Y) { topLeft = new Point(endCellCoord.X, startCellCoord.Y); bottomRight = new Point(startCellCoord.X, endCellCoord.Y); }
                    else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = endCellCoord; bottomRight = startCellCoord; }

                    int cellSelectionXRange = System.Math.Abs(startCellCoord.X - endCellCoord.X);
                    int cellSelectionYRange = System.Math.Abs(startCellCoord.Y - endCellCoord.Y);

                    if (topLeft != null && bottomRight != null)
                    {
                        Debug.WriteLine(" TopLeft @ [" + topLeft.X.ToString() + "," + topLeft.Y.ToString() + "]");
                        Debug.WriteLine(" BottomRight @ [" + bottomRight.X.ToString() + "," + bottomRight.Y.ToString() + "]");
                    }


                    for (int j = topLeft.Y; j <= bottomRight.Y; j++)
                        for (int i = topLeft.X; i <= bottomRight.X; i++)
                        {
                            var tmpP = new Point(i, j);
                            if (_userSelectedPoints.Contains(tmpP))
                                _userSelectedPoints.Remove(tmpP);
                        }

                }

                //reset selection so it is no longer drawn on mouse up
                SelectionRect = new Rectangle(new Point(0, 0), new Size(0, 0));
                ReDraw();

            }
            //ADD
            else if (e.Button == MouseButtons.Left /*&& Control.ModifierKeys == Keys.Shift*/)
            {

                cellWidth = gridPanel.Width / APIClass.CurrentMap.Width;
                cellHeight = gridPanel.Height / APIClass.CurrentMap.Height;
                Point topLeft = new Point(), bottomRight = new Point();
                Point startCellCoord = new Point(RectStartPoint.X / cellWidth, RectStartPoint.Y / cellHeight);
                Point endCellCoord = new Point(e.Location.X / cellWidth, e.Location.Y / cellHeight);

                //if 1 click on same cell
                if (startCellCoord.Equals(endCellCoord))
                {
                    Debug.WriteLine("Click @ [" + startCellCoord.X.ToString() + "," + startCellCoord.Y.ToString() + "]");
                    var tmpP = new Point(startCellCoord.X, startCellCoord.Y);
                    if (!_userSelectedPoints.Contains(tmpP))
                        _userSelectedPoints.Add(tmpP);
                    else Debug.WriteLine("Point already in list, skipping");
                    //_userSelectedCells.Add(new Point(startCellCoord.X, startCellCoord.Y));
                }
                else //if ended on different cell
                {

                    if (startCellCoord.X <= endCellCoord.X && startCellCoord.Y <= endCellCoord.Y) { topLeft = startCellCoord; bottomRight = endCellCoord; }
                    else if (startCellCoord.X < endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = new Point(startCellCoord.X, endCellCoord.Y); bottomRight = new Point(endCellCoord.X, startCellCoord.Y); }
                    else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y < endCellCoord.Y) { topLeft = new Point(endCellCoord.X, startCellCoord.Y); bottomRight = new Point(startCellCoord.X, endCellCoord.Y); }
                    else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = endCellCoord; bottomRight = startCellCoord; }

                    int cellSelectionXRange = System.Math.Abs(startCellCoord.X - endCellCoord.X);
                    int cellSelectionYRange = System.Math.Abs(startCellCoord.Y - endCellCoord.Y);

                    if (topLeft != null && bottomRight != null)
                    {
                        Debug.WriteLine(" TopLeft @ [" + topLeft.X.ToString() + "," + topLeft.Y.ToString() + "]");
                        Debug.WriteLine(" BottomRight @ [" + bottomRight.X.ToString() + "," + bottomRight.Y.ToString() + "]");
                    }


                    for (int j = topLeft.Y; j <= bottomRight.Y; j++)
                        for (int i = topLeft.X; i <= bottomRight.X; i++)
                        {
                            var tmpP = new Point(i, j);
                            if (!_userSelectedPoints.Contains(tmpP))
                                _userSelectedPoints.Add(tmpP);
                            else Debug.WriteLine("Point already in list, skipping");
                        }

                }

                //reset selection so it is no longer drawn on mouse up
                SelectionRect = new Rectangle(new Point(0, 0), new Size(0, 0));
                ReDraw();

            }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void gridPanel_Click(object sender, EventArgs e)
        {
            //if (((MouseEventArgs)e).Button == MouseButtons.Right)
            //{
            //    Debug.WriteLine("Right Click");
            //    //Point pos = this.PointToClient(Cursor.Position);
            //    //gridPanelContextMenu.Show(this, pos);
            //    gridPanelContextMenu.Show(((MouseEventArgs)e).Location);
            //}
        }



        #endregion


        #region DRAWING


        #region TRIANGLE PICKER

        private int triangleSize = 80;
        private bool _hideSuggestionWhenSelecting = true;
        private void SugestionVisibilityButton_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
                return;

            _hideSuggestionWhenSelecting = !_hideSuggestionWhenSelecting;
            Logger.AppendText("Suggestion is"+(_hideSuggestionWhenSelecting == true? " hidden" : " visible")+".");
            if (gridPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { ReDraw(); }));
            }
            else
                ReDraw();
        }

        void SetupTriangleControl()
        {

            trianglePoint1 = new Point(10, triangleSize - 1);
            trianglePoint2 = new Point((int)(triangleSize / 2f) + 10, 10);
            trianglePoint3 = new Point(triangleSize + 10, triangleSize - 1);
            triangleDotPoint = new PointF((trianglePoint1.X + trianglePoint2.X + trianglePoint3.X) / 3, (trianglePoint1.Y + trianglePoint2.Y + trianglePoint3.Y) / 3);
            triangleDotRadius = 7;
        }

        private void CalculateTrianglePercentages()
        {
            var d1 = MathUtilities.DistanceBetweenPoints(triangleDotPoint, trianglePoint1);
            var d2 = MathUtilities.DistanceBetweenPoints(triangleDotPoint, trianglePoint2);
            var d3 = MathUtilities.DistanceBetweenPoints(triangleDotPoint, trianglePoint3);
            var total = d1 + d2 + d3;
            var w1 = System.Math.Abs(100 - (d1 * 100) / triangleSize);
            var w2 = System.Math.Abs(100 - (d2 * 100) / triangleSize);
            var w3 = System.Math.Abs(100 - (d3 * 100) / triangleSize);
            var p1 = (System.Math.Round(d1 / total * 100));
            var p2 = (System.Math.Round(d2 / total * 100));
            var p3 = (System.Math.Round(d3 / total * 100));

            //Debug.WriteLine(w1+ " - " + w2 + " - " + w3);
            //Logger.AppendText(p1+" - "+p2+" - "+p3);
        }

        private void DrawTriangle()
        {
            _trianglePickerGraphics.Clear(Color.FromName("Control"));
            using (Pen p = new Pen(Color.Black, 3f))
            {
                _trianglePickerGraphics.DrawPolygon(p, new Point[] { trianglePoint1, trianglePoint2, trianglePoint3 });
                _trianglePickerGraphics.FillEllipse(
                    new SolidBrush(Color.Black),
                    new RectangleF(new PointF(triangleDotPoint.X - triangleDotRadius, triangleDotPoint.Y - triangleDotRadius),
                    new SizeF(new PointF(triangleDotRadius * 2, triangleDotRadius * 2))));
            }
            trianglePanel.BackgroundImage = _trianglePickerBitmap;
            trianglePanel.Refresh();
        }

        private void trianglePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left))
                return;
            Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);
            //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
            if (MathUtilities.PointInTriangle(p, trianglePoint1, trianglePoint2, trianglePoint3))
            {
                //Debug.WriteLine("Inside!");
                triangleDotPoint = p;
            }
            else {
                //Debug.WriteLine("Outside :(");
                if (p.X <= trianglePoint2.X && p.Y <= trianglePoint1.Y) //top left corner
                {
                    //if(MathUtilities.LineSide(p1, p2, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint1, trianglePoint2, out triangleDotPoint);
                }
                else if (p.X > trianglePoint2.X && p.Y <= trianglePoint1.Y)
                { //top right corner
                    //if(MathUtilities.LineSide(p2, p3, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint2, trianglePoint3, out triangleDotPoint);
                }
                else if (p.Y > trianglePoint1.Y) //below p1 && p3
                {
                    //if(MathUtilities.LineSide(p3, p1, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint3, trianglePoint1, out triangleDotPoint);
                }
            }
            CalculateTrianglePercentages();
            DrawTriangle();
        }

        private void trianglePanel_Click(object sender, EventArgs e)
        {
            if (!((MouseEventArgs)e).Button.Equals(MouseButtons.Left))
                return;
            Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);
            //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
            if (MathUtilities.PointInTriangle(p, trianglePoint1, trianglePoint2, trianglePoint3))
            {
                //Debug.WriteLine("Inside!");
                triangleDotPoint = p;
            }
            else {
                //Debug.WriteLine("Outside :(");
                if (p.X <= trianglePoint2.X && p.Y <= trianglePoint1.Y) //top left corner
                {
                    //if(MathUtilities.LineSide(p1, p2, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint1, trianglePoint2, out triangleDotPoint);
                }
                else if (p.X > trianglePoint2.X && p.Y <= trianglePoint1.Y)
                { //top right corner
                    //if(MathUtilities.LineSide(p2, p3, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint2, trianglePoint3, out triangleDotPoint);
                }
                else if (p.Y > trianglePoint1.Y) //below p1 && p3
                {
                    //if(MathUtilities.LineSide(p3, p1, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, trianglePoint3, trianglePoint1, out triangleDotPoint);
                }
            }
            CalculateTrianglePercentages();
            DrawTriangle();
        }

        #endregion

        private void gridPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void objectiveSlider_ValueChanged(object sender, Winamp.Components.WinampTrackBarValueChangedEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        Bitmap prevDraw;

        private void sliderGroupLabel_Click(object sender, EventArgs e)
        {

        }

        private void ReDraw()
        {
            ////Logger.AppendText("ReDraw called!");
            //if (APIClass.CurrentMap == null)
            //{
            //    Debug.WriteLine("Current map null");
            //    return;
            //}

            //try
            //{

            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            
            cellWidth = gridPanel.Width / APIClass.CurrentMap.Width;
            cellHeight = gridPanel.Height / APIClass.CurrentMap.Height;            
            _cellPanelGraphics.Clear(_emptyCellColor);

            //Draw grid
            using (Pen p = new Pen(_groundTile, 1))
            {
                for (int i = 0; i < currentMap.Height + 1; i++)
                    _cellPanelGraphics.DrawLine(p, new Point(0, i * cellHeight), new Point(currentMap.Width * cellWidth, i * cellHeight));
                for (int j = 0; j < currentMap.Height + 1; j++)
                    _cellPanelGraphics.DrawLine(p, new Point(j * cellWidth, 0), new Point(j * cellWidth, currentMap.Height * cellHeight));
            }



            try
            {
                //foreach (var c in _cellsToDraw)
                for (int k = 0; k < currentMap.Width * currentMap.Height; k++)
                    //for (int i = 0; i<currentMap.Height; i++)
                    //for(int j = 0; j<currentMap.Width; j++)
                {

                    //int k = i * currentMap.Height + j;

                    var c = _cellsToDraw[k];
                    c.SelectedToDraw = false;

                    // Drawing user selection
                    if (_hideSuggestionWhenSelecting)
                    {
                        c = currentMap.Cells.Find(e => (e.X == c.X && e.Y == c.Y));
                        c.SelectedToDraw = false;

                    }

                    if (_userSelectedPoints.Count > 0)
                    { //if there is a selection
                        var tmpCIndex = _userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y));
                        if (tmpCIndex != -1) //if a cell is not selected
                        {
                            c = _cellsToDraw[k];
                            c.SelectedToDraw = true;
                        }
                    }

                    //var c = currentMap.Cells[k];
                    //c.SelectedToDraw = false;

                    //if (_hideSuggestionWhenSelecting)
                    //{
                    //    if (_userSelectedPoints.Count > 0)
                    //    { //if there is a selection
                    //        var tmpCIndex = _userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y));
                    //        if (tmpCIndex != -1) //if a cell is not selected
                    //        {
                    //            c = _cellsToDraw[k];
                    //            c.SelectedToDraw = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}
                    //Debug.WriteLine(c.X+","+c.Y);

                    switch (_cellHighlightSettings)
                    {
                        case CellHighlight.None:
                            //if ((APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && !(c.IsWalkable))
                            //    if (!c.SelectedToDraw)
                            //    {
                            //        using (HatchBrush b = new HatchBrush(HatchStyle.Percent70, _emptyCellColor, _groundTileColorSelected)) //passa a ser nao walkable (mais claro?)
                            //            FillCell(c.X, c.Y, b);
                            //    }
                            //    else {
                            //    }
                            //else if (!(APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && c.IsWalkable)
                            //    if (c.SelectedToDraw)
                            //    {
                            //        FillCell(c.X, c.Y, _groundTileColorSelected);
                            //    }
                            //    else {
                            //        using (HatchBrush b = new HatchBrush(HatchStyle.Percent80, _groundTileColorSelected, _emptyCellColor)) // passa a ser walkable (mais escuro)
                            //            FillCell(c.X, c.Y, b);
                            //    }
                            /*else */
                            if (c.IsWalkable)
                            //{
                                FillCell(c.X, c.Y, _groundTileColorSelected);
                            //}
                            break;

                        case CellHighlight.All:
                                if ((APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && !(c.IsWalkable))
                                {
                                if (c.SelectedToDraw)
                                    FillCell(c.X, c.Y, _removedCellColorSelected);
                                else
                                    FillCell(c.X, c.Y, _removedCellColorUnselected);
                            }
                                else if (!(APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && c.IsWalkable) {
                                if (c.SelectedToDraw)
                                    FillCell(c.X, c.Y, _addedCellColorSelected);
                                else
                                    FillCell(c.X, c.Y, _addedCellColorUnselected);
                            }
                                else if (c.IsWalkable)
                                    FillCell(c.X, c.Y, _groundTileColorSelected);
                            break;

                        case CellHighlight.Added:
                                if (!(APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && c.IsWalkable)
                                {
                                    if (c.SelectedToDraw)
                                        FillCell(c.X, c.Y, _addedCellColorSelected);
                                    else
                                        FillCell(c.X, c.Y, _addedCellColorUnselected);
                                }
                                else if (c.IsWalkable)
                                    FillCell(c.X, c.Y, _groundTileColorSelected);
                            break;

                        case CellHighlight.Removed:
                                if ((APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && !(c.IsWalkable))
                                {
                                    if (c.SelectedToDraw)
                                        FillCell(c.X, c.Y, _removedCellColorSelected);
                                    else
                                        FillCell(c.X, c.Y, _removedCellColorUnselected);
                                }
                                else if (c.IsWalkable)
                                    FillCell(c.X, c.Y, _groundTileColorSelected);
                            break;

                        default:
                            if (c.IsWalkable)
                                FillCell(c.X, c.Y, _groundTileColorSelected);
                            break;
                    }
                }

                //draw user selected cells
                if (_userSelectedPoints.Count > 0)
                    //DrawUserSelectedCells();
                    DrawUserSelection();
                if (_lockedCellList.Count > 0)
                    DrawUserLockedCells();

                //generate map objectfrom chromosome and get start and end points here to draw
                //if (solutionChromosomeMap != null)
                //{
                //    DrawStartEndPoints(solutionChromosomeMap.StartPoint, solutionChromosomeMap.EndPointList);
                //}
                //else
                //{
                    DrawStartEndPoints(APIClass.CurrentMap.StartPoint, APIClass.CurrentMap.EndPointList);
                //}

                gridPanel.BackgroundImage = _cellBitmap;
                prevDraw = (Bitmap) _cellBitmap.Clone();

                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { gridPanel.Refresh(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    gridPanel.Refresh();
                }
            }
            catch (Exception exc)
            {
                DebugUtilities.DebugException(exc);
            }
        }
        
        private void DrawSelectionPreviewRectangle()
        {
            if (currentMap == null)
                return;
            try
            {
                using (Bitmap b = (Bitmap)prevDraw.Clone())
                using (Graphics g = Graphics.FromImage(b))
                {
                    //User selection preview rect
                    if (SelectionRect != null && SelectionRect.Width > 0 && SelectionRect.Height > 0)
                    {
                        if (selectionAdd)
                            g.FillRectangle(selectionBrushAdd, SelectionRect);
                        else
                            g.FillRectangle(selectionBrushRemove, SelectionRect);
                        gridPanel.BackgroundImage = b;
                        gridPanel.Refresh();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public enum BorderSide
        {
            Top,
            Left,
            Bottom,
            Right
        }

        private void DrawBorder(int x, int y, Pen p, BorderSide s)
        {
            switch (s)
            {
                case BorderSide.Top:
                    _cellPanelGraphics.DrawLine(p, new Point(x*cellWidth, y*cellHeight), new Point(x*cellWidth + cellWidth, y*cellHeight));
                    break;
                case BorderSide.Left:
                    _cellPanelGraphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight), new Point(x * cellWidth, y*cellHeight + cellHeight));
                    break;
                case BorderSide.Bottom:
                    _cellPanelGraphics.DrawLine(p, new Point(x*cellWidth, y*cellHeight + cellHeight), new Point(x*cellWidth + cellWidth, y*cellHeight + cellHeight));
                    break;
                case BorderSide.Right:
                    _cellPanelGraphics.DrawLine(p, new Point(x*cellWidth + cellWidth, y*cellHeight), new Point(x*cellWidth + cellWidth, y*cellHeight + cellHeight));
                    break;
                default:
                    break;
            }            
        }

        private void DrawUserSelection()
        {
            using (Pen pen = new Pen(new SolidBrush(Color.FromArgb(0, 170, 0)), 3))
            {
                foreach(var p in _userSelectedPoints)
                {
                    if (p.X > 0)//test left
                    {
                        var indexLeft = _userSelectedPoints.FindIndex(point => (point.X == (p.X - 1) && point.Y == p.Y));
                        if (indexLeft == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Left);
                    }
                    else {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Left);
                    }

                    if (p.X < currentMap.Width - 1) //test right
                    {
                        var indexRight = _userSelectedPoints.FindIndex(point => (point.X == (p.X + 1) && point.Y == p.Y));
                        if (indexRight == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Right);
                    }
                    else {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Right);
                    }
                    if (p.Y > 0) //test up
                    {
                        var indexUp = _userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y - 1)));
                        if (indexUp == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Top);
                    }
                    else {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Top);
                    }
                    if (p.Y < currentMap.Height - 1) //test bottom
                    {
                        var indexBot = _userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y + 1)));
                        if (indexBot == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Bottom);
                    }
                    else {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Bottom);
                    }
                }
            }
        }

        private void DrawUserSelectedCells()
        {
            Pen pen = new Pen(new SolidBrush(Color.FromArgb(0, 170, 0)), 3);
            foreach (var p in _userSelectedPoints)
            {
                _cellPanelGraphics.DrawRectangle(pen, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
            }

        }

        private void DrawUserLockedCells()
        {
            Pen pen = new Pen(new SolidBrush(Color.FromArgb(170, 0, 0)), 3);
            foreach (var p in _lockedCellList)
            {
                _cellPanelGraphics.DrawRectangle(pen, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
            }

        }

        private void FillCell(int x, int y, HatchBrush b)
        {
            //SolidBrush b = new SolidBrush(c);
            Pen p = new Pen(new SolidBrush(_cellBorderColor), 1);
            //_g.FillRectangle(_groundTile, x * cellWidth + 1, y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            _cellPanelGraphics.FillRectangle(b, x * cellWidth + 1, y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            _cellPanelGraphics.DrawRectangle(p, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
        }

        private void FillCell(int x, int y, Color c)
        {
            SolidBrush b = new SolidBrush(c);
            Pen p = new Pen(new SolidBrush(_cellBorderColor), 1);
            //_g.FillRectangle(_groundTile, x * cellWidth + 1, y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            _cellPanelGraphics.FillRectangle(b, x * cellWidth + 1, y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            _cellPanelGraphics.DrawRectangle(p, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
        }

        private void DrawStartEndPoints(StartingPoint start, List<EndingPoint> endPoints)
        {
            if (start != null)
                _cellPanelGraphics.DrawString("S", new Font("ArialBold", 12), new SolidBrush(Color.Yellow), new RectangleF(new PointF(start.x * cellWidth, start.y * cellHeight), new SizeF(cellWidth, cellHeight)), StringFormat.GenericDefault);
            if (endPoints != null)
                foreach (var e in endPoints)
                    _cellPanelGraphics.DrawString("E", new Font("ArialBold", 12), new SolidBrush(Color.Yellow), new RectangleF(new PointF(e.x * cellWidth, e.y * cellHeight), new SizeF(cellWidth, cellHeight)), StringFormat.GenericDefault);
        }



        #endregion


        #region ALGORITHM
        

        void AlgorithmRunCompleteCallback(List<Gene> solution)
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");
            if (!_algorithmsInitialized)
            {
                _algorithmsInitialized = true; //ignore the initialization solution
                return;
            }

            solutionChromosomeMap = APIClass.MapObjectFromChromosome(new Chromosome(solution)); //create map from chromosome. should pass genes?
            solutionHistory.Add(solutionChromosomeMap.CloneJson());
            _currentSolutionIndex = solutionHistory.Count - 1;
            _hitmeCount = 2;

            _cellsToDraw = solutionChromosomeMap.Cells;

            if (gridPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
            }
            else
            {
                ReDraw();
            }

            _justReset = false;

        }


        private bool _justReset = true;
        private void ResetAlgorithm()
        {
            if (_justReset)
                return;

            if (currentMap == null)
                return;
            _algorithmsInitialized = false;

            //_objAlgTest = new ObjectiveAlgorithmTestClass();
            //_novAlgTest = new NoveltyAlgorithmTestClass();
            _objAlgTest.ResetOnNextRun = true;
            _novAlgTest.ResetOnNextRun = true;

            solutionChromosomeMap = null;
            _cellsToDraw = APIClass.CurrentMap.Cells;
            ReDraw();

            RunAlgorithm();
            _justReset = true;
        }

        private void ParseMapAndRunAlgorithm()
        {
            if (objRunning || novRunning)
                return;

            try
            {
                currentMap = APIClass.ParseMapFile();

                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    ReDraw();
                }

                var dif = APIClass.CalculateDifference(previousMap, currentMap);

                if (dif > 0.0 && dif < 500)
                {
                    //Logger.AppendText("Dif:" + dif.ToString());
                    if (currentMap.EndPointList == null)
                        Logger.AppendText("WARNING: No ending points detected.");

                    if (currentMap.StartPoint == null)
                        Logger.AppendText("WARNING: No start point detected.");
                    RunAlgorithm();
                }
                previousMap = currentMap;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void SendSaveCommandAndReloadMap()
        {
            if (DirectoryManager.ProjectDir == null || currentMap == null)
                return;

            Debug.WriteLine("Wrote solution");
            //solutionChromosomeMap = null;
            _actHook.SendReloadCommand();
            currentMap = previousMap = APIClass.ParseMapFile();
            //_cellsToDraw = currentMap.Cells;
            ////previousMap = currentMap;
            if (gridPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
            }
            else
            {
                ReDraw();
            }
            //actHook.restoreWindow();
            APIClass._mapSaved = false;
        }

        private void RunAlgorithm()
        {
            if (objRunning || novRunning)
                return;

            if(_userSketchInfluencePercent == 0 && _objectiveInfluencePercent == 0 && _noveltyInfluencePercent == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            if(!keepPopCheckBox.Checked)
            {
                _objAlgTest.ResetOnNextRun = true;
                _novAlgTest.ResetOnNextRun = true;
            }

            //if (PreviousObjectivePopulation != null)
            //    _objAlgTest = new ObjectiveAlgorithmTestClass(PreviousObjectivePopulation);

            //if (PreviousNoveltyPopulation != null)
            //    _novAlgTest = new NoveltyAlgorithmTestClass(PreviousNoveltyPopulation);

            AlgorithmRunComplete d = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            //OBJECTIVE SETUP
            _objAlgTest.InitialPopulationSize = (int)ObjPopInput.Value;
            _objAlgTest.GenerationLimit = (int)ObjGenInput.Value;
            _objAlgTest.PercentUserSketchInfluence = _userSketchInfluencePercent;
            if (RandomPopCarryOverCheckBox.Checked)
                _objAlgTest.NextPopulationCarryMethod = PopulationCarryMethod.Random;
            else
                _objAlgTest.NextPopulationCarryMethod = PopulationCarryMethod.TopPercent;
            _objAlgTest.PercentNoveltyChromosomesToRecieve = _noveltyInfluencePercent;
            _objAlgTest.PercentObjectiveChromosomesToKeep = _objectiveInfluencePercent;
            _objAlgTest.CrossoverTypeSelected = CurrentCrossoverSelection;
            _objAlgTest.PercentMutation = (double)ObjMutInput.Value/100.0;
            _objAlgTest.PercentElitism = (double)ObjEliInput.Value/100.0;
            _objAlgTest.CrossoverTypeSelected = CurrentCrossoverSelection;
            _objAlgTest.UserSelectionPositiveFocus = _userSelectedPoints;
            _objAlgTest.CurrentObjective = _currObjective;

            //NoveltyAlgorithmTestClass.OnNoveltyAlgorithmFinished += ObjectiveAlgorithmTestClass.NoveltyAlgorithm_Finished;

            _objAlgTest.NewObjectiveRun(currentMap, d); //setup


            //NOVELTY SETUP
            _novAlgTest.InitialPopulationSize = (int)NovPopInput.Value;
            _novAlgTest.GenerationLimit = (int)NovGenInput.Value;
            _novAlgTest.PercentChromosomesToInject = _noveltyInfluencePercent;
            //ObjectiveAlgorithmTestClass.OnGenerationTerminated += NoveltyAlgorithmTestClass.ObjectiveAlgorithm_GenerationEnd;
            //ObjectiveAlgorithmTestClass.OnObjectiveAlgorithmComplete += NoveltyAlgorithmTestClass.ObjectiveAlgorithm_Finished;
            _novAlgTest.PercentMutation = (double)NovMutInput.Value / 100.0;
            _novAlgTest.PercentElitism = (double)NovEliInput.Value / 100.0;
            _novAlgTest.CrossoverTypeSelected = CurrentCrossoverSelection;
            _novAlgTest.UserSelectionPositiveFocus = _userSelectedPoints;

            _novAlgTest.NewNoveltyRun(currentMap, d); // setup


            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                objRunning = true;
                _objAlgTest.Run();
                objRunning = false;
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                novRunning = true;
                _novAlgTest.Run();
                novRunning = false;
            }));


        }

        
        #endregion


        #region FILEWATCHER



        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void InitWatcher()
        {
            try
            {
                fsw = new FileSystemWatcher(DirectoryManager.ScriptsDir, "dungeon.lua");

                /* Watch for changes in LastAccess and LastWrite times, and
                the renaming of files or directories. */
                //fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                //   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                fsw.NotifyFilter = NotifyFilters.LastWrite;

                //add event handlers
                fsw.Changed += new FileSystemEventHandler(fsw_Changed);

                // Begin watching.
                fsw.EnableRaisingEvents = true;

                //LogWrite("Watcher started for: " + dungeonFilePath +"\n");
                Logger.AppendText("Watcher started for: " + DirectoryManager.DungeonFilePath + "\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        int count = 0;
        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            count++;
            if (count < 2) return; //needed because watcher fires twice

            //Debug.WriteLine("FILE CHANGE CARALHO");

            //If user decides to apply algorithm solution, press button
            if (APIClass._mapSaved)
            {
                SendSaveCommandAndReloadMap();
            }
            else
            {
                Debug.WriteLine("File save detected");
                if (autoSuggestions)
                    ParseMapAndRunAlgorithm();
            }
            count = 0;
        }

        #endregion



    }

    
    public class ComboItem
    {
        public string Key { get; set; }
        public Objective Value { get; set; }

        public ComboItem(string k, Objective v)
        {
            Key = k; Value = v;
        }
    }

}
