using EditorBuddyMonster.LoG2API;
using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm.Fitness
{
    class ConvergenceFitness : HasStuff
    {
        protected List<Cell> cells;
        List<CellStruct> mapCells = new List<CellStruct>();

        int numberOfSkeletons = 0;
        int numberOfMummy = 0;
        int numberOfTurtle = 0;
        int numberOfArmor = 0;
        int numberOfResource = 0;
        int numberOfWeapon = 0;
        int numberOfMonsters = 0;
        int numberOfItens = 0;

        public ConvergenceFitness(List<Cell> spawCells, string binaryString)
        {
            cells = spawCells;

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES, ChromosomeUtils.NUMBER_GENES);
                int type = Convert.ToInt32(s, 2);

                CellStruct cell = new CellStruct(type, cells[i].X, cells[i].Y);
                mapCells.Add(cell);

                if (HasSkeleton(cell))
                {
                    numberOfSkeletons++;
                }
                else if (HasMummy(cell))
                {
                    numberOfMummy++;
                }
                else if (HasTurtle(cell))
                {
                    numberOfTurtle++;
                }
                else if (HasArmor(cell))
                {
                    numberOfArmor++;
                }
                else if (HasResource(cell))
                {
                    numberOfResource++;
                }
                else if (HasWeapon(cell))
                {
                    numberOfWeapon++;
                }
            }

            numberOfMonsters = numberOfSkeletons + numberOfTurtle + numberOfMummy;
            numberOfItens = numberOfArmor + numberOfResource + numberOfWeapon;
        }

        public double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest

            int numberSkeletons = 0;
            int numberMummy = 0;
            int numberTurtle = 0;
            int numberArmor = 0;
            int numberResource = 0;
            int numberWeapon = 0;

            List<CellStruct> listThing = new List<CellStruct>();

            string binaryString = chromosome.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES, ChromosomeUtils.NUMBER_GENES);

                int type = Convert.ToInt32(s, 2);
                CellStruct tmp = new CellStruct(type, cells[i].X, cells[i].Y);

                if (HasMonster(tmp) || HasItem(tmp))
                {
                    listThing.Add(tmp);
                }

                if (HasSkeleton(tmp))
                {
                    numberSkeletons++;
                }
                else if (HasMummy(tmp))
                {
                    numberMummy++;
                }
                else if (HasTurtle(tmp))
                {
                    numberTurtle++;
                }
                else if (HasArmor(tmp))
                {
                    numberArmor++;
                }
                else if (HasResource(tmp))
                {
                    numberResource++;
                }
                else if (HasWeapon(tmp))
                {
                    numberWeapon++;
                }
            }

            int position = 0;
            foreach (CellStruct c in listThing)
            {
                CellStruct originalCell = mapCells.FirstOrDefault(k => k.x == c.x && k.y == c.y);

                if (SameType(c, originalCell))
                {
                    position++;
                }
            }

            int numberItens = numberResource + numberArmor + numberWeapon;
            int numberMonsters = numberSkeletons + numberMummy + numberTurtle;

            double fitnessNumber = FunctionUpDown(numberMonsters, numberOfMonsters) *
                                   FunctionUpDown(numberItens, numberOfItens);

            double fitnessPosition = FunctionUpDown(position, numberOfItens + numberOfMonsters);

            double fitnessType = FunctionUpDown(numberArmor, numberOfArmor) *
                      FunctionUpDown(numberMummy, numberOfMummy) *
                      FunctionUpDown(numberResource, numberOfResource) *
                      FunctionUpDown(numberSkeletons, numberOfSkeletons) *
                      FunctionUpDown(numberTurtle, numberOfTurtle) *
                      FunctionUpDown(numberWeapon, numberOfWeapon);

            fitness = (1.0 / 2.0) * fitnessNumber + (1.0 / 4.0) * fitnessPosition + (1.0 / 4.0) * fitnessType;


            return fitness;
        }

        protected bool SameType(CellStruct c, CellStruct originalCell)
        {
            if (HasMonster(c) && HasMonster(originalCell) ||
               HasItem(c) && HasItem(originalCell))
            {
                return true;
            }
            return false;
        }

        protected double FunctionUpDown(double x, double n)
        {
            if (n == 0)
            {
                return System.Math.Max(0.0, -System.Math.Abs(x / 2.0) + 1.0);
            }
            return System.Math.Max(0.0, -System.Math.Abs((x / n) - 1.0) + 1.0);
        }
    }
}
