using EditorBuddyMonster.Layers;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm.Fitness
{
    class GuidelineMonster : HasStuff
    {
        protected List<Cell> cells;
        protected AreaManager areaManager;

        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public double HordesPercentage { get; set; }


        private delegate bool HasSomething(int x, int y, List<CellStruct> listCells);

        public GuidelineMonster(List<Cell> spawnCells, AreaManager areaManager, int maxMonsters, int maxItens, double hordesPercentage)
        {
            this.cells = spawnCells;
            this.areaManager = areaManager;
            this.MaxItens = maxItens;
            this.MaxMonsters = maxMonsters;
            this.HordesPercentage = hordesPercentage;
        }

        public double CalculateFitness(Chromosome chromosome)
        {
            double totalFitness = 0.0; // Value between 0 and 1. 1 is the fittest
            int numberMonsters = 0;
            //int numberItems = 0;
            double monsterFit = 1.0;
            //double itemFit = 1.0;

            string binaryString = chromosome.ToBinaryString();

            List<CellStruct> listCells = new List<CellStruct>();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * ChromosomeUtils.NUMBER_GENES, ChromosomeUtils.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);
                listCells.Add(new CellStruct(j, cells[i].X, cells[i].Y));
            }

            foreach (Area area in areaManager.AreaList)
            {
                Cell startCell = area.StartCell;
                List<Cell> cellsArea = area.Cells;
                double monsterDifficulty = 0.0;
                double areaMonsterFit = 0.0;

                foreach (Cell c in cellsArea)
                {
                    if (HasMonster(c.X, c.Y, listCells))
                    {
                        monsterDifficulty += GetMonsterDifficulty(c, area, listCells);
                        numberMonsters++;
                    }
                    /*else if (HasItem(c.X, c.Y, listCells))
                    {
                        itemFit *= GetItemFitness(c, area, listCells);
                        numberItems++;
                    }*/
                }

                double size = area.Size / 9.0;
                switch (area.Difficulty)
                {
                    case RoomDifficulty.Safe:
                        {
                            areaMonsterFit = Function(monsterDifficulty, 0, 0, size);
                        }
                        break;
                    case RoomDifficulty.Medium:
                        {
                            areaMonsterFit = Function(monsterDifficulty, size / 2.0, 0, size);
                        }
                        break;
                    case RoomDifficulty.Extreme:
                        {
                            areaMonsterFit = Function(monsterDifficulty, size, 0, size);
                        }
                        break;
                }

                monsterFit *= (1.0 / area.Size) * areaMonsterFit;
            }

            double maxMonstersFitness = 0.0;
            if (MaxMonsters == 0)
            {
                maxMonstersFitness = FunctionZero(numberMonsters);
            }
            else
            {
                maxMonstersFitness = FunctionNBest(numberMonsters, MaxMonsters);
            }
            /*double maxItensFitness = 0.0;
            if (MaxItens == 0)
            {
                maxItensFitness = FunctionZero(numberItems);
            }
            else
            {
                maxItensFitness = FunctionNBest(numberItems, MaxItens);
            }*/

            totalFitness = maxMonstersFitness * monsterFit;
            return totalFitness;
        }

        private double GetItemFitness(Cell cell, Area area, List<CellStruct> listCells)
        {
            double closeToMonster = 0.0;
            double distanceToEntrance = 0.0;

            int distanceToMonster = FloodFill(cell, listCells, HasMonster, false);

            switch (area.ItemAccessibility)
            {
                case ItemAccessibility.SafeToGet:
                    {
                        closeToMonster = Function(distanceToMonster, area.Size, 0, area.Size);

                        distanceToEntrance = Function(DistanceStartTarget(area.StartCell, cell, listCells), 0, 0, area.Size);
                        break;
                    }
                case ItemAccessibility.HardToGet:
                    {
                        closeToMonster = Function(distanceToMonster, 0, 0, area.Size);

                        distanceToEntrance = Function(DistanceStartTarget(area.StartCell, cell, listCells), area.Size, 0, area.Size);
                        break;
                    }
            }

            return distanceToEntrance * closeToMonster;
        }

        private double GetMonsterDifficulty(Cell cell, Area area, List<CellStruct> listCells)
        {
            double distance = 0.0;
            double type = 0.0;
            double horde = 0.0;
            CellStruct c = listCells.FirstOrDefault(x => x.x == cell.X && x.y == cell.Y);

            distance = Function(DistanceStartTarget(area.StartCell, cell, listCells), 0, 0, area.Size);

            if(HasSkeleton(c))
            {
                type = 1.0;
            }
            else if (HasMummy(c))
            {
                type = 0.6;
            }
            else if (HasTurtle(c))
            {
                type = 0.2;
            }

            int distanceToMonster = FloodFill(cell, listCells, HasMonster, false);

            if(distanceToMonster < 4) horde = Function(distanceToMonster, 1, 0, 3);

            return 0.7 * type + 0.2 * distance + 0.1 * horde;
        }

        private static int FloodFill(Cell startCell, List<CellStruct> listCells, HasSomething has, bool checkFirst)
        {
            int tileTraversed = 0;

            for (int i = 0; i < listCells.Count; i++)
            {
                listCells[i].visited = false;
            }

            if (checkFirst)
            {
                if (has(startCell.X, startCell.Y, listCells)) return 0;
            }


            ListQueue<CellStruct> queue = new ListQueue<CellStruct>();
            CellStruct firstCell = listCells.FirstOrDefault(c => c.x == startCell.X && c.y == startCell.Y);
            firstCell.visited = true;
            queue.Enqueue(firstCell);

            while (queue.Count != 0)
            {
                CellStruct node = queue.Dequeue();
                tileTraversed++;

                //west
                if (HasCellUnvisited(node.x - 1, node.y, listCells))
                {
                    if (has(node.x - 1, node.y, listCells)) return tileTraversed;

                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x - 1 && c.y == node.y);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //east
                if (HasCellUnvisited(node.x + 1, node.y, listCells))
                {
                    if (has(node.x + 1, node.y, listCells)) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x + 1 && c.y == node.y);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //north
                if (HasCellUnvisited(node.x, node.y - 1, listCells))
                {
                    if (has(node.x, node.y - 1, listCells)) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y - 1);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //south
                if (HasCellUnvisited(node.x, node.y + 1, listCells))
                {
                    if (has(node.x, node.y + 1, listCells)) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y + 1);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
            }
            return tileTraversed;
        }

        private static int DistanceStartTarget(Cell start, Cell target, List<CellStruct> listCells)
        {
            int tileTraversed = 0;

            for (int i = 0; i < listCells.Count; i++)
            {
                listCells[i].visited = false;
            }
            
            if (start.X == target.X && start.Y == target.Y) return 0;


            ListQueue<CellStruct> queue = new ListQueue<CellStruct>();
            CellStruct firstCell = listCells.FirstOrDefault(c => c.x == start.X && c.y == start.Y);
            firstCell.visited = true;
            queue.Enqueue(firstCell);

            while (queue.Count != 0)
            {
                CellStruct node = queue.Dequeue();
                tileTraversed++;

                //west
                if (HasCellUnvisited(node.x - 1, node.y, listCells))
                {
                    if (node.x - 1 == target.X && node.y == target.Y) return tileTraversed;

                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x - 1 && c.y == node.y);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //east
                if (HasCellUnvisited(node.x + 1, node.y, listCells))
                {
                    if (node.x + 1 == target.X && node.y == target.Y) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x + 1 && c.y == node.y);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //north
                if (HasCellUnvisited(node.x, node.y - 1, listCells))
                {
                    if (node.x == target.X && node.y - 1 == target.Y) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y - 1);
                    openNode.visited = true;
                    queue.Enqueue(openNode);
                }
                //south
                if (HasCellUnvisited(node.x, node.y + 1, listCells))
                {
                    if (node.x == target.X && node.y + 1 == target.Y) return tileTraversed;
                    CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y + 1);
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

        /**------------ FITNESS -------------**/

        /* Para quando x <= n */
        private double FunctionRising(double x, double n)
        {
            return System.Math.Min(System.Math.Abs(x / n), 1.0);
        }

        private double FunctionMedium(double x, double n)
        {
            return System.Math.Max(0.0, -System.Math.Abs(((2.0 * x) / n) - 1.0) + 1.0);
        }

        private double FunctionDecreasing(double x, double n)
        {
            return System.Math.Abs((x / n) - 1.0);
        }

        //Para quando x for maior que n.
        private double FunctionNBest(double x, double n)
        {
            return System.Math.Max(0.0, -System.Math.Abs((x / n) - 1.0) + 1.0);
        }

        private double GetMonsterMultiplier(int numberMonsters)
        {
            if (numberMonsters == 1) return 1.0;
            else if (numberMonsters == 2) return 1.5;
            else if (numberMonsters >= 3 && numberMonsters <= 6) return 2.0;
            else if (numberMonsters >= 7 && numberMonsters <= 10) return 2.5;
            else if (numberMonsters >= 11 && numberMonsters <= 14) return 3.0;
            else if (numberMonsters >= 15) return 4.0;
            return 0.0;
        }

        private int GetMonstersArea(List<Area> areas)
        {
            int totalTiles = 0;

            foreach (Area a in areas)
            {
                switch (a.Difficulty)
                {
                    case (RoomDifficulty.Safe):
                        {
                            totalTiles += a.Size;
                            break;
                        }
                    case (RoomDifficulty.Medium):
                        {
                            totalTiles += a.Size * 2;
                            break;
                        }
                    case (RoomDifficulty.Extreme):
                        {
                            totalTiles += a.Size * 3;
                            break;
                        }
                }

            }
            return totalTiles;
        }

        private double NumberMonster(double areaSize, double difficulty)
        {
            return System.Math.Ceiling((double)(areaSize * difficulty / GetMonstersArea(areaManager.AreaList)) * MaxMonsters);
        }

        private double FunctionZero(double x)
        {
            if (x < 0.0) return System.Math.Exp(2.0 * x);
            return System.Math.Exp(-2.0 * x);
        }

        private double Function(double x, double n, double min, double max)
        {
            double result = 0.0;
            if(x < n)
            {
                result = (1 / (n - min)) * x + (-min / (n - min));
            } else
            {
                result = (1 / (n - max)) * x + (-max / (n - max));
            }

            return System.Math.Max(0.0, result);
        }
    }
}
