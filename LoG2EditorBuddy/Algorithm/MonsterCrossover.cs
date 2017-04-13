using GAF;
using GAF.Operators;
using GAF.Threading;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.LoG2API.Elements;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;

namespace Log2CyclePrototype
{
    class MonsterCrossover : IGeneticOperator
    {
        private ReplacementMethod replacementMethod;
        private CrossoverType crossoverType;
        private double crossoverProbability;

        public event Crossover.CrossoverCompleteHandler OnCrossoverComplete;

        int numberOfEvaluations = 0;
        public bool Enabled { get; set; }
        public bool AllowDuplicates { get; set; }

        Population currentPopulation;

        public MonsterCrossover(ReplacementMethod replacementMethod, CrossoverType crossoverType, double crossoverProbability, bool allowDuplicates)
        {
            this.replacementMethod = replacementMethod;
            this.crossoverType = crossoverType;
            this.crossoverProbability = crossoverProbability;
            this.AllowDuplicates = allowDuplicates;
            this.Enabled = true;
        }

        public int GetOperatorInvokedEvaluations()
        {
            return numberOfEvaluations;
        }

        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
        {
            numberOfEvaluations = 0;
            this.currentPopulation = currentPopulation;

            if (newPopulation == null)
            {
                Console.WriteLine("MonsterCrossover: Null new population");
                throw new NullReferenceException();
            }

            if (!Enabled)
                return;
            if (replacementMethod == ReplacementMethod.GenerationalReplacement)
            {
                newPopulation.Solutions.Clear();

                List<Chromosome> elites = currentPopulation.GetElites();
                List<Chromosome> nonElites = currentPopulation.GetNonElites();

                newPopulation.Solutions.AddRange(elites);

                int childGenerated = 0;

                while(childGenerated < nonElites.Count){
                    List<Chromosome> parents = currentPopulation.SelectParents();

                    if (RandomProvider.GetThreadRandom().NextDouble() <= crossoverProbability)
                    {
                        Chromosome child1, child2;

                        var crossoverResult = PerformCrossover(parents[0], parents[1], out child1, out child2);

                        if (OnCrossoverComplete != null)
                        { 
                            var eventArgs = new CrossoverEventArgs(crossoverResult);
                            OnCrossoverComplete(this, eventArgs);
                        }

                        if (AllowDuplicates || !newPopulation.SolutionExists(child1))
                        {
                            newPopulation.Solutions.Add(child1);
                            childGenerated++;
                        }
                        if (childGenerated < nonElites.Count && (AllowDuplicates || !newPopulation.SolutionExists(child2)))
                        {
                            newPopulation.Solutions.Add(child2);
                            childGenerated++;
                        }
                    }
                    else
                    {
                        newPopulation.Solutions.Add(parents[0]);
                        childGenerated++;

                        if (childGenerated < nonElites.Count)
                        {
                            newPopulation.Solutions.Add(parents[1]);
                            childGenerated++;
                        }
                    }
                }
            }
            else if(replacementMethod == ReplacementMethod.DeleteLast)
            {
                throw new NotImplementedException("ReplacementMethod DeleteLast in MonsterCrossOver");
            }
        }

        private CrossoverData PerformCrossover(Chromosome p1, Chromosome p2, out Chromosome c1, out Chromosome c2)
        {
            CrossoverData crossoverData = new CrossoverData();
            int solutionSize = currentPopulation.Solutions[0].Count; // The solutions have all the same size, so we get the first one.
            List<Gene> genes1 = ListClone(p1.Genes);
            List<Gene> genes2 = ListClone(p2.Genes);

            switch (crossoverType)
            {
                case CrossoverType.SinglePoint:
                    {
                        int point = RandomProvider.GetThreadRandom().Next(1, solutionSize - 1);
                        crossoverData.Points.Add(point);
                        for(int i = 0; i < point; i++)
                        {
                            Cell cell1 = (Cell)genes1[i].ObjectValue;
                            Cell cell2 = (Cell)genes2[i].ObjectValue;

                            cell1 = cell1.CloneJson() as Cell;
                            cell2 = cell2.CloneJson() as Cell;

                            Monster m1 = cell1.Monster;
                            Monster m2 = cell2.Monster;

                            if(m1 != null)
                            {
                                m1.x = cell2.X;
                                m1.y = cell2.Y;
                            }

                            if(m2 != null)
                            {
                                m2.x = cell1.X;
                                m2.y = cell1.Y;
                            }

                            cell1.Monster = m2;
                            cell2.Monster = m1;

                            genes1[i] = new Gene(cell1);
                            genes2[i] = new Gene(cell2);
                        }
                    }
                    break;

                case CrossoverType.DoublePoint:
                    {
                        int point1 = RandomProvider.GetThreadRandom().Next(1, solutionSize - 2);
                        int point2 = RandomProvider.GetThreadRandom().Next(point1 + 1, solutionSize - 1);

                        for (int i = 0; i < point1; i++)
                        {
                            Cell cell1 = (Cell)genes1[i].ObjectValue;
                            Cell cell2 = (Cell)genes2[i].ObjectValue;

                            cell1 = cell1.CloneJson() as Cell;
                            cell2 = cell2.CloneJson() as Cell;

                            Monster m1 = cell1.Monster;
                            Monster m2 = cell2.Monster;

                            if (m1 != null)
                            {
                                m1.x = cell2.X;
                                m1.y = cell2.Y;
                            }

                            if (m2 != null)
                            {
                                m2.x = cell1.X;
                                m2.y = cell1.Y;
                            }

                            cell1.Monster = m2;
                            cell2.Monster = m1;


                            genes1[i] = new Gene(cell1);
                            genes2[i] = new Gene(cell2);
                        }

                        for (int i = point2; i < solutionSize; i++)
                        {
                            Cell cell1 = (Cell)genes1[i].ObjectValue;
                            Cell cell2 = (Cell)genes2[i].ObjectValue;

                            cell1 = cell1.CloneJson() as Cell;
                            cell2 = cell2.CloneJson() as Cell;

                            Monster m1 = cell1.Monster;
                            Monster m2 = cell2.Monster;

                            if (m1 != null)
                            {
                                m1.x = cell2.X;
                                m1.y = cell2.Y;
                            }

                            if (m2 != null)
                            {
                                m2.x = cell1.X;
                                m2.y = cell1.Y;
                            }

                            cell1.Monster = m2;
                            cell2.Monster = m1;

                            genes1[i] = new Gene(cell1);
                            genes2[i] = new Gene(cell2);
                        }
                    }
                    break;
            }

            if (genes1.Count != p1.Genes.Count || genes2.Count != p2.Genes.Count)
                throw new ChromosomeCorruptException("Chromosome is corrupt!");

            c1 = new Chromosome(genes1);
            c2 = new Chromosome(genes2);

            return crossoverData;
        }

        internal List<Gene> ListClone(List<Gene> list)
        {
            List<Gene> nl = new List<Gene>();
            foreach (var g in list)
                nl.Add(g.DeepClone());
            return nl;
        }

    }

}
