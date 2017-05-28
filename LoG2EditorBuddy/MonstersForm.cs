﻿using gma.System.Windows;
using EditorBuddyMonster.Exceptions;
using EditorBuddyMonster.Layers;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using EditorBuddyMonster.WinAPI;
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

namespace EditorBuddyMonster
{
    public partial class Monsters : Form
    {
        private Draw drawer;
        private Core core;
        private UserSelection userSelection;
        public AreaManager AreasManager { get; private set; }
        Image lastImage;


        private int InnovationPercentage { get { return trackBar_innovation.Value; } }
        private int GuidelinePercentage { get { return trackBar_objective.Value; } }
        private int UserPercentage { get { return trackBar_userplacement.Value; } }
        private int NumberMonsters { get { return Convert.ToInt32(numericUpDown_maxmonsters.Value); } }
        private int NumberItens { get { return Convert.ToInt32(numericUpDown_numberItens.Value); } }
        private int HordesPercentage { get { return trackBar_hordes.Value; } }

    public Monsters()
        {
            InitializeComponent();
        }

        private void Monsters_Load(object sender, EventArgs e)
        {
            core = new Core(this, InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);



            //Solves flickering when redrawing gridPanel and triangle Panel
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, gridPanel, new object[] { true });


            drawer = new Draw(gridPanel);

            Logger.EntryWritten += Logger_EntryWritten;

            difficultyDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(RoomDifficulty));
            itemAccessibilityDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ItemAccessibility));
        }

        public void MapLoaded()
        {
            userSelection = new UserSelection(this, core, gridPanel);

            AreasManager = new AreaManager(core.OriginalMap, gridPanel);

            

            Invoke((MethodInvoker)(() =>
            {
                areaBindingSource.Clear();
                foreach(Area a in AreasManager.AreaList)
                {
                    areaBindingSource.Add(a);
                }

                groupBox_selection.Enabled = true;

                button_undo.Enabled = true;
                button_previous.Enabled = true;
                button_next.Enabled = true;
                button_newSuggestion.Enabled = true;
                button_select.Enabled = true;

                trackBar_history.Enabled = true;
            }));
        }

        /*********************************************************************************/
        /******************--------------SELECTION---------------*************************/
        /*********************************************************************************/

        #region Selection

        private void button_clear_Click(object sender, EventArgs e)
        {
            userSelection.ClearSelection();
            ReDraw();
        }

        private void button_invert_Click(object sender, EventArgs e)
        {
            if (userSelection.InvertSelection())
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
            if (!userSelection.Attached)
            {
                userSelection.Attach();
            }
            else
            {
                userSelection.Dettach();
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
            var fd = new FolderBrowserDialog();
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (!core.LoadDirectory(fd.SelectedPath))
                {
                    Logger.AppendText(@"Directory is not valid. Please pick a project directory containing a "".dungeon_editor"" file");
                }

            }
            else Logger.AppendText("Project directory not picked, please pick a LoG2 Project Directory");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            List<Point> selectPoints = userSelection.GetSelectedPoints();

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
            toolTip_panel.Show(core.CurrentMap.getToolTipInfo(e.X, e.Y, gridPanel.Width, gridPanel.Height), gridPanel, e.X, e.Y, durationMilliseconds);
        }

        private void button_newRun_Click(object sender, EventArgs e)
        {
            core.NewSuggestion(InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);
        }

        public void UpdateTrackHistory()
        {
            Invoke((MethodInvoker)(() =>
            {
                trackBar_history.Maximum = core.CountSuggestions - 1;
                trackBar_history.Value = core.IndexMap;
            }));
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Selected)
                {
                    AreasManager.SetSelected((string)row.Cells[0].Value);
                } else{
                    AreasManager.SetUnselected((string)row.Cells[0].Value);
                }
            }
            if(dataGridView.CurrentRow != null) AreasManager.SetSelected((string)dataGridView.CurrentRow.Cells[0].Value);
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
            progressBar1.Increment(100);
        }

        public void ReDraw()
        {
            Image image = lastImage.Clone() as Image;

            if (userSelection != null && userSelection.Attached) image = userSelection.Draw(gridPanel.Width, gridPanel.Height, image);

            image = AreasManager.Draw(gridPanel.Width, gridPanel.Height, image);

            gridPanel.BackgroundImage = image;

            if (gridPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { gridPanel.Refresh(); })); //needed when calling the callback from a different thread
            }
            else
            {
                gridPanel.Refresh();
            }
        }

        public void ReDrawMap()
        {
            lastImage = core.CurrentMap.Draw(gridPanel.Width, gridPanel.Height);
            ReDraw();
        }

        int conP = 0;
        int innoP = 0;
        int guideP = 0;
        int mixP = 0;
        public void Progress(int n, int value)
        {

            Invoke((MethodInvoker)(() => {
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
                if (conP + innoP + guideP + mixP > 400)
                {
                    System.Timers.Timer t = new System.Timers.Timer(10000);
                    t.Elapsed += new ElapsedEventHandler(Reset);
                    t.Enabled = true;
                }
            }));
        }

        private void Reset(object sender, ElapsedEventArgs e)
        {
            ResetProgress();
        }
        public void ResetProgress()
        {

            Invoke((MethodInvoker)(() => {
                conP = 0;
                innoP = 0;
                guideP = 0;
                mixP = 0;
                progressBar1.Value = 0;
            }));
        }
        
    }
}
