using GAF;
using GAF.Extensions;
using GAF.Operators;
using EditorBuddyMonster.Algorithm;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster
{
    class InnovationPool : HasStuff
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        private bool running;
        private Delegate callback;

        private Map originalMap;
        private List<Cell> cells;
        private Monsters monsters;

        public bool HasSolution { get; private set; }
        public Population Solution { get; private set; }

        List<CellStruct> mapCells = new List<CellStruct>();

        public InnovationPool(Monsters monsters)
        {
            this.monsters = monsters;

            InitialPopulation = 30;
            GenerationLimit = 30;
            MutationPercentage = 0.35;
            CrossOverPercentage = 0.4;
            ElitismPercentage = 10;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap.CloneJson() as Map;
            cells = originalMap.SpawnCells;

            this.callback = callback;

            Chromosome chrom = ChromosomeUtils.ChromosomeFromMap(originalMap, true);

            string binaryString = chrom.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES_TOTAL + ChromosomeUtils.NUMBER_GENES_ID, ChromosomeUtils.NUMBER_GENES);
                string sID = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES_TOTAL, ChromosomeUtils.NUMBER_GENES_ID);
                int type = Convert.ToInt32(s, 2);
                int id = Convert.ToInt32(sID, 2);

                mapCells.Add(new CellStruct(id, type, cells[i].X, cells[i].Y));
            }

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * ChromosomeUtils.NUMBER_GENES_TOTAL, true, true);

            population.Solutions.Clear();

            for (int i = 0; i < InitialPopulation; i++)
            {
                population.Solutions.Add(new Chromosome(binaryString));
            }

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the mutation operator
            var mutate = new MutateInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES, ChromosomeUtils.NUMBER_GENES_ID);

            var swap = new MutateSwapInterval(MutationPercentage, ChromosomeUtils.NUMBER_GENES_TOTAL);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //hook up to some useful events
            ga.OnRunComplete += OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(mutate);
            ga.Operators.Add(swap);
            

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            
            List<CellStruct> listMonsters = new List<CellStruct>();
            
            string binaryString = chromosome.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES_TOTAL + ChromosomeUtils.NUMBER_GENES_ID, ChromosomeUtils.NUMBER_GENES);
                string sID = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES_TOTAL, ChromosomeUtils.NUMBER_GENES_ID);

                int type = Convert.ToInt32(s, 2);
                int id = Convert.ToInt32(sID, 2);
                CellStruct tmp = new CellStruct(id, type, cells[i].X, cells[i].Y);

                if (HasMonster(tmp))
                {
                    listMonsters.Add(tmp);
                }
                
            }

            foreach(CellStruct c in listMonsters)
            {
                CellStruct originalCell = mapCells.FirstOrDefault(x => x.id == c.id);

                fitness += Equality(c, originalCell) * Distance(c, mapCells);
            }

            fitness = fitness / listMonsters.Count;
            
            return fitness;
        }

        private double Distance(CellStruct c, List<CellStruct> originalCells)
        {
            int distance = FloodFill(c, originalCells);
            return (double) distance / (double) originalCells.Count;
        }

        private static int FloodFill(CellStruct startCell, List<CellStruct> listCells)
        {
            int tileTraversed = 0;

            for (int i = 0; i < listCells.Count; i++)
            {
                listCells[i].visited = false;
            }

            ListQueue<CellStruct> queue = new ListQueue<CellStruct>();
            CellStruct firstCell = listCells.FirstOrDefault(c => c.x == startCell.x && c.y == startCell.y);

            if (startCell.id == firstCell.id) return 0;

            firstCell.visited = true;
            queue.Enqueue(firstCell);

            while (queue.Count != 0)
            {
                CellStruct node = queue.Dequeue();
                tileTraversed++;

                //west
                if (HasCellUnvisited(node.x - 1, node.y, listCells))
                {
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x - 1 && c.y == node.y);

                    if (openNode.id == startCell.id) return tileTraversed;
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //east
                if (HasCellUnvisited(node.x + 1, node.y, listCells))
                {
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x + 1 && c.y == node.y);
                    if (openNode.id == startCell.id) return tileTraversed;
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //north
                if (HasCellUnvisited(node.x, node.y - 1, listCells))
                {
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y - 1);
                    if (openNode.id == startCell.id) return tileTraversed;
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //south
                if (HasCellUnvisited(node.x, node.y + 1, listCells))
                {
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y + 1);
                    if (openNode.id == startCell.id) return tileTraversed;
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
            }
            return tileTraversed;
        }

        private static bool HasCellUnvisited(int x, int y, List<CellStruct> listCells)
        {
            foreach (CellStruct cs in listCells)
            {
                if (cs.x == x && cs.y == y && cs.visited == false)
                {
                    return true;
                }
            }
            return false;
        }

        private double Equality(CellStruct c, CellStruct originalCell)
        {
            if (SameType(c, originalCell) && !AreEquals(c, originalCell))
            {
                return 1.0;
            }
            return 0.0;
        }

        private bool SameType(CellStruct c, CellStruct originalCell)
        {
            if(HasMonster(c) && HasMonster(originalCell) ||
               HasItem(c) && HasItem(originalCell))
            {
                return true;
            }
            return false;
        }

        private bool AreEquals(CellStruct c, CellStruct originalCell)
        {
            if(HasArmor(c) && HasArmor(originalCell) ||
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
            monsters.Progress(2, 100 * currentGeneration / GenerationLimit);
            return currentGeneration > GenerationLimit;
        }

        private void OnRunComplete(object sender, GaEventArgs e)
        {
            Solution = e.Population;

            running = false;
            HasSolution = true;

            callback.DynamicInvoke();
        }
        
    }
}
