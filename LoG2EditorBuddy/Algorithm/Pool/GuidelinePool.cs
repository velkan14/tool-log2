﻿using GAF;
using GAF.Operators;
using Povoater.Layers;
using Povoater.LoG2API;
using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Povoater.Algorithm.Fitness;

namespace Povoater.Algorithm
{
    class GuidelinePool : HasStuff
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


        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public double HordesPercentage { get; set; }

        public double LeverMaxMonster { get; set; }
        public double LeverMaxItem { get; set; }
        public double LeverAmountHordes { get; set; }
        public double LeverDanger { get; set; }
        public double LeverAccessibility { get; set; }

        Population population;
        Guideline fitness;

        public GuidelinePool(Monsters monsters, Map currentMap, Delegate callback, AreaManager areaManager, int maxMonsters, int maxItens, double hordesPercentage)
        {
            this.monsters = monsters;
            this.callback = callback;

            this.MaxMonsters = maxMonsters;
            this.MaxItens = maxItens;
            this.HordesPercentage = hordesPercentage;

            originalMap = currentMap.CloneJson() as Map;

            InitialPopulation = 30;
            GenerationLimit = 50;
            MutationPercentage = 0.4;
            CrossOverPercentage = 0.8;
            ElitismPercentage = 10;

            running = false;
            HasSolution = false;

            cells = originalMap.SpawnCells;

            fitness = new Guideline(cells, areaManager, MaxMonsters, MaxItens, HordesPercentage)
            {
                MaxItemsLever = LeverMaxItem,
                MaxMonstersLever = LeverMaxMonster,
                AmountHordesLever = LeverAmountHordes,
                DangerLever = LeverDanger,
                AccessibilityLever = LeverAccessibility
            };

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            population = new Population(InitialPopulation, cells.Count * ChromosomeUtils.NUMBER_GENES, true, true, ParentSelectionMethod.FitnessProportionateSelection);
        }

        public GuidelinePool(Monsters monsters, Map currentMap, Delegate callback, AreaManager areaManager, int maxMonsters, int maxItens, double hordesPercentage, Population pop) : this(monsters, currentMap, callback, areaManager, maxMonsters, maxItens, hordesPercentage)
        {
            population.Solutions.Clear();

            population.Solutions.AddRange(pop.GetTop(10));

            for(int i = 0; i < 20; i++)
            {
                population.Solutions.Add(new Chromosome(cells.Count * ChromosomeUtils.NUMBER_GENES));
            }
        }

        public void Run()
        {
            if (running) return;
            
            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, ChromosomeUtils.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, fitness.CalculateFitness);

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
            monsters.Progress(1, 100 * currentGeneration / GenerationLimit);
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
