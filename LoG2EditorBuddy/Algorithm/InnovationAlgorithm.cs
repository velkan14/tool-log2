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
    class InnovationAlgorithm
    {
        public bool KeepPopulation { get; set; }
        public bool RandomTransferPopulation { get; set; }

        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        private bool running;

        private Map originalMap;

        private Delegate callback;

        public InnovationAlgorithm()
        {
            KeepPopulation = false;
            RandomTransferPopulation = false;

            InitialPopulation = 100;
            GenerationLimit = 100;
            MutationPercentage = 1.0;
            ElitismPercentage = 5;

            running = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap;
            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, currentMap.SpawnCells.Count * 12);

            //create the chromosomes
            /*for (var p = 0; p < InitialPopulation; p++)
            {

                var chromosome = new Chromosome();
                foreach (var cell in map.WalkableCells)
                {
                    chromosome.Genes.Add(new Gene(cell));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }*/


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            //var crossover = new MonsterCrossover(ReplacementMethod.GenerationalReplacement, CrossoverType.SinglePoint, 1.0, true);
            var crossover = new Crossover(0.5, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);
            //create the mutation operator
            //var mutate = new MonsterMutate(MutationPercentage);
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //hook up to some useful events
            //ga.OnGenerationComplete += ;
            //ga.OnRunComplete += ga_OnRunComplete;ga_OnGenerationComplete

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

        private double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            double expectedMonsters = 10.0;
            int numberOfMonsters = 0;

            var genes = chromosome.Genes;

            for (int i = 0; i < genes.Count; i++)
            {
                LoG2API.Cell c = (LoG2API.Cell)genes[i].ObjectValue;
                LoG2API.Cell originalCell = originalMap.GetCellAt(c.X, c.Y);

                if (c.Monster == null && originalCell.Monster == null)
                {
                    fitness += 1;
                }
                else if (!originalMap.GetCellAt(c.X, c.Y).SameMonsters(c))
                {
                    fitness += 1;
                }

                if(c.Monster != null) numberOfMonsters++;
            }
            
            fitness = fitness / genes.Count;

            if (numberOfMonsters > expectedMonsters) fitness = fitness * expectedMonsters / numberOfMonsters; // 5: numero de monstros que queremos;
            else fitness = fitness * numberOfMonsters / expectedMonsters;

            return fitness;
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            double expectedMonsters = 10.0;
            int numberOfMonsters = 0;

            string binaryString = chromosome.ToBinaryString();
            
            List<LoG2API.Cell> spawnCells = originalMap.SpawnCells;

            for (int i = 0; i < spawnCells.Count; i++)
            {
                string s = binaryString.Substring(i * 12, 12);
                string index = s.Substring(0, 3);
                string monster = s.Substring(3, 2);
                string weapon = s.Substring(5, 2);
                string resource = s.Substring(7, 2);
                string armor = s.Substring(9, 3);

                if (index.Equals("000"))
                {
                    //Nothing
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("001"))
                {
                    //Monster
                    if (monster.Equals("00"))
                    {
                        //No Monster
                        if (spawnCells[i].Monster == null)
                        {
                            fitness += 1;
                        }

                    }
                    else if (monster.Equals("01"))
                    {
                        //Skeleton
                        if (spawnCells[i].Monster == null || !spawnCells[i].Monster.ElementType.Equals("skeleton_trooper"))
                        {
                            fitness += 1;
                        }

                        numberOfMonsters++;
                    }
                    else if (monster.Equals("10"))
                    {
                        //Mummy
                        if (spawnCells[i].Monster == null || !spawnCells[i].Monster.ElementType.Equals("mummy"))
                        {
                            fitness += 1;
                        }
                        numberOfMonsters++;
                    }
                    else if (monster.Equals("11"))
                    {
                        //Turtle
                        if (spawnCells[i].Monster == null || !spawnCells[i].Monster.ElementType.Equals("turtle"))
                        {
                            fitness += 1;
                        }
                        numberOfMonsters++;
                    }
                }
                else if (index.Equals("010"))
                {
                    //Weapon
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("011"))
                {
                    //Resource
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("100"))
                {
                    //Armor
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("101"))
                {
                    //Resources and armor
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("110"))
                {
                    //Nothing
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
                else if (index.Equals("111"))
                {
                    //Nothing
                    if (spawnCells[i].Monster == null)
                    {
                        fitness += 1;
                    }
                }
            }

            fitness = fitness / spawnCells.Count;
            if (numberOfMonsters > expectedMonsters) fitness = fitness * expectedMonsters / numberOfMonsters; // 5: numero de monstros que queremos;
            else fitness = fitness * numberOfMonsters / expectedMonsters;

            return fitness;
        }

        private bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > GenerationLimit;
        }

        private void OnRunComplete(object sender, GaEventArgs e)
        {
            running = false;
            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];

            callback.DynamicInvoke(chromosome);

            //Debug.WriteLine("Best Novelty Fitness: " + chromosome.Fitness);
            Logger.AppendText("Best Novelty Fitness: " + chromosome.Fitness);
        }
        
    }
}
