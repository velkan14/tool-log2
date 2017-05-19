// Decompiled with JetBrains decompiler
// Type: GAF.Operators.SwapMutate
// Assembly: GAF, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F9FE644E-0C8A-452C-8CAF-3D334B3B38D3
// Assembly location: D:\Programming\Repositories\IST\Tese\GAexample\ObjectBasedGene\bin\Debug\GAF.dll

using GAF;
using GAF.Threading;
using EditorBuddyMonster;
using System;
using System.Collections.Generic;
using System.Threading;
using EditorBuddyMonster.Utilities;
using System.Diagnostics;
using EditorBuddyMonster.LoG2API;

namespace GAF.Operators
{
    public class CustomSwapMutate : IGeneticOperator
    {
        private readonly object _syncLock = new object();
        private double _mutationProbabilityS;

        public bool Enabled { get; set; }

        public double MutationProbability
        {
            get
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    return this._mutationProbabilityS;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
            set
            {
                object obj = this._syncLock;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(obj, ref lockTaken);
                    this._mutationProbabilityS = value;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(obj);
                }
            }
        }

        internal CustomSwapMutate()
            : this(1.0)
        {
        }

        public CustomSwapMutate(double mutationProbability)
        {
            this._mutationProbabilityS = mutationProbability;
            this.Enabled = true;
        }

        public virtual void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
        {
            if (newPopulation == null)
                newPopulation = new Population(0, 0, currentPopulation.ReEvaluateAll, currentPopulation.LinearlyNormalised, currentPopulation.ParentSelectionMethod);
            if (!this.Enabled)
                return;
            newPopulation.Solutions.Clear();
            newPopulation.Solutions.AddRange((IEnumerable<Chromosome>)currentPopulation.Solutions);
            foreach (Chromosome chromosome in newPopulation.Solutions)
            {
                double mutationProbability = this.MutationProbability < 0.0 ? 0.0 : this.MutationProbability;
                if (chromosome == null)
                    throw new ChromosomeException("The Cromosome is null.");
                if (!chromosome.IsElite)
                {
                    if (chromosome.Genes == null)
                        throw new GeneException("The Chromosomes Genes are null.");
                    if (RandomProvider.GetThreadRandom().NextDouble() <= mutationProbability)
                        this.Mutate(chromosome, mutationProbability);
                }
            }
        }

        protected virtual void Mutate(Chromosome chromosome, double mutationProbability)
        {
            List<int> swapPoints = this.GetSwapPoints(chromosome);
            this.Mutate(chromosome, swapPoints[0], swapPoints[1]);
        }

        internal void Mutate(Chromosome chromosome, int first, int second)
        {

            var c1 = (Cell)chromosome.Genes[first].ObjectValue;
            var c2 = (Cell)chromosome.Genes[second].ObjectValue;

            //int tmpC1Type = c1.CellType;
            //bool tmpC1Walkable = c1.IsWalkable;
            //int tmpC2Type = c2.CellType;
            //bool tmpC2Walkable = c2.IsWalkable;
            //c1.CellType = tmpC2Type;
            //c1.IsWalkable = tmpC2Walkable;
            //c2.CellType = tmpC1Type;
            //c2.IsWalkable = tmpC1Walkable;

            ////store coords
            //Tuple<int, int> coordsP1 = new Tuple<int, int>(((Cell)chromosome.Genes[first].ObjectValue).X, ((Cell)chromosome.Genes[first].ObjectValue).Y);
            //Tuple<int, int> coordsP2 = new Tuple<int, int>(((Cell)chromosome.Genes[second].ObjectValue).X, ((Cell)chromosome.Genes[second].ObjectValue).Y);
            //var s1 = ((Cell)chromosome.Genes[first].ObjectValue).CloneJson();
            //s1.X = coordsP2.Item1;
            //s1.Y = coordsP2.Item2;
            //var s2 = ((Cell)chromosome.Genes[second].ObjectValue).CloneJson();
            //s2.X = coordsP1.Item1;
            //s2.Y = coordsP1.Item2;

            Cell s1 = c1.CloneJson() as Cell;
            s1.CellType = c2.CellType;
            s1.IsWalkable = c2.IsWalkable;
            Cell s2 = c2.CloneJson() as Cell;
            s2.CellType = c1.CellType;
            s2.IsWalkable = c1.IsWalkable;
            //Debug.WriteLine("Trading " + s1.CellType + "for " + s2.CellType);
            chromosome.Genes[first] = new Gene(s1);
            chromosome.Genes[second] = new Gene(s2);

            //Gene gene = chromosome.Genes[first];
            //chromosome.Genes[first] = chromosome.Genes[second];
            //chromosome.Genes[second] = gene;


        }

        internal List<int> GetSwapPoints(Chromosome chromosome)
        {
            List<int> list = new List<int>();
            int num1 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count - 1);
            int num2 = 0;
            while (num1 == num2 || num2 == 0)
                num2 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count - 1);
            list.Add(num1);
            list.Add(num2);
            return list;
        }

        public int GetOperatorInvokedEvaluations()
        {
            return 0;
        }
    }
}
