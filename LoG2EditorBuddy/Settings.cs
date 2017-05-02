using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log2CyclePrototype
{
    public partial class Settings : Form
    {
        Core core;
        public Settings(Core core)
        {
            this.core = core;
            InitializeComponent();
            initializeParameters(); 
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void initializeParameters()
        {
            NumericUpDown_Objective_InitialPopulation_ValueChanged(null, null);
            NumericUpDown_Objective_mutationPercentage_ValueChanged(null, null);
            NumericUpDown_Objective_generations_ValueChanged(null, null);
            NumericUpDown_Objective_elitismPercentage_ValueChanged(null, null);
            NumericUpDown_Innovation_initialPopulation_ValueChanged(null, null);
            NumericUpDown_Innovation_mutationPercentage_ValueChanged(null, null);
            NumericUpDown_Innovation_generations_ValueChanged(null, null);
            NumericUpDown_Innovation_elitismPercentage_ValueChanged(null, null);
            core.CrossoverType = CrossoverT.FourByFourSquare;
            CheckBox_RandomPopCarryOver_CheckedChanged(null, null);
            CheckBox_keepPopulation_CheckedChanged(null, null);
        }

        private void Button_ResetAlgorithm_Click(object sender, EventArgs e)
        {

        }

        private void NumericUpDown_Objective_InitialPopulation_ValueChanged(object sender, EventArgs e)
        {
            core.InitialPopulationObjective = Convert.ToInt32(NumericUpDown_Objective_InitialPopulation.Value);
        }

        private void NumericUpDown_Objective_mutationPercentage_ValueChanged(object sender, EventArgs e)
        {
            core.MutationPercentageObjective = Convert.ToInt32(NumericUpDown_Objective_mutationPercentage.Value);
        }

        private void NumericUpDown_Objective_generations_ValueChanged(object sender, EventArgs e)
        {
            core.GenerationsObjective = Convert.ToInt32(NumericUpDown_Objective_generations.Value);
        }

        private void NumericUpDown_Objective_elitismPercentage_ValueChanged(object sender, EventArgs e)
        {
            core.ElitismPercentageObjective = Convert.ToInt32(NumericUpDown_Objective_elitismPercentage.Value);
        }

        private void NumericUpDown_Innovation_initialPopulation_ValueChanged(object sender, EventArgs e)
        {
            core.InitialPopulationInnovation = Convert.ToInt32(NumericUpDown_Innovation_initialPopulation.Value);
        }

        private void NumericUpDown_Innovation_mutationPercentage_ValueChanged(object sender, EventArgs e)
        {
            core.MutationPercentageInnovation = Convert.ToInt32(NumericUpDown_Innovation_mutationPercentage.Value);
        }

        private void NumericUpDown_Innovation_generations_ValueChanged(object sender, EventArgs e)
        {
            core.GenerationsInnovation = Convert.ToInt32(NumericUpDown_Innovation_generations.Value);
        }

        private void NumericUpDown_Innovation_elitismPercentage_ValueChanged(object sender, EventArgs e)
        {
            core.ElitismPercentageInnovation = Convert.ToInt32(NumericUpDown_Innovation_elitismPercentage.Value);
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
                    core.CrossoverType = CrossoverT.SinglePoint;
                    Logger.AppendText("Crossover Type changed to Single Point");
                }
                else if (itemTag.ToString().Contains("dp"))
                {
                    core.CrossoverType = CrossoverT.DoublePoint;
                    Logger.AppendText("Crossover Type changed to Double Point");
                }
                else if (itemTag.ToString().Contains("2x2s"))
                {
                    core.CrossoverType = CrossoverT.TwoByTwoSquare;
                    Logger.AppendText("Crossover Type changed to custom 2x2 Square shape");
                }
                else if (itemTag.ToString().Contains("3x3s"))
                {
                    core.CrossoverType = CrossoverT.ThreeByThreeSquare;
                    Logger.AppendText("Crossover Type changed to custom 3x3 Square shape");
                }
                else if (itemTag.ToString().Contains("4x4s"))
                {
                    core.CrossoverType = CrossoverT.FourByFourSquare;
                    Logger.AppendText("Crossover Type changed to custom 4x4 Square shape");
                }
            }

        }

        private void CheckBox_RandomPopCarryOver_CheckedChanged(object sender, EventArgs e)
        {
            core.RandomTransferPopulation = CheckBox_RandomPopCarryOver.Checked;
        }

        private void CheckBox_keepPopulation_CheckedChanged(object sender, EventArgs e)
        {
            core.KeepPopulation = CheckBox_keepPopulation.Checked;
        }
    }
}
