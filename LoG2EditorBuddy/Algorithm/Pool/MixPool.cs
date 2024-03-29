﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using Povoater.LoG2API;
using GAF.Operators;
using Povoater.Layers;
using Povoater.Algorithm.Fitness;
using Povoater.Utilities;

namespace Povoater.Algorithm
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

        double guidelineLevel = 0.0;
        double ConvergenceLevel = 0.0;
        double innovationLevel = 0.0;

        public double LeverMaxMonster { get; set; }
        public double LeverMaxItem { get; set; }
        public double LeverAmountHordes { get; set; }
        public double LeverDanger { get; set; }
        public double LeverAccessibility { get; set; }

        public MixPool(Monsters monsters, Map currentMap, Delegate callback)
        {
            this.monsters = monsters;
            this.callback = callback;

            originalMap = currentMap.CloneJson() as Map;

            InitialPopulation = 30;
            GenerationLimit = 50;
            MutationPercentage = 0.4;
            CrossOverPercentage = 0.8;
            ElitismPercentage = 10;

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
            guidelineFitness = new Guideline(cells, areaManager, MaxMonsters, MaxItens, HordesPercentage)
            {
                MaxItemsLever = LeverMaxItem,
                MaxMonstersLever = LeverMaxMonster,
                AmountHordesLever = LeverAmountHordes,
                DangerLever = LeverDanger,
                AccessibilityLever = LeverAccessibility
            };

            double total = GuidelinePercentage + UserPercentage + InnovationPercentage;
            guidelineLevel = GuidelinePercentage / total;
            ConvergenceLevel = UserPercentage / total;
            innovationLevel = InnovationPercentage / total;
            
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
            double convFit = 0.0;
            double innoFit = 0.0;
            double guidFit = 0.0;

            convFit = convergenceFitness.CalculateFitness(chromosome);
            innoFit = 1.0 - convFit;
            if (guidelineLevel != 0.0) guidFit = guidelineFitness.CalculateFitness(chromosome);
            
            totalFitness =  guidelineLevel * guidFit +
                            ConvergenceLevel * convFit +
                            innovationLevel * innoFit;

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
