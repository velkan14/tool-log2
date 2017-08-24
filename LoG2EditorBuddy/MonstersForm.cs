using gma.System.Windows;
using Povoater.Exceptions;
using Povoater.Layers;
using Povoater.LoG2API;
using Povoater.Utilities;
using Povoater.WinAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Povoater
{
    public partial class Monsters : Form
    {
        private Core core;
        public UserSelection USelection { get; private set; }
        public AreaManager AreasManager { get; private set; }
        Image lastImage;

        NotifyIcon notifyIcon;

        private bool AdvanceMode { get { return toggleSwitch_advanceMode.Checked; } }

        private int InnovationPercentage {
            get {
                if (AdvanceMode)
                {
                    return trackBar_innovation.Value;
                }
                else
                {
                    if (toggleSwitch_innovation.Checked)
                    {
                        return 50;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        private int GuidelinePercentage {
            get
            {
                if (AdvanceMode)
                {
                    return trackBar_objective.Value;
                }
                else
                {
                    if (toggleSwitch_objective.Checked)
                    {
                        return 100;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        private int UserPercentage {
            get
            {
                if (AdvanceMode)
                {
                    return trackBar_userplacement.Value;
                }
                else
                {
                    if (toggleSwitch_userplacement.Checked)
                    {
                        return 25;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        private int NumberMonsters { get { return Convert.ToInt32(numericUpDown_maxmonsters.Value); } }
        private int NumberItens { get { return Convert.ToInt32(numericUpDown_numberItens.Value); } }
        private int HordesPercentage { get { return trackBar_hordes.Value; } }
        
        public bool Redraw { get; set; }
        public bool RedrawMap { get; set; }

        public Monsters()
        {
            InitializeComponent();
        }

        private void Monsters_Load(object sender, EventArgs e)
        {
            core = new Core(this, InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            notifyIcon.Visible = true;

            //Solves flickering when redrawing gridPanel and triangle Panel
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, gridPanel, new object[] { true });

            Redraw = false;

            Logger.EntryWritten += Logger_EntryWritten;

            difficultyDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(RoomDifficulty));
            itemAccessibilityDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ItemAccessibility));

            toggleSwitch1_CheckedChanged(null, null);
        }

        public void MapLoaded()
        {
            USelection = new UserSelection(this, core, gridPanel);

            if(AreasManager == null) AreasManager = new AreaManager(core.OriginalMap, gridPanel);

            Invoke((MethodInvoker)(() =>
            {
                areaBindingSource.Clear();
                foreach (Area a in AreasManager.AreaList)
                {
                    areaBindingSource.Add(a);
                }

                groupBox_selection.Enabled = true;

                button_previous.Enabled = true;
                button_next.Enabled = true;
                button_newSuggestion.Enabled = true;
                button_select.Enabled = true;
                button_export.Enabled = true;

                toggleSwitch_view.Enabled = true;
                trackBar_history.Enabled = true;

                gridPanel.Enabled = true;
            }));
        }

        /*********************************************************************************/
        /******************--------------SELECTION---------------*************************/
        /*********************************************************************************/

        #region Selection

        private void button_clear_Click(object sender, EventArgs e)
        {
            USelection.ClearSelection();
            ReDraw();
        }

        private void button_invert_Click(object sender, EventArgs e)
        {
            if (USelection.InvertSelection())
            {
                ReDraw();
            }
            else
            {
                Logger.AppendText("No selection to invert.");
            }
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            if (!USelection.Attached)
            {
                USelection.Attach();
                button_select.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                USelection.Dettach();

                button_select.FlatStyle = FlatStyle.Standard;
                ReDraw();
            }

        }

        #endregion
        /*********************************************************************************/


        void Logger_EntryWritten(object sender, LogEntryEventArgs args)
        {
            if (textBox_logger.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { textBox_logger.AppendText(args.Message); textBox_logger.AppendText(Environment.NewLine); }));
            }
            else
            {
                textBox_logger.AppendText(args.Message);
                textBox_logger.AppendText(Environment.NewLine);
            }
        }



        private void selectProjectDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDirectory();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            List<Point> selectPoints = USelection.GetSelectedPoints();

            if (selectPoints.Count == 0)
            {
                Logger.AppendText("No selection, exporting entire solution.");
                APIClass.SaveMapFile(core.CurrentMap);
                core.ReloadLOG();
            }
            else
            {
                Logger.AppendText("Exporting solution.");
                APIClass.ExportSelection(core.CurrentMap, core.OriginalMap, selectPoints);
                core.ReloadLOG();
            }
        }




        private void button_next_Click(object sender, EventArgs e)
        {
            core.NextMap();
            UpdateTrackHistory();
            ReDrawMap();
        }

        private void button_previous_Click(object sender, EventArgs e)
        {
            core.PreviousMap();
            UpdateTrackHistory();
            ReDrawMap();
        }


        private void gridPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int durationMilliseconds = 10000;
            string tooltip = "";
            if (!toggleSwitch_view.Checked) tooltip = core.OriginalMap.getToolTipInfo(e.X, e.Y, gridPanel.Width, gridPanel.Height);
            else tooltip = core.CurrentMap.getToolTipInfo(e.X, e.Y, gridPanel.Width, gridPanel.Height);
            toolTip_panel.Show(tooltip, gridPanel, e.X, e.Y, durationMilliseconds);
        }

        private void button_newRun_Click(object sender, EventArgs e)
        {
            ResetProgress();
            core.NewSuggestion(InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);
        }

        public void UpdateTrackHistory()
        {
            Invoke((MethodInvoker)(() =>
            {
                trackBar_history.Maximum = core.CountSuggestions - 1;
                trackBar_history.Value = core.IndexMap;

                if (!core.HasNextMap()) button_next.Enabled = false;
                else button_next.Enabled = true;
                if (!core.HasPreviousMap()) button_previous.Enabled = false;
                else button_previous.Enabled = true;
            }));
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Selected)
                {
                    AreasManager.SetSelected((string)row.Cells[0].Value);
                }
                else
                {
                    AreasManager.SetUnselected((string)row.Cells[0].Value);
                }
            }
            if (dataGridView.CurrentRow != null) AreasManager.SetSelected((string)dataGridView.CurrentRow.Cells[0].Value);
            if (lastImage != null) ReDraw();
        }

        private void dataGridView_CurrentCellDirtyStateChanged(object sender,
            EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (lastImage != null) ReDraw();
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
        }

        public void Draw()
        {
            if (RedrawMap)
            {
                if(!toggleSwitch_view.Checked) lastImage = core.OriginalMap.Draw(gridPanel.Width, gridPanel.Height);
                else lastImage = core.CurrentMap.Draw(gridPanel.Width, gridPanel.Height);

                RedrawMap = false;
            }

                Image image = lastImage.Clone() as Image;

            if (USelection != null && USelection.Attached) image = USelection.Draw(gridPanel.Width, gridPanel.Height, image);

            image = AreasManager.Draw(gridPanel.Width, gridPanel.Height, image);

            gridPanel.BackgroundImage = image;

            gridPanel.Refresh();
        }

        public void ReDraw()
        {
            Redraw = true;
        }

        public void ReDrawMap()
        {
            Redraw = true;
            RedrawMap = true;
        }

        int conP = 0;
        int innoP = 0;
        int guideP = 0;
        int mixP = 0;
        public void Progress(int n, int value)
        {

            Invoke((MethodInvoker)(() =>
            {
                switch (n)
                {
                    case 0:
                        {
                            conP = value;
                            break;
                        }
                    case 1:
                        {
                            innoP = value;
                            break;
                        }
                    case 2:
                        {
                            guideP = value;
                            break;
                        }
                    case 3:
                        {
                            mixP = value;
                            break;
                        }
                }
                progressBar1.Value = Convert.ToInt32(Math.Min(100, (1.0 / 4.0) * conP + (1.0 / 4.0) * innoP + (1.0 / 4.0) * guideP + (1.0 / 4.0) * mixP));
                /*if (conP + innoP + guideP + mixP > 400)
                {
                    System.Timers.Timer t = new System.Timers.Timer(10000);
                    t.Elapsed += new ElapsedEventHandler(Reset);
                    t.Enabled = true;
                }*/
            }));
        }

        internal void NotifyUser()
        {
            if (this.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => {
                    notifyIcon.BalloonTipTitle = "Povoater";
                    notifyIcon.BalloonTipText = "Hey! I just got a suggestion for you to check! If you don't like it, you can try to configure me.";
                    notifyIcon.ShowBalloonTip(30000);
                })); //needed when calling the callback from a different thread
            }
            else
            {
                notifyIcon.BalloonTipTitle = "Povoater";
                notifyIcon.BalloonTipText = "Hey! I just got a suggestion for you to check!";
                notifyIcon.ShowBalloonTip(30000);
            }
            
        }

        private void Reset(object sender, ElapsedEventArgs e)
        {
            ResetProgress();
        }

        private void ResetProgress()
        {
            conP = 0;
            innoP = 0;
            guideP = 0;
            mixP = 0;
            progressBar1.Value = 0;
        }

        private void SnapTick_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tmp = (TrackBar)sender;
            if (tmp.Value < 25)
                tmp.Value = 0;
            else if (tmp.Value >= 25 && tmp.Value < 75)
                tmp.Value = 50;
            else if(tmp.Value >= 75)
                tmp.Value = 100;
        }

        private void button_undo_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Redraw)
            {
                if (this.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { Draw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    Draw();
                }
            }
            Redraw = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDirectory();
        }

        private void LoadDirectory()
        {
            var fd = new FolderBrowserDialog();
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (core.LoadDirectory(fd.SelectedPath))
                {
                    button_select_project.Enabled = false;
                }
                else
                {
                    Logger.AppendText(@"Directory is not valid. Please pick a project directory containing a "".dungeon_editor"" file");
                }

            }
            else Logger.AppendText("Project directory not picked, please pick a LoG2 Project Directory");
        }

        private void toggleSwitch_view_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch_view.Enabled)
            {
                ReDrawMap();
            }
        }

        private void toggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch_advanceMode.Checked)
            {
                trackBar_userplacement.Visible = true;
                trackBar_objective.Visible = true;
                trackBar_innovation.Visible = true;
                toggleSwitch_userplacement.Visible = false;
                toggleSwitch_objective.Visible = false;
                toggleSwitch_innovation.Visible = false;
                label_on1.Visible = true;
                label_on2.Visible = true;
                label_on3.Visible = true;
                label_off1.Visible = true;
                label_off2.Visible = true;
                label_off3.Visible = true;
            }
            else
            {
                trackBar_userplacement.Visible = false;
                trackBar_objective.Visible = false;
                trackBar_innovation.Visible = false;
                toggleSwitch_userplacement.Visible = true;
                toggleSwitch_objective.Visible = true;
                toggleSwitch_innovation.Visible = true;
                label_on1.Visible = false;
                label_on2.Visible = false;
                label_on3.Visible = false;
                label_off1.Visible = false;
                label_off2.Visible = false;
                label_off3.Visible = false;
            }
        }

        private void updateCore(object sender, EventArgs e)
        {
            core.InnovationPercentage = InnovationPercentage;
            core.GuidelinePercentage = GuidelinePercentage;
            core.UserPercentage = UserPercentage;

            core.NumberItens = NumberItens;
            core.NumberMonsters = NumberMonsters;
            core.HordesPercentage = HordesPercentage;

        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(StringResources.CreditsHelpString, "Credits");
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            core.ResetTimer();
        }

        private void ShowPovoater(object sender, EventArgs e)
        {
            Console.WriteLine("Show");
            core.ShowPovoater();
        }
    }
}
