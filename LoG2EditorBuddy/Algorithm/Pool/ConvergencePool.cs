﻿using GAF;
using GAF.Operators;
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
    class ConvergencePool : HasStuff
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
        private Population population;
        ConvergenceFitness fitness;
        
        public ConvergencePool(Monsters monsters, Map currentMap, Delegate callback)
        {
            this.monsters = monsters;
            this.callback = callback;

            originalMap = currentMap.CloneJson() as Map;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 0.4;
            CrossOverPercentage = 0.8;
            ElitismPercentage = 10;

            running = false;
            HasSolution = false;


            cells = originalMap.SpawnCells;

            Chromosome chrom = ChromosomeUtils.ChromosomeFromMap(originalMap);

            string binaryString = chrom.ToBinaryString();

            fitness = new ConvergenceFitness(cells, binaryString);

            population = new Population(InitialPopulation, cells.Count * ChromosomeUtils.NUMBER_GENES, true, true, ParentSelectionMethod.FitnessProportionateSelection);

            population.Solutions.Clear();

            for (int i = 0; i < 5; i++)
            {
                population.Solutions.Add(new Chromosome(binaryString));
            }
            for (int i = 0; i < 25; i++)
            {
                population.Solutions.Add(new Chromosome(cells.Count * ChromosomeUtils.NUMBER_GENES));
            }
        }

        public void Run()
        {
            if (running) return;


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the mutation operator
            var mutate = new MutateInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES);

            var swap = new MutateSwapInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES);
            //create the GA
            var ga = new GeneticAlgorithm(population, fitness.CalculateFitness);

            //hook up to some useful events
            ga.OnRunComplete += OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            //ga.Operators.Add(mutate);
            //ga.Operators.Add(swap);
            ga.Operators.Add(new BinaryMutate(MutationPercentage));
            ga.Operators.Add(new CrossoverIndex(CrossOverPercentage, ChromosomeUtils.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement));


            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        

        protected double Invert(double x)
        {
            return 1.0 - x;
        }        

        protected bool AreEquals(CellStruct c, CellStruct originalCell)
        {
            if (HasArmor(c) && HasArmor(originalCell) ||
                HasResource(c) && HasResource(originalCell) ||
                HasWeapon(c) && HasWeapon(originalCell) ||
                HasMummy(c) && HasMummy(originalCell) ||
                HasTurtle(c) && HasTurtle(originalCell) ||
                HasSkeleton(c) && HasSkeleton(originalCell) ||
                IsEmpty(c) && IsEmpty(originalCell))
            {
                return true;
            }
            return false;
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
            monsters.Progress(0, 100 * currentGeneration / GenerationLimit);
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
