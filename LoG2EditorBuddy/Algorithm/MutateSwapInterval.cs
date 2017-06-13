using GAF;
using GAF.Extensions;
using GAF.Operators;
using GAF.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm
{
    class MutateSwapInterval : IGeneticOperator
    {
        private int Interval;
        private int invokedEvaluations;
        public bool Enabled { get; set; }
        public double MutationProbability { get; private set; }
        public bool AllowDuplicates { get; private set; }
        public Population NewPopulation { get; private set; }

        public MutateSwapInterval(double mutationProbability, int interval) : this(mutationProbability, interval, false)
        {

        }

        public MutateSwapInterval(double mutationProbability, int interval, bool allowDuplicates)
        {
            MutationProbability = mutationProbability;
            AllowDuplicates = allowDuplicates;
            Interval = interval;

            Enabled = true;
        }

        public int GetOperatorInvokedEvaluations()
        {
            return invokedEvaluations;
        }

        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate)
        {
            if (currentPopulation.Solutions == null || currentPopulation.Solutions.Count == 0)
            {
                throw new ArgumentException("There are no Solutions in the current Population.");
            }

            if (newPopulation == null)
            {
                newPopulation = new Population(0, 0, currentPopulation.ReEvaluateAll, currentPopulation.LinearlyNormalised, currentPopulation.ParentSelectionMethod);
            }

            if (!Enabled)
                return;

            invokedEvaluations = 0;
            NewPopulation = newPopulation;

            //copy everything accross including elite
            newPopulation.Solutions.Clear();
            newPopulation.Solutions.AddRange(currentPopulation.Solutions);

            var solutionsToProcess = newPopulation.GetNonElites();

            foreach (var chromosome in solutionsToProcess)
            {

                if (chromosome == null || chromosome.Genes == null)
                {
                    throw new ArgumentException("The Chromosome is either null or the Chromosome's Genes are null.");
                }

                Mutate(chromosome);
            }
        }

        private void Mutate(Chromosome child)
        {
            Chromosome childToMutate = null;

            if (AllowDuplicates)
            {
                childToMutate = child;
            }
            else
            {

                //We have to clone the chromosome before we mutate it as it may
                //not be usable i.e. if it is a duplicate if we didn't clone it 
                //and we created a duplicate through mutation we would have to 
                //undo the mutation. This way is easier.
                childToMutate = new Chromosome(child.Genes);
            }

            //call the default mutation behaviour
            //cannot mutate elites or else we will ruin them
            if (childToMutate.IsElite)
                return;

            if (childToMutate == null || childToMutate.Genes == null)
            {
                throw new ArgumentException("The Chromosome is either null or the Chromosomes Genes are null.");
            }

            //check probability by generating a random number between zero and one and if 
            //this number is less than or equal to the given mutation probability 
            //e.g. 0.001 then the bit value is changed.
            var rd = RandomProvider.GetThreadRandom().NextDouble();
            
            if (rd <= MutationProbability)
            {
                SwapGenes(childToMutate);
            }

            //only add the mutated chromosome if it does not exist otherwise do nothing
            if (!AllowDuplicates && !NewPopulation.SolutionExists(childToMutate))
            {
                //swap existing genes for the mutated onese
                child.Genes.Clear();
                child.Genes.AddRangeCloned(childToMutate.Genes);
            }
        }

        private void SwapGenes(Chromosome childToMutate)
        {
            CrossoverData data = CreateCrossoverData(childToMutate.Genes.Count);

            for(int i = 0; i < Interval; i++)
            {
                Gene g1 = childToMutate.Genes[data.Points[0] + i];
                Gene g2 = childToMutate.Genes[data.Points[1] + i];

                childToMutate.Genes[data.Points[0] + i] = g2;
                childToMutate.Genes[data.Points[1] + i] = g1;
            }
        }

        internal CrossoverData CreateCrossoverData(int chromosomeLength)
        {
            var result = new CrossoverData();

            int point1;
            int point2;
            do
            {
                point2 = RandomProvider.GetThreadRandom().Next(1, chromosomeLength / Interval) * Interval;
                point1 = RandomProvider.GetThreadRandom().Next(1, chromosomeLength / Interval) * Interval;
            } while (point2 == point1);

            result.Points.Add(System.Math.Min(point2, point1));
            result.Points.Add(System.Math.Max(point2, point1));

            return result;
        }
    }
}
