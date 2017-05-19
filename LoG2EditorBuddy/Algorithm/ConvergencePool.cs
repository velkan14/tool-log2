using GAF;
using GAF.Operators;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm
{
    class ConvergencePool : HasStuff
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        private bool running;
        private Delegate callback;

        private Map originalMap;
        private List<Cell> cells;

        private int maxItens = 0;
        private int maxMonsters = 0;
        private int emptyTiles = 0;
        private Monsters monsters;

        public bool HasSolution { get; private set; }
        public Population Solution { get; private set; }


        public ConvergencePool(Monsters monsters)
        {
            this.monsters = monsters;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 0.7;
            CrossOverPercentage = 0.7;
            ElitismPercentage = 5;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap.CloneJson() as Map;

            cells = originalMap.SpawnCells;

            foreach (Cell c in cells)
            {
                if (c.Exists("turtle") ||
                   c.Exists("mummy") ||
                   c.Exists("skeleton_trooper"))
                {
                    maxMonsters++;
                }
                else if (c.Exists("cudgel") ||
                   c.Exists("machete") ||
                   c.Exists("rapier") ||
                   c.Exists("battle_axe") ||
                   c.Exists("potion_healing") ||
                   c.Exists("borra") ||
                   c.Exists("bread") ||
                   c.Exists("peasant_cap") ||
                   c.Exists("peasant_breeches") ||
                   c.Exists("peasant_tunic") ||
                   c.Exists("sandals") ||
                   c.Exists("leather_cap") ||
                   c.Exists("leather_brigandine") ||
                   c.Exists("leather_pants") ||
                   c.Exists("leather_boots"))
                {
                    maxItens++;
                }
                else
                {
                    emptyTiles++;
                }
            }

            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES, true, true);

            population.Solutions.Clear();

            Chromosome chrom = APIClass.ChromosomeFromMap(originalMap);

            for (int i = 0; i < InitialPopulation; i++)
            {
                population.Solutions.Add(new Chromosome(chrom.ToBinaryString()));
            }

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, APIClass.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        /* Para quando x <= n */
        private double FunctionRising(double x, double n)
        {
            return System.Math.Min(System.Math.Abs(x / n), 1.0);
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            int numberItens = 0;
            int numberMonsters = 0;
            int numberEqual = 0;
            int numberEmpty = 0;

            string binaryString = chromosome.ToBinaryString();

            List<CellStruct> listCells = new List<CellStruct>();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);
                listCells.Add(new CellStruct(j, cells[i].X, cells[i].Y));
            }

            foreach (Cell c in cells)
            {
                if (HasMonster(c.X, c.Y, listCells) && c.Monster != null)
                {
                    numberMonsters++;
                    if (HasTurtle(c.X, c.Y, listCells) && c.Exists("turtle")) numberEqual++;
                    if (HasMummy(c.X, c.Y, listCells) && c.Exists("mummy")) numberEqual++;
                    if (HasSkeleton(c.X, c.Y, listCells) && c.Exists("skeleton_trooper")) numberEqual++;
                }
                if (HasItem(c.X, c.Y, listCells) && (c.Exists("cudgel") ||
                   c.Exists("machete") ||
                   c.Exists("rapier") ||
                   c.Exists("battle_axe") ||
                   c.Exists("potion_healing") ||
                   c.Exists("borra") ||
                   c.Exists("bread") ||
                   c.Exists("peasant_cap") ||
                   c.Exists("peasant_breeches") ||
                   c.Exists("peasant_tunic") ||
                   c.Exists("sandals") ||
                   c.Exists("leather_cap") ||
                   c.Exists("leather_brigandine") ||
                   c.Exists("leather_pants") ||
                   c.Exists("leather_boots")))
                {
                    numberItens++;
                    if (HasResource(c.X, c.Y, listCells) && (c.Exists("cudgel") ||
                   c.Exists("machete") ||
                   c.Exists("rapier") ||
                   c.Exists("battle_axe"))) numberEqual++;
                    if (HasWeapon(c.X, c.Y, listCells) && (c.Exists("potion_healing") ||
                   c.Exists("borra") ||
                   c.Exists("bread"))) numberEqual++;
                    if (HasArmor(c.X, c.Y, listCells) &&(c.Exists("peasant_cap") ||
                   c.Exists("peasant_breeches") ||
                   c.Exists("peasant_tunic") ||
                   c.Exists("sandals") ||
                   c.Exists("leather_cap") ||
                   c.Exists("leather_brigandine") ||
                   c.Exists("leather_pants") ||
                   c.Exists("leather_boots"))) numberEqual++;
                }
                if(!HasMonster(c.X, c.Y, listCells) && !HasItem(c.X, c.Y, listCells) && 
                    !c.Exists("turtle") &&
                   !c.Exists("mummy") &&
                   !c.Exists("skeleton_trooper") &&
                   !c.Exists("cudgel") &&
                   !c.Exists("machete") &&
                   !c.Exists("rapier") &&
                   !c.Exists("battle_axe") &&
                   !c.Exists("potion_healing") &&
                   !c.Exists("borra") &&
                   !c.Exists("bread") &&
                   !c.Exists("peasant_cap") &&
                   !c.Exists("peasant_breeches") &&
                   !c.Exists("peasant_tunic") &&
                   !c.Exists("sandals") &&
                   !c.Exists("leather_cap") &&
                   !c.Exists("leather_brigandine") &&
                   !c.Exists("leather_pants") &&
                   !c.Exists("leather_boots"))
                {
                    numberEmpty++;
                }


            }
            fitness = (1.0 / 4.0) * FunctionRising(numberMonsters, maxMonsters) +
                    (1.0 / 4.0) * FunctionRising(numberItens, maxItens) +
                   (1.0 / 4.0) * FunctionRising(numberEqual, maxMonsters) +
                   (1.0 / 4.0) * FunctionRising(numberEmpty, emptyTiles);

            return fitness;
        }

        private bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(0, 100 * currentGeneration / GenerationLimit);
            return currentGeneration > GenerationLimit;
        }

        private void OnRunComplete(object sender, GaEventArgs e)
        {
            Solution = e.Population;

            running = false;
            HasSolution = true;

            callback.DynamicInvoke();
        }
    }
}
