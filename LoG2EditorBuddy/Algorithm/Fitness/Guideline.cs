using Povoater.Layers;
using Povoater.LoG2API;
using Povoater.Utilities;
using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Algorithm.Fitness
{
    class Guideline : HasStuff
    {
        protected List<Cell> cells;
        protected AreaManager areaManager;

        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public double HordesPercentage { get; set; }

        public double MaxMonstersLever { get; set; }
        public double MaxItemsLever { get; set; }
        public double AmountHordesLever { get; set; }
        public double DangerLever { get; set; }
        public double AccessibilityLever { get; set; }


        private delegate bool HasSomething(int x, int y, List<CellStruct> listCells);

        public Guideline(List<Cell> spawnCells, AreaManager areaManager, int maxMonsters, int maxItens, double hordesPercentage)
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
            int numberItems = 0;
            int numberHordes = 0;

            double monsterFit = 0.0;
            double itemFit = 1.0;


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
                double monsterDifficulty = 0.0;

                foreach (Cell c in area.Cells)
                {
                    if (HasMonster(c.X, c.Y, listCells))
                    {
                        int distanceToMonster = Distance(c, listCells, HasMonster, false);

                        monsterDifficulty += GetMonsterDifficulty(c, area, listCells, distanceToMonster);
                        numberMonsters++;
                        
                        if (distanceToMonster < 2) numberHordes++;
                    }
                    else if (HasItem(c.X, c.Y, listCells))
                    {
                        itemFit *= GetItemFitness(c, area, listCells);
                        numberItems++;
                    }
                }

                monsterFit += (1.0 / areaManager.AreaList.Count) * 
                    Math.Function(monsterDifficulty, area.GetDesireDifficulty(), 0.0, area.Size);
            }

            double maxMonstersFitness = Math.Function(numberMonsters, MaxMonsters, 0, cells.Count);

            double maxItensFitness = Math.Function(numberItems, MaxItens, 0, cells.Count);

            double hordesFit = 1.0;

            if(numberMonsters > 0)
            {
                double percentageOfHordes = numberHordes / numberMonsters;
                hordesFit = Math.Function(percentageOfHordes, HordesPercentage, 0.0, 1.0);
            }

            //totalFitness = (0.34 * maxMonstersFitness + 0.33 * monsterFit + 0.33 * hordesFit) * (0.5 * maxItensFitness + 0.5 * itemFit);
            totalFitness = MaxMonstersLever * maxMonstersFitness +
                            MaxItemsLever * maxItensFitness +
                            AmountHordesLever * hordesFit +
                            DangerLever * monsterFit +
                            AccessibilityLever * itemFit;

            if (Double.IsNaN(totalFitness))
            { Logger.AppendText("Error: NaN Guidline"); }

            return totalFitness;
        }

        private double GetItemFitness(Cell cell, Area area, List<CellStruct> listCells)
        {
            double closeToMonster = 0.0;
            double distanceToEntrance = 0.0;

            int distanceToMonster = Distance(cell, listCells, HasMonster, false);

            int distanceToStart = DistanceStartTarget(area.StartCell, cell, listCells);
            switch (area.ItemAccessibility)
            {
                case ItemAccessibility.SafeToGet:
                    {
                        //closeToMonster = Function(distanceToMonster, area.Size, 0, area.Size);

                        if (distanceToStart > area.Size) distanceToEntrance = 1.0;
                        else distanceToEntrance = Math.Function(distanceToStart, 0, 0, area.Size);
                        break;
                    }
                case ItemAccessibility.HardToGet:
                    {
                        //closeToMonster = Function(distanceToMonster, 0, 0, area.Size);
                        if (distanceToStart > area.Size) distanceToEntrance = 0.0;
                        else distanceToEntrance = Math.Function(distanceToStart, area.Size, 0, area.Size);
                        break;
                    }
            }

            return distanceToEntrance;
        }

        private double GetMonsterDifficulty(Cell cell, Area area, List<CellStruct> listCells, int distanceToMonster)
        {
            double distance = 0.0;
            double type = 0.0;
            double horde = 0.0;

            CellStruct c = listCells.FirstOrDefault(x => x.x == cell.X && x.y == cell.Y);

            int distanceToStart = DistanceStartTarget(area.StartCell, cell, listCells);
            if (distanceToStart > area.Size)
                Logger.AppendText("OMG!!!");

            distance = Math.Function(distanceToStart, 0, 0, area.Size);

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

            if(distanceToMonster < 4) horde = Math.Function(distanceToMonster, 1, 0, 3);

            return  0.5 * type + 0.25 * distance + 0.25 * horde;
        }

        private static int DistanceStartTarget(Cell start, Cell target, List<CellStruct> listCells)
        {
            int tileTraversed = 0;

            for (int i = 0; i < listCells.Count; i++)
            {
                listCells[i].visited = false;
            }

            List<CellStruct> list = new List<CellStruct>();
            List<CellStruct> copyList = new List<CellStruct>();

            CellStruct firstCell = listCells.FirstOrDefault(c => c.x == start.X && c.y == start.Y);
            firstCell.visited = true;
            list.Add(firstCell);

            while (list.Count != 0)
            {
                copyList.AddRange(list);
                list.Clear();

                foreach(CellStruct node in copyList)
                {
                    if (node.x == target.X && node.y == target.Y)
                        return tileTraversed;

                    //west
                    if (HasCellUnvisited(node.x - 1, node.y, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x - 1 && c.y == node.y);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //east
                    if (HasCellUnvisited(node.x + 1, node.y, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x + 1 && c.y == node.y);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //north
                    if (HasCellUnvisited(node.x, node.y - 1, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y - 1);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //south
                    if (HasCellUnvisited(node.x, node.y + 1, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y + 1);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                }

                tileTraversed++;
            }
            return tileTraversed;
        }

        private static int Distance(Cell start, List<CellStruct> listCells, HasSomething has, bool checkFirst)
        {
            int tileTraversed = 0;

            for (int i = 0; i < listCells.Count; i++)
            {
                listCells[i].visited = false;
            }

            List<CellStruct> list = new List<CellStruct>();
            List<CellStruct> copyList = new List<CellStruct>();

            CellStruct firstCell = listCells.FirstOrDefault(c => c.x == start.X && c.y == start.Y);
            firstCell.visited = true;
            list.Add(firstCell);

            while (list.Count != 0)
            {
                copyList.AddRange(list);
                list.Clear();

                foreach (CellStruct node in copyList)
                {
                    if (has(node.x, node.y, listCells))
                    {
                        if (checkFirst && node.x == firstCell.x && node.y == firstCell.y) return tileTraversed;
                        else if (node.x != firstCell.x || node.y != firstCell.y) return tileTraversed;
                    }
                        

                    //west
                    if (HasCellUnvisited(node.x - 1, node.y, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x - 1 && c.y == node.y);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //east
                    if (HasCellUnvisited(node.x + 1, node.y, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x + 1 && c.y == node.y);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //north
                    if (HasCellUnvisited(node.x, node.y - 1, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y - 1);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                    //south
                    if (HasCellUnvisited(node.x, node.y + 1, listCells))
                    {
                        CellStruct openNode = listCells.FirstOrDefault(c => c.x == node.x && c.y == node.y + 1);
                        openNode.visited = true;
                        list.Add(openNode);
                    }
                }

                tileTraversed++;
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
                    case (RoomDifficulty.Hard):
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

        /*private double Function(double x, double n, double min, double max)
        {
            double result = 0.0;

            if (n == x)
            {
                result = 1.0;
            }
            else if (x < n)
            {
                result = (1 / (n - min)) * x + (-min / (n - min));
            }
            else
            {
                result = (1 / (n - max)) * x + (-max / (n - max));
            }

            if (Double.IsNaN(result))
            { Logger.AppendText("Error: NaN Guidline"); }

            return System.Math.Max(0.0, result);
        }*/
    }
}
