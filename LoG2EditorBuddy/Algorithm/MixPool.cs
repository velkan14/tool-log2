using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using EditorBuddyMonster.LoG2API;
using GAF.Operators;
using EditorBuddyMonster.Layers;

namespace EditorBuddyMonster.Algorithm
{
    class MixPool : GuidelinePool
    {
        /*******************************************************/
        /*********************Parameters************************/
        /*******************************************************/
        public double GuidelinePercentage { get; set; }
        public double InnovationPercentage { get; set; }
        public double UserPercentage { get; set; }

        /*******************************************************/

        public MixPool(Monsters monsters) : base(monsters)
        {
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

        internal void Run(Map currentMap, AreaManager areaManager, Population conv, Population inno, Population obj, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap;
            cells = currentMap.SpawnCells;

            this.areaManager = areaManager;
            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES, true, true, ParentSelectionMethod.StochasticUniversalSampling);

            population.Solutions.Clear();

            double total = GuidelinePercentage + UserPercentage + InnovationPercentage;
            int objective = (int)System.Math.Round((GuidelinePercentage / total) * InitialPopulation);
            int user = (int)System.Math.Round((UserPercentage / total) * InitialPopulation);
            int innov = (int)System.Math.Round((InnovationPercentage / total) * InitialPopulation);

            int fix = InitialPopulation - (objective + user + innov);

            user += fix;
            
            population.Solutions.AddRange(obj.GetTop(objective));
            population.Solutions.AddRange(obj.GetTop(user));
            population.Solutions.AddRange(obj.GetTop(innov));


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, APIClass.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(3, 100 * currentGeneration / GenerationLimit);
            return currentGeneration > GenerationLimit;
        }
    }
}
