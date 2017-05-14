using gma.System.Windows;
using Log2CyclePrototype.Exceptions;
using Log2CyclePrototype.Layers;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using Log2CyclePrototype.WinAPI;
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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Log2CyclePrototype
{
    public partial class Monsters : Form
    {
        private Settings settingsForm;

        private Draw drawer;
        private Core core;
        private UserSelection userSelection;
        Layer layerDifficulty, layerItens, layerMonsters, layerResources;
        public AreaManager AreasManager { get; private set; }
        Image lastImage;

        public Monsters()
        {
            InitializeComponent();
        }

        private void Monsters_Load(object sender, EventArgs e)
        {
            core = new Core(this);



            //Solves flickering when redrawing gridPanel and triangle Panel
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, gridPanel, new object[] { true });


            drawer = new Draw(gridPanel);

            Logger.EntryWritten += Logger_EntryWritten;

            difficultyDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(Difficulty));

            settingsForm = new Settings(core);
            initializeParameters(); //FIXME: Talvez não seja a forma correcta de o fazer
        }

        public void MapLoaded()
        {
            userSelection = new UserSelection(this, core, gridPanel);

            layerDifficulty = new Layer(this, core.OriginalMap, gridPanel, panel_palett_difficulty, Color.Red);
            layerItens = new Layer(this, core.OriginalMap, gridPanel, panel_palette_itens, Color.ForestGreen);
            layerMonsters = new Layer(this, core.OriginalMap, gridPanel, panel_palette_monsters, Color.IndianRed);
            layerResources = new Layer(this, core.OriginalMap, gridPanel, panel_palette_resources, Color.Yellow);

            AreasManager = new AreaManager(core.OriginalMap, gridPanel);

            

            Invoke((MethodInvoker)(() =>
            {
                foreach(Area a in AreasManager.AreaList)
                {
                    areaBindingSource.Add(a);
                }

                groupBox_layer_difficulty.Enabled = true;
                groupBox_layer_itens.Enabled = true;
                groupBox_layer_monsters.Enabled = true;
                groupBox_layer_resources.Enabled = true;
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
                layerDifficulty.Dettach();
                layerItens.Dettach();
                layerMonsters.Dettach();
                layerResources.Dettach();
            }
            else
            {
                userSelection.Dettach();
                ReDraw();
            }

        }

        #endregion
        /*********************************************************************************/

        /*********************************************************************************/
        /******************--------------PARAMATERS---------------************************/
        /*********************************************************************************/

        #region Parameters

        private void initializeParameters()
        {
            trackBar_innovation_Scroll(null, null);
            trackBar_userplacement_Scroll(null, null);
            trackBar_objective_Scroll(null, null);
            trackBar_hordes_Scroll(null, null);
            trackBar_mapobjects_Scroll(null, null);
            numericUpDown_maxmonsters_ValueChanged(null, null);
        }

        private void trackBar_innovation_Scroll(object sender, EventArgs e)
        {
            core.InnovationPercentage = trackBar_innovation.Value;
        }

        private void trackBar_userplacement_Scroll(object sender, EventArgs e)
        {
            core.UserPercentage = trackBar_userplacement.Value;
        }

        private void trackBar_objective_Scroll(object sender, EventArgs e)
        {
            core.ObjectivePercentage = trackBar_objective.Value;
        }

        private void trackBar_hordes_Scroll(object sender, EventArgs e)
        {
            //core.HordesPercentage = trackBar_hordes.Value;
        }

        private void trackBar_mapobjects_Scroll(object sender, EventArgs e)
        {
            //core.MapObjectsPercentage = trackBar_mapobjects.Value;
        }


        private void numericUpDown_maxmonsters_ValueChanged(object sender, EventArgs e)
        {
            //core.MaxMonsters = Convert.ToInt32(numericUpDown_maxmonsters.Value);
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

        private void button_visibility_difficulty_Click(object sender, EventArgs e)
        {
            if (layerDifficulty.Active)
            {
                layerDifficulty.Active = false;
                button_visibility_difficulty.ImageIndex = 0;
            }
            else
            {
                layerDifficulty.Active = true;
                button_visibility_difficulty.ImageIndex = 1;
            }
            ReDraw();
        }

        private void button_visibility_itens_Click(object sender, EventArgs e)
        {
            if (layerItens.Active)
            {
                layerItens.Active = false;
                button_visibility_itens.ImageIndex = 0;
            }
            else
            {
                layerItens.Active = true;
                button_visibility_itens.ImageIndex = 1;
            }
            ReDraw();
        }

        private void button_visibility_monsters_Click(object sender, EventArgs e)
        {
            if (layerMonsters.Active)
            {
                layerMonsters.Active = false;
                button_visibility_monsters.ImageIndex = 0;
            }
            else
            {
                layerMonsters.Active = true;
                button_visibility_monsters.ImageIndex = 1;
            }
            ReDraw();
        }

        private void button_visibility_resources_Click(object sender, EventArgs e)
        {
            if (layerResources.Active)
            {
                layerResources.Active = false;
                button_visibility_resources.ImageIndex = 0;
            }
            else
            {
                layerResources.Active = true;
                button_visibility_resources.ImageIndex = 1;
            }
            ReDraw();
        }

        private void panel_difficulty_click(object sender, MouseEventArgs e)
        {
            if (!layerDifficulty.Attached)
            {
                userSelection.Dettach();
                layerDifficulty.Attach();
                layerItens.Dettach();
                layerMonsters.Dettach();
                layerResources.Dettach();
            }
        }

        private void panel_itens_click(object sender, MouseEventArgs e)
        {
            if (!layerItens.Attached)
            {
                userSelection.Dettach();
                layerDifficulty.Dettach();
                layerItens.Attach();
                layerMonsters.Dettach();
                layerResources.Dettach();
            }
        }

        private void panel_monsters_click(object sender, MouseEventArgs e)
        {
            if (!layerMonsters.Attached)
            {
                userSelection.Dettach();
                layerDifficulty.Dettach();
                layerItens.Dettach();
                layerMonsters.Attach();
                layerResources.Dettach();
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
            core.NewSuggestion();
        }

        public void UpdateTrackHistory()
        {
            Invoke((MethodInvoker)(() =>
            {
                trackBar_history.Maximum = core.CountSuggestions - 1;
                trackBar_history.Value = core.IndexMap;
            }));
        }

        private void panel_resources_click(object sender, MouseEventArgs e)
        {
            if (!layerResources.Attached)
            {
                layerDifficulty.Dettach();
                layerItens.Dettach();
                layerMonsters.Dettach();
                layerResources.Attach();
            }
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
            AreasManager.SetSelected((string)dataGridView.CurrentRow.Cells[0].Value);
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
            settingsForm.Show();
        }

        public void ReDraw()
        {
            Image image = lastImage.Clone() as Image;

            if (layerDifficulty != null) image = layerDifficulty.Draw(gridPanel.Width, gridPanel.Height, image);
            if (layerItens != null) image = layerItens.Draw(gridPanel.Width, gridPanel.Height, image);
            if (layerMonsters != null) image = layerMonsters.Draw(gridPanel.Width, gridPanel.Height, image);
            if (layerResources != null) image = layerResources.Draw(gridPanel.Width, gridPanel.Height, image);
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

    }
}
