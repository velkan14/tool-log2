﻿using gma.System.Windows;
using Log2CyclePrototype.Exceptions;
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

namespace Log2CyclePrototype
{
    public partial class Monsters : Form
    {
        private Settings settingsForm;

        private Draw drawer;
        private Core core;
        private UserSelection userSelection;
        

        private Map currentMap, previousMap, solutionChromosomeMap;
       

        public Monsters()
        {
            InitializeComponent();
        }

        private void Monsters_Load(object sender, EventArgs e)
        {
            core = new Core(this);
            userSelection = core.getUserSelection();

            //Solves flickering when redrawing gridPanel and triangle Panel
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, gridPanel, new object[] { true });

            drawer = new Draw(gridPanel);
            

            Logger.EntryWritten += Logger_EntryWritten;

            settingsForm = new Settings(core);
            initializeParameters(); //FIXME: Talvez não seja a forma correcta de o fazer
        }

        /*********************************************************************************/
        /******************--------------SELECTION---------------*************************/
        /*********************************************************************************/

        #region Selection

        bool selectionAdd = true;
        private Point startPointSelection;
        private Rectangle SelectionRect = new Rectangle();

        private void gridPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                startPointSelection = e.Location;
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
        private void gridPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                Point tempEndPoint = e.Location;
                SelectionRect.Location = new Point(
                    System.Math.Min(startPointSelection.X, tempEndPoint.X),
                    System.Math.Min(startPointSelection.Y, tempEndPoint.Y));
                SelectionRect.Size = new Size(
                    System.Math.Abs(startPointSelection.X - tempEndPoint.X),
                    System.Math.Abs(startPointSelection.Y - tempEndPoint.Y));
                if (currentMap != null)
                {
                    gridPanel.BackgroundImage = drawer.DrawSelectionPreviewRectangle(SelectionRect, selectionAdd);
                    gridPanel.Refresh();
                }
                

            }

        }

        private void gridPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentMap == null)
                return;

            try
            {
                int cellWidth = gridPanel.Width / APIClass.CurrentMap.Width;
                int cellHeight = gridPanel.Height / APIClass.CurrentMap.Height;
                Point topLeft = new Point(), bottomRight = new Point();
                Point startCellCoord = new Point(startPointSelection.X / cellWidth, startPointSelection.Y / cellHeight);
                Point endCellCoord = new Point(e.Location.X / cellWidth, e.Location.Y / cellHeight);

                //if 1 click on same cell
                if (startCellCoord.Equals(endCellCoord))
                {
                    Debug.WriteLine("Click @ [" + startCellCoord.X.ToString() + "," + startCellCoord.Y.ToString() + "]");
                    var tmpP = new Point(startCellCoord.X, startCellCoord.Y);

                    //DELETE
                    if (e.Button == MouseButtons.Right)
                    {
                        userSelection.removeSelectedPoint(tmpP);
                    }
                    //ADD
                    else if (e.Button == MouseButtons.Left)
                    {
                        userSelection.addSelectedPoint(tmpP);
                    }
                    
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
                            //DELETE
                            if (e.Button == MouseButtons.Right)
                            {
                                userSelection.removeSelectedPoint(tmpP);
                            }
                            //ADD
                            else if (e.Button == MouseButtons.Left)
                            {
                                userSelection.addSelectedPoint(tmpP);
                            }
                        }

                }
                

                //reset selection so it is no longer drawn on mouse up
                SelectionRect = new Rectangle(new Point(0, 0), new Size(0, 0));
                ReDraw();
            }
            catch (Exception ee)
            {
                Debug.Print(ee.ToString());
            }

        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            ClearSelectedUserCells();
            ReDraw();
        }

        private void button_invert_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
                return;

            if (userSelection.invertSelection())
            {
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

        private void ClearSelectedUserCells()
        {
            userSelection.clearSelection();
            startPointSelection = new Point(-1, -1);
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
            trackBar_endpoints_Scroll(null, null);
            numericUpDown_maxmonsters_ValueChanged(null, null);
            numericUpDown_characterlevel_ValueChanged(null, null);
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
            core.HordesPercentage = trackBar_hordes.Value;
        }

        private void trackBar_mapobjects_Scroll(object sender, EventArgs e)
        {
            core.MapObjectsPercentage = trackBar_mapobjects.Value;
        }

        private void trackBar_endpoints_Scroll(object sender, EventArgs e)
        {
            core.EndPointsPercentage = trackBar_endpoints.Value;
        }

        private void numericUpDown_maxmonsters_ValueChanged(object sender, EventArgs e)
        {
            core.MaxMonsters = Convert.ToInt32(numericUpDown_maxmonsters.Value);
        }

        private void numericUpDown_characterlevel_ValueChanged(object sender, EventArgs e)
        {
            core.CharacterLevel = Convert.ToInt32(numericUpDown_characterlevel.Value);
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
                if (!core.loadDirectory(fd.SelectedPath))
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
            /*if (APIClass.CurrentMap == null)
            {
                Logger.AppendText("Current map on API null");
                return;
            }

            try
            {
                if (solutionChromosomeMap != null && userSelectedPoints != null)
                {
                    if (userSelectedPoints.Count == 0)
                    {
                        Logger.AppendText("No selection, exporting entire solution.");
                        if (solutionChromosomeMap != null)
                        {
                            var res = APIClass.SaveMapFile(solutionChromosomeMap);
                        }
                        else Debug.WriteLine("Cant save, solution map null");
                        //return;
                    }
                    else
                    {
                        Logger.AppendText("Exporting solution.");
                        bool result = APIClass.ExportSelection(solutionChromosomeMap, userSelectedPoints); //use map vs list of genes
                        if (result)
                            activityHook.SendReloadCommand();
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
            }*/
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
        }

        public void ReDraw()
        {
            gridPanel.BackgroundImage = drawer.ReDraw(core.getMap(), userSelection.getSelectedPoints()); //FIXME

            if (gridPanel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { gridPanel.Refresh(); })); //needed when calling the callback from a different thread
            }
            else
            {
                gridPanel.Refresh();
            }
        }


       
    }
}
