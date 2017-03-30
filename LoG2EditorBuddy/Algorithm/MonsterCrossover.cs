using GAF;
using GAF.Operators;
using GAF.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    class MonsterCrossover : IGeneticOperator
    {
        private int MAP_WIDTH = 32; 
        private int MAP_HEIGHT = 32; 

        private ReplacementMethod replacementMethod;
        private CrossoverType crossoverType;
        private double crossoverProbability;

        public event Crossover.CrossoverCompleteHandler OnCrossoverComplete;

        int numberOfEvaluations = 0;
        public bool Enabled { get; set; }
        public bool AllowDuplicates { get; set; }

        public MonsterCrossover(int mapWidth, int mapHeight, ReplacementMethod replacementMethod, CrossoverType crossoverType, double crossoverProbability, bool allowDuplicates)
        {
            this.MAP_WIDTH = mapWidth;
            this.MAP_HEIGHT = mapHeight;
            this.replacementMethod = replacementMethod;
            this.crossoverType = crossoverType;
            this.crossoverProbability = crossoverProbability;
            this.AllowDuplicates = allowDuplicates;

        }

        public int GetOperatorInvokedEvaluations()
        {
            return numberOfEvaluations;
        }

        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
        {
            numberOfEvaluations = 0;

            if (newPopulation == null)
                newPopulation = new Population(0, 0, currentPopulation.ReEvaluateAll, currentPopulation.LinearlyNormalised, currentPopulation.ParentSelectionMethod);
            if (!Enabled)
                return;
            int numberOfElites = 0;

            if (replacementMethod == ReplacementMethod.DeleteLast)
            {
                newPopulation.Solutions.Clear();
                newPopulation.Solutions.AddRange(currentPopulation.Solutions);
            }
            else
            {
                List<Chromosome> elites = currentPopulation.GetElites();
                numberOfElites = elites.Count;
                //numberOfElites = Enumerable.Count<Chromosome>((IEnumerable<Chromosome>)elites);
                if (elites != null && numberOfElites > 0)
                    newPopulation.Solutions.AddRange(elites);
            }
            int currentPopulationSize = currentPopulation.Solutions.Count;
            int numberOfChildrenToGenerate = currentPopulationSize - numberOfElites;
            while (numberOfChildrenToGenerate > 0)
            {
                Chromosome c1, c2;
                List<Chromosome> list = currentPopulation.SelectParents();
                Chromosome p1 = list[0];
                Chromosome p2 = list[1];
                CrossoverData crossoverResult = PerformCrossover(p1, p2, out c1, out c2);
                if (OnCrossoverComplete != null)
                    OnCrossoverComplete(this, new CrossoverEventArgs(crossoverResult));

                if(numberOfChildrenToGenerate > 1)
                {
                    AddChild(c1, currentPopulation, ref newPopulation, fitnessFunctionDelegate);
                    numberOfChildrenToGenerate--;
                    AddChild(c2, currentPopulation, ref newPopulation, fitnessFunctionDelegate);
                    numberOfChildrenToGenerate--;
                }
                else
                {
                    AddChild(c1, currentPopulation, ref newPopulation, fitnessFunctionDelegate);
                    numberOfChildrenToGenerate--;
                }
            }
        }

        private CrossoverData PerformCrossover(Chromosome p1, Chromosome p2, out Chromosome c1, out Chromosome c2)
        {
            CrossoverData crossoverData = CreateCrossoverData();

            List<Gene> genes1 = ListClone(p1.Genes);
            List<Gene> genes2 = ListClone(p2.Genes);

            int parent1NumberGenes = genes1.Count;
            int parent2NumberGenes = genes2.Count;

            if (parent1NumberGenes != parent2NumberGenes)
                throw new ArgumentException("Parent chromosomes are not the same length.");

            if (RandomProvider.GetThreadRandom().NextDouble() <= crossoverProbability)
            {
                //iterate all the replacement points
                foreach (int point in crossoverData.Points) //FIXME: tenho de meter só para monstros. Acho que tenho de verificar se é um tile walkable.
                {
                    Cell cell1 = (Cell) genes1[point].ObjectValue;
                    Cell cell2 = (Cell) genes2[point].ObjectValue;

                    Monster m1 = cell1.Monster;
                    Monster m2 = cell2.Monster;

                    cell1.Monster = m2;
                    cell2.Monster = m1;

                    genes1[point].ObjectValue = cell1;
                    genes2[point].ObjectValue = cell2;
                }
            }

            if (genes1.Count != parent1NumberGenes || genes2.Count != parent2NumberGenes)
                throw new ChromosomeCorruptException("Chromosome is corrupt!");
            c1 = new Chromosome(genes1);
            c2 = new Chromosome(genes2);

            return crossoverData;
        }

        internal CrossoverData CreateCrossoverData()
        {
            CrossoverData crossoverData = new CrossoverData();

            switch (crossoverType)
            {
                case CrossoverType.TwoByTwoSquare:
                    {
                        int rx, ry;
                        rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 2);
                        ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 2);

                        for (int yOffset = 0; yOffset < 2; yOffset++)
                            for (int xOffset = 0; xOffset < 2; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case CrossoverType.ThreeByThreeSquare:
                    {
                        int rx, ry;
                        rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 3);
                        ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 3);

                        for (int yOffset = 0; yOffset < 3; yOffset++)
                            for (int xOffset = 0; xOffset < 3; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case CrossoverType.FourByFourSquare:
                    {
                        int rx, ry;
                        rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 4);
                        ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 4);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case CrossoverType.FourByFourCircle:
                    {
                        int rx, ry;
                        do
                        {
                            rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1);
                            ry = RandomProvider.GetThreadRandom().Next(1, MAP_HEIGHT - 1);
                        }
                        while (rx > 28 || ry > 28);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                if ((yOffset == 0 && xOffset == 0) || (yOffset == 0 && xOffset == 3) || (yOffset == 3 && xOffset == 0) || (yOffset == 3 && xOffset == 3))
                                    continue;
                                else
                                    crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;

                case CrossoverType.Triangle: //size??

                    break;

                case CrossoverType.Dispersion:

                    break;

                default:
                    {
                        int rx, ry;

                        rx = RandomProvider.GetThreadRandom().Next(0, MAP_WIDTH - 1 - 4);
                        ry = RandomProvider.GetThreadRandom().Next(0, MAP_HEIGHT - 1 - 4);

                        for (int yOffset = 0; yOffset < 4; yOffset++)
                            for (int xOffset = 0; xOffset < 4; xOffset++)
                                crossoverData.Points.Add((MAP_HEIGHT * (ry + yOffset) + (rx + xOffset)));
                    }
                    break;
            }
            return crossoverData;
        }

        private bool AddChild(Chromosome child, Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunction)
        {
            int currentPopulationSize = currentPopulation.Solutions.Count;

            if (child.Genes != null && (this.AllowDuplicates || !newPopulation.SolutionExists(child)))
            {
                if (replacementMethod == ReplacementMethod.DeleteLast)
                {
                    child.Evaluate(fitnessFunction);
                    numberOfEvaluations++;
                    if (child.Fitness > currentPopulation.MinimumFitness)
                    {
                        newPopulation.Solutions.Add(child);
                        if (newPopulation.Solutions.Count > currentPopulationSize)
                        {
                            newPopulation.Solutions.Sort();
                            newPopulation.Solutions.RemoveAt(currentPopulationSize - 1);
                        }
                        return true;
                    }
                }
                else
                {
                    newPopulation.Solutions.Add(child);
                    return true;    
                }
            }
              
            return false;
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
