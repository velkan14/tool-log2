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
    class MutateInterval : HasStuff, IGeneticOperator
    {
        private int Interval;
        private int invokedEvaluations;
        public bool Enabled { get; set;}
        public double MutationProbability { get; private set; }
        public bool AllowDuplicates { get; private set; }
        public Population NewPopulation { get; private set; }

        public MutateInterval(double mutationProbability, int interval) : this(mutationProbability, interval, false)
        {
            
        }

        public MutateInterval(double mutationProbability, int interval, bool allowDuplicates)
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

            string binaryString = childToMutate.ToBinaryString();

            int count = binaryString.Length / Interval;

            for (int i = 0; i < count; i++)
            {
                string s = binaryString.Substring(i * Interval, Interval);

                int type = Convert.ToInt32(s, 2);

                CellStruct tmpCell = new CellStruct(type, 0, 0);
                if (HasMonster(tmpCell))
                {
                    var rd = RandomProvider.GetThreadRandom().NextDouble();
                    if (rd <= MutationProbability)
                    {
                        int index = i * Interval;

                        var rdType = RandomProvider.GetThreadRandom().Next(0,3);

                        if(rdType == 0)
                        {
                            // 1 : 00000001
                            childToMutate.Genes[index].ObjectValue = false;
                            childToMutate.Genes[index + 1].ObjectValue = false;
                            childToMutate.Genes[index + 2].ObjectValue = false;
                            childToMutate.Genes[index + 3].ObjectValue = false;

                            childToMutate.Genes[index + 4].ObjectValue = false;
                            childToMutate.Genes[index + 5].ObjectValue = false;
                            childToMutate.Genes[index + 6].ObjectValue = false;
                            childToMutate.Genes[index + 7].ObjectValue = true;
                        } else if(rdType == 1)
                        {
                            // 12 : 00001100
                            childToMutate.Genes[index].ObjectValue = false;
                            childToMutate.Genes[index + 1].ObjectValue = false;
                            childToMutate.Genes[index + 2].ObjectValue = false;
                            childToMutate.Genes[index + 3].ObjectValue = false;

                            childToMutate.Genes[index + 4].ObjectValue = true;
                            childToMutate.Genes[index + 5].ObjectValue = true;
                            childToMutate.Genes[index + 6].ObjectValue = false;
                            childToMutate.Genes[index + 7].ObjectValue = false;
                        }
                        else
                        {
                            // 23 : 00010111
                            childToMutate.Genes[index].ObjectValue = false;
                            childToMutate.Genes[index + 1].ObjectValue = false;
                            childToMutate.Genes[index + 2].ObjectValue = false;
                            childToMutate.Genes[index + 3].ObjectValue = true;

                            childToMutate.Genes[index + 4].ObjectValue = false;
                            childToMutate.Genes[index + 5].ObjectValue = true;
                            childToMutate.Genes[index + 6].ObjectValue = true;
                            childToMutate.Genes[index + 7].ObjectValue = true;
                        }
                    }
                }
            }

            /*for (int i = 0; i < childToMutate.Genes.Count; i++)
            {
                if (IsMarked(i, binaryString))
                {
                    //check probability by generating a random number between zero and one and if 
                    //this number is less than or equal to the given mutation probability 
                    //e.g. 0.001 then the bit value is changed.
                    var rd = RandomProvider.GetThreadRandom().NextDouble();

                    if (rd <= MutationProbability)
                    {
                        MutateGene(childToMutate.Genes[i]);
                    }
                }
            }*/

            //only add the mutated chromosome if it does not exist otherwise do nothing
            if (!AllowDuplicates && !NewPopulation.SolutionExists(childToMutate))
            {

                //swap existing genes for the mutated onese
                child.Genes.Clear();
                child.Genes.AddRangeCloned(childToMutate.Genes);
            }
        }

        private bool IsMarked(int i, string binaryString)
        {
            int index = 0;
            do
            {
                index += Interval;
            } while (index < i);
            index -= Interval;

            string s = binaryString.Substring(index, Interval);
            if(!IsEmpty(new CellStruct(Convert.ToInt32(s, 2), 0, 0)))
            {
                return true;
            }
            return false;
        }

        private void MutateGene(Gene gene)
        {
            if (gene.GeneType == GeneType.Object)
            {
                throw new OperatorException("Genes with a GeneType of Object cannot be mutated by the BinaryMutate operator.");
            }

            switch (gene.GeneType)
            {
                case GeneType.Binary:
                    {
                        gene.ObjectValue = !(bool)gene.ObjectValue;
                        break;
                    }
                case GeneType.Real:
                    {
                        gene.ObjectValue = (double)gene.ObjectValue * -1;
                        break;
                    }
                case GeneType.Integer:
                    {
                        gene.ObjectValue = (int)gene.ObjectValue * -1;
                        break;
                    }
            }
        }
    }
}
