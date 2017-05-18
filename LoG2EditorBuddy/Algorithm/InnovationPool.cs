using GAF;
using GAF.Extensions;
using GAF.Operators;
using Log2CyclePrototype.Algorithm;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    class InnovationPool
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
        private Monsters monsters;

        public bool HasSolution { get; private set; }
        public Population Solution { get; private set; }


        public InnovationPool(Monsters monsters)
        {
            this.monsters = monsters;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 0.35;
            CrossOverPercentage = 0.4;
            ElitismPercentage = 5;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap.CloneJson() as Map;
            cells = originalMap.SpawnCells;

            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES);

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, APIClass.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);
            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //hook up to some useful events
            ga.OnRunComplete += OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);
            

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double positionFitness = 0.0; // Value between 0 and 1. 1 is the fittest

            string binaryString = chromosome.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
                {
                    //Turtle
                    if (!cells[i].Exists("turtle"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
                {
                    //Mummy
                    if (!cells[i].Exists("mummy"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
                {
                    //Skeleton
                    if (!cells[i].Exists("skeleton_trooper"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 33 || j == 34 || j == 35)
                {
                    //Cudgel
                    if (!cells[i].Exists("cudgel"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 36 || j == 37 || j == 38)
                {
                    //Machete
                    if (!cells[i].Exists("machete"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 39 || j == 40 || j == 41)
                {
                    //Rapier
                    if (!cells[i].Exists("rapier"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 42 || j == 43 || j == 44)
                {
                    //Battle Axe
                    if (!cells[i].Exists("battle_axe"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 45 || j == 46 || j == 47 || j == 48)
                {
                    //Potion
                    if (!cells[i].Exists("potion_healing"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 49 || j == 50 || j == 51)
                {
                    //Borra
                    if (!cells[i].Exists("borra"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 52 || j == 53 || j == 54)
                {
                    //Bread
                    if (!cells[i].Exists("bread"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 55 || j == 56)
                {
                    //Peasant cap
                    if (!cells[i].Exists("peasant_cap"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 57 || j == 58)
                {
                    //Peasant breeches
                    if (!cells[i].Exists("peasant_breeches"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 59)
                {
                    //Peasant tunic
                    if (!cells[i].Exists("peasant_tunic"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 60)
                {
                    //Sandals
                    if (!cells[i].Exists("sandals"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 61)
                {
                    //Leather cap
                    if (!cells[i].Exists("leather_cap"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 62)
                {
                    //Leather brigandine
                    if (!cells[i].Exists("leather_brigandine"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 63)
                {
                    //Leather pants
                    if (!cells[i].Exists("leather_pants"))
                    {
                        positionFitness++;
                    }
                }
                else if (j == 64)
                {
                    //Leather boots
                    if (!cells[i].Exists("leather_boots"))
                    {
                        positionFitness++;
                    }
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
                        positionFitness++;
                    }
                }
            }

            positionFitness = positionFitness / cells.Count;

            return positionFitness;
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(2, 100 * currentGeneration / GenerationLimit);
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
