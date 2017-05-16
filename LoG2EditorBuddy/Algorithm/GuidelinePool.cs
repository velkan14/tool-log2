using GAF;
using GAF.Operators;
using Log2CyclePrototype.Layers;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Algorithm
{
    class GuidelinePool
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        /*******************************************************/
        /*********************Parameters************************/
        /*******************************************************/

        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public float HordesPercentage { get; set; } //FIXME: Needs to be used
        /*******************************************************/

        protected bool running;
        protected Delegate callback;

        protected Map originalMap;
        protected List<Cell> cells;
        protected AreaManager areaManager;

        public bool HasSolution { get; protected set; }
        public Population Solution { get; protected set; }

        private delegate bool HasSomething(int x, int y, List<CellStruct> listCells);

        public GuidelinePool()
        {
            InitialPopulation = 100;
            GenerationLimit = 200;
            MutationPercentage = 0.6;
            CrossOverPercentage = 0.5;
            ElitismPercentage = 10;

            MaxMonsters = 10;
            MaxItens = 5;

            running = false;
            HasSolution = false;
        }

        public void Run(Map currentMap, AreaManager areaManager, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap;
            this.areaManager = areaManager;
            cells = currentMap.SpawnCells;

            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES, true, true);

            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, APIClass.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
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
        private double FunctionUpDown(double x, double n)
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
                    case (Difficulty.Easy):
                        {
                            totalTiles += a.Size;
                            break;
                        }
                    case (Difficulty.Medium):
                        {
                            totalTiles += a.Size * 2;
                            break;
                        }
                    case (Difficulty.Hard):
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

        protected double CalculateFitness(Chromosome chromosome)
        {
            double totalFitness = 0.0; // Value between 0 and 1. 1 is the fittest
            int totalMonsters = 0;
            int totalItens = 0;

            string binaryString = chromosome.ToBinaryString();

            List<CellStruct> listCells = new List<CellStruct>();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);
                listCells.Add(new CellStruct(j, cells[i].X, cells[i].Y));
            }

            foreach (Area area in areaManager.AreaList)
            {
                Cell startCell = area.StartCell;
                List<Cell> cellsArea = area.Cells;

                int numberMonsters = 0;
                int numberItens = 0;
                int numberHordes = 0;
                int numberMonsterEasy = 0;
                int numberMonsterMedium = 0;
                int numberMonsterHard = 0;
                double fitness = 0.0;


                foreach (Cell c in cellsArea)
                {
                    if (HasMonster(c.X, c.Y, listCells))
                    {
                        numberMonsters++;
                        if (HasTurtle(c.X, c.Y, listCells)) numberMonsterEasy++;
                        if (HasMummy(c.X, c.Y, listCells)) numberMonsterMedium++;
                        if (HasSkeleton(c.X, c.Y, listCells)) numberMonsterHard++;
                        if (FloodFill(c, cellsArea, listCells, HasMonster, false) <= 1) numberHordes++;
                    }
                    else if (HasItem(c.X, c.Y, listCells))
                    {
                        numberItens++;
                    }
                }

                switch (area.Difficulty)
                {
                    case Difficulty.Easy:
                        {
                            double numberMonsterArea = NumberMonster(area.Size, 1);
                            double numberMonsterFit = 0.0;

                            if (numberMonsterArea == 0.0)
                            {
                                numberMonsterFit = FunctionZero(numberMonsters);
                            }
                            else
                            {
                                numberMonsterFit = FunctionUpDown(numberMonsters, numberMonsterArea);
                            }

                            if (numberMonsters != 0.0)
                            {
                                fitness = (1.0 / 4.0) * numberMonsterFit +
                                    (1.0 / 4.0) * FunctionRising(FloodFill(startCell, cellsArea, listCells, HasMonster, true), area.Size) +
                                    (1.0 / 4.0) * FunctionDecreasing(numberHordes, numberMonsters) +
                                    (1.0 / 4.0) * FunctionUpDown((numberMonsterEasy + numberMonsterMedium * 2.0 + numberMonsterHard * 4.0) * GetMonsterMultiplier(numberMonsters), area.Size / 9.0);
                                
                            }

                        }
                        break;
                    case Difficulty.Medium:
                        {
                            double numberMonsterArea = NumberMonster(area.Size, 2);
                            double numberMonsterFit = 0.0;

                            if (numberMonsterArea == 0.0)
                            {
                                numberMonsterFit = FunctionZero(numberMonsters);
                            }
                            else
                            {
                                numberMonsterFit = FunctionUpDown(numberMonsters, numberMonsterArea);
                            }

                            if (numberMonsters != 0.0)
                            {
                                fitness = (1.0 / 4.0) * numberMonsterFit +
                                    (1.0 / 4.0) * FunctionMedium(FloodFill(startCell, cellsArea, listCells, HasMonster, true), area.Size) +
                                    (1.0 / 4.0) * FunctionMedium(numberHordes, numberMonsters) +
                                    (1.0 / 4.0) * FunctionUpDown((numberMonsterEasy * 1.0 + numberMonsterMedium * 2.0 + numberMonsterHard * 4.0) * GetMonsterMultiplier(numberMonsters), (area.Size /9.0 )* 2.0);
                            }

                        }
                        break;
                    case Difficulty.Hard:
                        {
                            double numberMonsterArea = NumberMonster(area.Size, 3);
                            double numberMonsterFit = 0.0;

                            if (numberMonsterArea == 0.0)
                            {
                                numberMonsterFit = FunctionZero(numberMonsters);
                            }
                            else
                            {
                                numberMonsterFit = FunctionUpDown(numberMonsters, numberMonsterArea);
                            }

                            if (numberMonsters != 0.0)
                            {
                                fitness = (1.0 / 4.0) * numberMonsterFit +
                                    (1.0 / 4.0) * FunctionDecreasing(FloodFill(startCell, cellsArea, listCells, HasMonster, true), area.Size) +
                                    (1.0 / 4.0) * FunctionRising(numberHordes, numberMonsters) +
                                    (1.0 / 4.0) * FunctionUpDown((numberMonsterEasy + numberMonsterMedium * 2.0 + numberMonsterHard * 4.0) * GetMonsterMultiplier(numberMonsters), (area.Size/9.0) * 3.0);

                            }
                        }
                        break;
                }
                if (fitness < 0.0) fitness = 0.0;

                totalFitness += (1.0 / areaManager.AreaList.Count) * fitness;
                totalMonsters += numberMonsters;
                totalItens += numberItens;

            }

            double hasArmor = FloodFill(areaManager.AreaList[0].StartCell, cells, listCells, HasArmor, true);
            double hasWeapon = FloodFill(areaManager.AreaList[0].StartCell, cells, listCells, HasWeapon, true);
            double hasResource = FloodFill(areaManager.AreaList[0].StartCell, cells, listCells, HasResource, true);
            
            double itensFitness = (1.0 / 3.0) * FunctionDecreasing(hasArmor, cells.Count) +
                                  (1.0 / 3.0) * FunctionDecreasing(hasWeapon, cells.Count) +
                                  (1.0 / 3.0) * FunctionDecreasing(hasResource, cells.Count);

            /* Objective of Max Itens*/
            double maxItensFitness = 0.0;
            if (MaxItens == 0)
            {
                maxItensFitness = FunctionZero(totalItens);
            }
            else
            {
                maxItensFitness = FunctionUpDown(totalItens, MaxItens);
            }

            /* Objective of Max Monster*/
            double maxMonstersFitness = 0.0;
            if (MaxMonsters == 0)
            {
                maxMonstersFitness = FunctionZero(totalMonsters);
            }
            else
            {
                maxMonstersFitness = FunctionUpDown(totalMonsters, MaxMonsters);
            }

            /* Percentages of all fitness */
            totalFitness = 0.5 * (0.5 * maxMonstersFitness + 0.5 * totalFitness) + 0.5 * (0.5 * itensFitness + 0.5 * maxItensFitness);

            return totalFitness;
        }

        private static int FloodFill(Cell startCell, List<Cell> cellsArea, List<CellStruct> listCells, HasSomething has, bool checkFirst)
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

        private bool HasItem(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 33 || j == 34 || j == 35)
            {
                //Cudgel
                return true;
            }
            else if (j == 36 || j == 37 || j == 38)
            {
                //Machete
                return true;
            }
            else if (j == 39 || j == 40 || j == 41)
            {
                //Rapier
                return true;
            }
            else if (j == 42 || j == 43 || j == 44)
            {
                //Battle Axe
                return true;
            }
            else if (j == 45 || j == 46 || j == 47 || j == 48)
            {
                //Potion
                return true;
            }
            else if (j == 49 || j == 50 || j == 51)
            {
                //Borra
                return true;
            }
            else if (j == 52 || j == 53 || j == 54)
            {
                //Bread
                return true;
            }
            else if (j == 55 || j == 56)
            {
                //Peasant cap
                return true;
            }
            else if (j == 57 || j == 58)
            {
                //Peasant breeches
                return true;
            }
            else if (j == 59)
            {
                //Peasant tunic
                return true;
            }
            else if (j == 60)
            {
                //Sandals
                return true;
            }
            else if (j == 61)
            {
                //Leather cap
                return true;
            }
            else if (j == 62)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 63)
            {
                //Leather pants
                return true;
            }
            else if (j == 64)
            {
                //Leather boots
                return true;
            }
            return false;
        }

        private bool HasWeapon(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 33 || j == 34 || j == 35)
            {
                //Cudgel
                return true;
            }
            else if (j == 36 || j == 37 || j == 38)
            {
                //Machete
                return true;
            }
            else if (j == 39 || j == 40 || j == 41)
            {
                //Rapier
                return true;
            }
            else if (j == 42 || j == 43 || j == 44)
            {
                //Battle Axe
                return true;
            }
            return false;
        }

        private bool HasResource(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 45 || j == 46 || j == 47 || j == 48)
            {
                //Potion
                return true;
            }
            else if (j == 49 || j == 50 || j == 51)
            {
                //Borra
                return true;
            }
            else if (j == 52 || j == 53 || j == 54)
            {
                //Bread
                return true;
            }
            return false;
        }

        private bool HasArmor(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 55 || j == 56)
            {
                //Peasant cap
                return true;
            }
            else if (j == 57 || j == 58)
            {
                //Peasant breeches
                return true;
            }
            else if (j == 59)
            {
                //Peasant tunic
                return true;
            }
            else if (j == 60)
            {
                //Sandals
                return true;
            }
            else if (j == 61)
            {
                //Leather cap
                return true;
            }
            else if (j == 62)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 63)
            {
                //Leather pants
                return true;
            }
            else if (j == 64)
            {
                //Leather boots
                return true;
            }
            return false;
        }

        private static bool HasCell(int x, int y, List<CellStruct> listCells)
        {
            foreach (CellStruct cs in listCells)
            {
                if (cs.x == x && cs.y == y)
                {
                    return true;
                }
            }
            return false;
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

        private static bool HasMonster(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
            {
                //Turtle
                return true;
            }
            else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
            {
                //Mummy
                return true;
            }
            else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
            {
                //Skeleton
                return true;
            }
            return false;
        }

        private static bool HasTurtle(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
            {
                //Turtle
                return true;
            }
            return false;
        }

        private static bool HasMummy(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
            {
                //Mummy
                return true;
            }
            return false;
        }

        private static bool HasSkeleton(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
            {
                //Skeleton
                return true;
            }
            return false;
        }

        protected bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
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
