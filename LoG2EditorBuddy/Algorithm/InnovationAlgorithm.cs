using GAF;
using GAF.Extensions;
using GAF.Operators;
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

        public CrossoverType CrossoverType { get; set; }

        private Map originalMap;

        public InnovationAlgorithm()
        {
            KeepPopulation = false;
            RandomTransferPopulation = false;

            InitialPopulation = 100;
            GenerationLimit = 50;
            MutationPercentage = 0.15;
            ElitismPercentage = 5;
        }

        public void Run(Map currentMap)
        {
            originalMap = currentMap;

            //get our cities
            Map map = CloneUtilities.CloneJson<Map>(currentMap);

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();

            //create the chromosomes
            for (var p = 0; p < InitialPopulation; p++)
            {

                var chromosome = new Chromosome();
                foreach (var cell in map.Cells)
                {
                    chromosome.Genes.Add(new Gene(cell));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new MonsterCrossover(map.Width, map.Height, ReplacementMethod.DeleteLast, CrossoverType, 0.8, true);

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            //ga.OnGenerationComplete += ga_OnGenerationComplete;
            //ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            ga.Run(TerminateFunction);
        }

        private double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest

            var genes = chromosome.Genes;

            for (int i = 0; i < genes.Count; i++)
            {
                //grab a cell from the candidate chromosome
                Cell c = (Cell)genes[i].ObjectValue;

                if (!originalMap.Cells[i].SameMonsters(c))
                {
                    fitness += 1;
                }
            }

            fitness = fitness / genes.Count;

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

        private void OnNoveltyComplete(object sender, GaEventArgs e)
        {

            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];
           
            //Debug.WriteLine("Best Novelty Fitness: " + chromosome.Fitness);
            Logger.AppendText("Best Novelty Fitness: " + chromosome.Fitness);
        }

    }
}
