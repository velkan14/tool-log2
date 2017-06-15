using GAF;
using GAF.Extensions;
using GAF.Operators;
using EditorBuddyMonster.Algorithm;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster
{
    class InnovationPool : HasStuff
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        protected bool running;
        protected Delegate callback;

        protected Map originalMap;
        protected List<Cell> cells;
        protected Monsters monsters;

        public bool HasSolution { get; protected set; }
        public Population Solution { get; protected set; }

        List<CellStruct> mapCells = new List<CellStruct>();

        int numberOfSkeletons = 0;
        int numberOfMummy = 0;
        int numberOfTurtle = 0;
        int numberOfArmor = 0;
        int numberOfResource = 0;
        int numberOfWeapon = 0;
        int numberOfMonsters = 0;
        int numberOfItens = 0;

        public InnovationPool(Monsters monsters)
        {
            this.monsters = monsters;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 0.35;
            CrossOverPercentage = 0.4;
            ElitismPercentage = 10;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap.CloneJson() as Map;
            cells = originalMap.SpawnCells;

            this.callback = callback;

            Chromosome chrom = ChromosomeUtils.ChromosomeFromMap(originalMap);

            string binaryString = chrom.ToBinaryString();

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

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * ChromosomeUtils.NUMBER_GENES, true, true);

            population.Solutions.Clear();

            for (int i = 0; i < InitialPopulation; i++)
            {
                population.Solutions.Add(new Chromosome(binaryString));
            }

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the mutation operator
            var mutate = new MutateInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES);

            var swap = new MutateSwapInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //hook up to some useful events
            ga.OnRunComplete += OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(mutate);
            ga.Operators.Add(swap);
            

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        protected double CalculateFitnessBinary(Chromosome chromosome)
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
            foreach(CellStruct c in listThing)
            {
                CellStruct originalCell = mapCells.FirstOrDefault(k => k.x == c.x && k.y == c.y);

                if(SameType(c, originalCell))
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


            return Invert(fitness);
        }

        protected double FunctionUpDown(double x, double n)
        {
            if(n == 0)
            {
                return System.Math.Max(0.0, -System.Math.Abs(x / 2.0) + 1.0);
            }
            return System.Math.Max(0.0, -System.Math.Abs((x / n) - 1.0) + 1.0);
        }
        
        protected double Invert(double x)
        {
            return 1.0 - x;
        }
        protected double Equality(CellStruct c, CellStruct originalCell)
        {
            if (SameType(c, originalCell) && !AreEquals(c, originalCell))
            {
                return 1.0;
            }
            return 0.0;
        }

        protected bool SameType(CellStruct c, CellStruct originalCell)
        {
            if(HasMonster(c) && HasMonster(originalCell) ||
               HasItem(c) && HasItem(originalCell))
            {
                return true;
            }
            return false;
        }

        protected bool AreEquals(CellStruct c, CellStruct originalCell)
        {
            if(HasArmor(c) && HasArmor(originalCell) ||
                HasResource(c) && HasResource(originalCell) ||
                HasWeapon(c) && HasWeapon(originalCell) ||
                HasMummy(c) && HasMummy(originalCell) ||
                HasTurtle(c) && HasTurtle(originalCell) ||
                HasSkeleton(c) && HasSkeleton(originalCell) ||
                IsEmpty(c) && IsEmpty(originalCell))
            {
                return true;
            }
            return false;
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(2, 100 * currentGeneration / GenerationLimit);
            return currentGeneration > GenerationLimit;
        }

        protected void OnRunComplete(object sender, GaEventArgs e)
        {
            Solution = e.Population;

            running = false;
            HasSolution = true;

            callback.DynamicInvoke();
        }
        
    }
}
