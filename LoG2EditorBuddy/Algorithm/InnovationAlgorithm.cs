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
        }

        public void Run(Map currentMap, Delegate callback)
        {
            originalMap = currentMap;
            this.callback = callback;

            //get our cities
            Map map = currentMap.CloneJson() as Map;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < InitialPopulation; p++)
            {

                var chromosome = new Chromosome();
                foreach (var cell in map.WalkableCells)
                {
                    chromosome.Genes.Add(new Gene(cell));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new MonsterCrossover(ReplacementMethod.GenerationalReplacement, CrossoverType.SinglePoint, 1.0, true);

            //create the mutation operator
            var mutate = new MonsterMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            //ga.OnGenerationComplete += ga_OnGenerationComplete;
            //ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            Logger.AppendText("Started the run");
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

            /*if (numberOfMonsters > expectedMonsters) fitness = fitness * expectedMonsters / numberOfMonsters; // 5: numero de monstros que queremos;
            else fitness = fitness * numberOfMonsters / expectedMonsters;*/

            //FIXME: Será preciso fazer alguma penalização... Ter em conta o número de monstros por exemplo para não excederam um número base.
            /*float cellOffsetPenalty = 0;
            cellOffsetPenalty = System.Math.Abs(TargetWalkableCellCount - APIClass.CountWalkableCellsInChromosome(chromosome)) / 1024f;
            fitness -= cellOffsetPenalty;*/
            return fitness;
        }

        private bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > GenerationLimit;
        }

        private void OnRunComplete(object sender, GaEventArgs e)
        {            
            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];


            List<LoG2API.Cell> cells = new List<LoG2API.Cell>();
            foreach(Gene g in chromosome.Genes)
            {
                cells.Add((LoG2API.Cell)g.ObjectValue);
            }

            callback.DynamicInvoke(cells);

            //Debug.WriteLine("Best Novelty Fitness: " + chromosome.Fitness);
            Logger.AppendText("Best Novelty Fitness: " + chromosome.Fitness);
        }
        
    }
}
