using GAF;
using GAF.Operators;
using Log2CyclePrototype.LoG2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Algorithm
{
    class ConvergenceAlgorithm
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

        public bool HasSolution { get; private set; }
        public Population Solution { get; private set; }


        public ConvergenceAlgorithm()
        {
            InitialPopulation = 200;
            GenerationLimit = 1000;
            MutationPercentage = 0.7;
            CrossOverPercentage = 0.7;
            ElitismPercentage = 5;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap;

            cells = currentMap.SpawnCells;

            foreach (Cell c in cells)
            {
                if (c.Exists("turtle") ||
                   c.Exists("mummy") ||
                   c.Exists("skeleton_trooper"))
                {
                    maxMonsters++;
                }
                if (c.Exists("cudgel") ||
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
            }

            Console.WriteLine("Number Monsters: " + maxMonsters);
            Console.WriteLine("Number Itens: " + maxItens);

            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES, true, true); 

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
            Logger.AppendText("Started the run");
            running = true;
            ga.Run(TerminateFunction);
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            int numberItens = 0;
            int numberMonsters = 0;
            string binaryString = chromosome.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
                {
                    //Turtle
                    if (cells[i].Exists("turtle"))
                    {
                        fitness++;
                    }
                    numberMonsters++;
                }
                else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
                {
                    //Mummy
                    if (cells[i].Exists("mummy"))
                    {
                        fitness++;
                    }
                    numberMonsters++;
                }
                else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
                {
                    //Skeleton
                    if (cells[i].Exists("skeleton_trooper"))
                    {
                        fitness++;
                    }
                    numberMonsters++;
                }
                else if (j == 33 || j == 34 || j == 35)
                {
                    //Cudgel
                    if (cells[i].Exists("cudgel"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 36 || j == 37 || j == 38)
                {
                    //Machete
                    if (cells[i].Exists("machete"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 39 || j == 40 || j == 41)
                {
                    //Rapier
                    if (cells[i].Exists("rapier"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 42 || j == 43 || j == 44)
                {
                    //Battle Axe
                    if (cells[i].Exists("battle_axe"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 45 || j == 46 || j == 47 || j == 48)
                {
                    //Potion
                    if (cells[i].Exists("potion_healing"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 49 || j == 50 || j == 51)
                {
                    //Borra
                    if (cells[i].Exists("borra"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 52 || j == 53 || j == 54)
                {
                    //Bread
                    if (cells[i].Exists("bread"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 55 || j == 56)
                {
                    //Peasant cap
                    if (cells[i].Exists("peasant_cap"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 57 || j == 58)
                {
                    //Peasant breeches
                    if (cells[i].Exists("peasant_breeches"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 59)
                {
                    //Peasant tunic
                    if (cells[i].Exists("peasant_tunic"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 60)
                {
                    //Sandals
                    if (cells[i].Exists("sandals"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 61)
                {
                    //Leather cap
                    if (cells[i].Exists("leather_cap"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 62)
                {
                    //Leather brigandine
                    if (cells[i].Exists("leather_brigandine"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 63)
                {
                    //Leather pants
                    if (cells[i].Exists("leather_pants"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else if (j == 64)
                {
                    //Leather boots
                    if (cells[i].Exists("leather_boots"))
                    {
                        fitness++;
                    }
                    numberItens++;
                }
                else
                {
                    //Nothing
                    if (!cells[i].Exists("turtle") &&
                        !cells[i].Exists("mummy") &&
                        !cells[i].Exists("skeleton_trooper") &&
                        !cells[i].Exists("cudgel") &&
                        !cells[i].Exists("machete") &&
                        !cells[i].Exists("rapier") &&
                        !cells[i].Exists("battle_axe") &&
                        !cells[i].Exists("potion_healing") &&
                        !cells[i].Exists("borra") &&
                        !cells[i].Exists("bread") &&
                        !cells[i].Exists("peasant_cap") &&
                        !cells[i].Exists("peasant_breeches") &&
                        !cells[i].Exists("peasant_tunic") &&
                        !cells[i].Exists("sandals") &&
                        !cells[i].Exists("leather_cap") &&
                        !cells[i].Exists("leather_brigandine") &&
                        !cells[i].Exists("leather_pants") &&
                        !cells[i].Exists("leather_boots"))
                    {
                        //fitness++;
                    }
                }
            }

           
            fitness = fitness / (maxItens + maxMonsters);

            double fit = func(numberMonsters + numberItens, maxItens + maxMonsters);

            double monstersFit = func(numberMonsters, maxMonsters);

            double itensFit = func(numberItens, maxItens);

            return (fitness + (monstersFit + itensFit) / 2.0) / 2.0;
        }

        private static double func(double x, double c)
        {
            if (x == c) return 1.0;
            if (x < c) return -c / (x - 2.0 * c);
            if (x > c) return c / x;
            return 0.0;
        }

        private bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
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
