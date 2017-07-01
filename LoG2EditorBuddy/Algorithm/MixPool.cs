using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using EditorBuddyMonster.LoG2API;
using GAF.Operators;
using EditorBuddyMonster.Layers;
using EditorBuddyMonster.Algorithm.Fitness;
using EditorBuddyMonster.Utilities;

namespace EditorBuddyMonster.Algorithm
{
    class MixPool
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }


        /*******************************************************/
        /*********************Parameters************************/
        /*******************************************************/
        public double GuidelinePercentage { get; set; }
        public double InnovationPercentage { get; set; }
        public double UserPercentage { get; set; }

        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public double HordesPercentage { get; set; }
        /*******************************************************/

        protected bool running;
        protected Delegate callback;

        protected Map originalMap;
        protected List<Cell> cells;
        protected Monsters monsters;

        public bool HasSolution { get; protected set; }
        public Population Solution { get; protected set; }

        ConvergenceFitness convergenceFitness;
        Guideline guidelineFitness;

        double guidelineP = 0.0;
        double userP = 0.0;
        double innovationP = 0.0;

        public MixPool(Monsters monsters, Map currentMap, Delegate callback)
        {
            this.monsters = monsters;
            this.callback = callback;

            originalMap = currentMap.CloneJson() as Map;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 1.0;
            CrossOverPercentage = 0.4;
            ElitismPercentage = 5;


            /*GuidelinePercentage = 1.0f;
            InnovationPercentage = 0.4f;
            UserPercentage = 0.4f;

            MaxMonsters = 7;
            MaxItens = 5;*/

            running = false;
            HasSolution = false;
        }

        internal void Run(AreaManager areaManager, Population conv, Population inno, Population obj)
        {
            if (running) return;
            
            cells = originalMap.SpawnCells;
            
            
            Chromosome chrom = ChromosomeUtils.ChromosomeFromMap(originalMap);

            string binaryString = chrom.ToBinaryString();

            convergenceFitness = new ConvergenceFitness(cells, binaryString);
            guidelineFitness = new Guideline(cells, areaManager, MaxMonsters, MaxItens, HordesPercentage);

            double total = GuidelinePercentage + UserPercentage + InnovationPercentage;
            guidelineP = GuidelinePercentage / total;
            userP = UserPercentage / total;
            innovationP = InnovationPercentage / total;
            Console.WriteLine("Guideline: {0}; User: {1}; Innovation: {2}", guidelineP, userP, innovationP);

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * ChromosomeUtils.NUMBER_GENES, true, true, ParentSelectionMethod.StochasticUniversalSampling);

            population.Solutions.Clear();
            
            population.Solutions.AddRange(obj.GetTop(10));
            population.Solutions.AddRange(inno.GetTop(10));
            population.Solutions.AddRange(conv.GetTop(10));


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, ChromosomeUtils.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            //ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        protected double CalculateFitness(Chromosome chromosome)
        {
            double totalFitness = 0.0;

            double convFit = convergenceFitness.CalculateFitness(chromosome);
            double innoFit = 1.0 - convFit;
            double guidFit = guidelineFitness.CalculateFitness(chromosome);
            
            totalFitness =  guidelineP * guidFit +
                            0.2*userP * convFit +
                            0.5*innovationP * innoFit;

            return totalFitness;
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(3, 100 * currentGeneration / GenerationLimit);
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
