using GAF;
using GAF.Threading;
using Log2CyclePrototype.LoG2API.Elements;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Algorithm
{
    class MonsterMutate : IGeneticOperator
    {
        public bool Enabled {get; set;}

        public double MutationProbability { get; set; }

        public int GetOperatorInvokedEvaluations()
        {
            return 0;
        }

        public MonsterMutate(double mutationProbability)
        {
            MutationProbability = mutationProbability;
            Enabled = true;
        }

        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate)
        {
            if (newPopulation == null)
            {
                Console.WriteLine("MonsterMutate: Null new population");
                throw new NullReferenceException();
                newPopulation = new Population(0, 0, currentPopulation.ReEvaluateAll, currentPopulation.LinearlyNormalised, currentPopulation.ParentSelectionMethod);
            }
            if (!Enabled)
                return;

            newPopulation.Solutions.Clear();
            newPopulation.Solutions.AddRange(currentPopulation.Solutions);

            for (int i = 0; i < newPopulation.Solutions.Count; i++) 
            {
                Chromosome chromosome = newPopulation.Solutions[i];
                double mutationProbability = MutationProbability < 0.0 ? 0.0 : MutationProbability;
                if (chromosome == null)
                    throw new ChromosomeException("The Cromosome is null.");
                if (!chromosome.IsElite)
                {
                    if (chromosome.Genes == null)
                        throw new GeneException("The Chromosomes Genes are null.");
                    if (RandomProvider.GetThreadRandom().NextDouble() <= mutationProbability)
                        newPopulation.Solutions[i] = Mutate(chromosome);
                }
            }
       
        }

        private Chromosome Mutate(Chromosome chromosome)
        {
            int swapPoint1 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count - 1);
            int swapPoint2 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count - 1);

            //Evitar trocar o mesmo gene
            while(swapPoint1 == swapPoint2)
            {
                swapPoint2 = RandomProvider.GetThreadRandom().Next(chromosome.Genes.Count - 1);
            }

            List<Gene> genes = new List<Gene>();

            for(int i = 0; i < chromosome.Genes.Count; i++)
            {
                genes.Add(chromosome.Genes[i].DeepClone());
            }

            LoG2API.Cell cell1 = (LoG2API.Cell)genes[swapPoint1].ObjectValue;
            LoG2API.Cell cell2 = (LoG2API.Cell)genes[swapPoint2].ObjectValue;


            cell1 = cell1.CloneJson() as LoG2API.Cell;
            cell2 = cell2.CloneJson() as LoG2API.Cell;

            Monster m1 = cell1.Monster;
            Monster m2 = cell2.Monster;

            //Monster m1 = new Monster("crab", cell1.X, cell1.Y, 0, 0, "ddd"); //FIXME: meter isto bem
            //Monster m2 = new Monster("crab", cell2.X, cell2.Y, 0, 0, "dd"); // FIXME:

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

            genes[swapPoint1] = new Gene(cell1);
            genes[swapPoint2] = new Gene(cell2);

            return new Chromosome(genes);
        }
    }
}
