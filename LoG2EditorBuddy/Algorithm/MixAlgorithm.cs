using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using Log2CyclePrototype.LoG2API;
using GAF.Operators;

namespace Log2CyclePrototype.Algorithm
{
    class MixAlgorithm
    {
        public int InitialPopulation { get; set; }
        public int GenerationLimit { get; set; }
        public double MutationPercentage { get; set; }
        public double CrossOverPercentage { get; set; }
        public int ElitismPercentage { get; set; }

        /*******************************************************/
        /*********************Parameters************************/
        /*******************************************************/
        public float ObjectivePercentage { get; set; }
        public float InnovationPercentage { get; set; }
        public float UserPercentage { get; set; }

        public int MaxMonsters { get; set; }
        public int MaxItens { get; set; }
        public float HordesPercentage { get; set; }
        public float MapObjectsPercentage { get; set; }
        /*******************************************************/

        private bool running;
        private Delegate callback;

        private Map originalMap;
        private List<Cell> cells;

        public bool HasSolution { get; private set; }
        public Population Solution { get; private set; }

        public MixAlgorithm()
        {
            InitialPopulation = 100;
            GenerationLimit = 200;
            MutationPercentage = 1.0;
            CrossOverPercentage = 0.4;
            ElitismPercentage = 5;

            ObjectivePercentage = 1.0f;
            InnovationPercentage = 0.4f;
            UserPercentage = 0.4f;

            MaxMonsters = 7;
            MaxItens = 5;

            running = false;
            HasSolution = false;
        }

        internal void Run(Map currentMap, Population conv, Population inno, Population obj, Delegate callback)
        {
            if (running) return;

            originalMap = currentMap;
            cells = currentMap.SpawnCells;

            this.callback = callback;

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population(InitialPopulation, cells.Count * APIClass.NUMBER_GENES, true, true, ParentSelectionMethod.StochasticUniversalSampling);

            population.Solutions.Clear();

            float total = ObjectivePercentage + UserPercentage + InnovationPercentage;
            int objective = (int)System.Math.Round(ObjectivePercentage / total);
            int user = (int)System.Math.Round(UserPercentage / total);
            int innov = (int)System.Math.Round(InnovationPercentage / total);

            population.Solutions.AddRange(obj.GetTop(objective * InitialPopulation));
            population.Solutions.AddRange(obj.GetTop(user * InitialPopulation));
            population.Solutions.AddRange(obj.GetTop(innov * InitialPopulation));


            //create the elite operator
            var elite = new Elite(ElitismPercentage);

            //create the crossover operator
            var crossover = new CrossoverIndex(CrossOverPercentage, APIClass.NUMBER_GENES, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);

            //create the mutation operator
            var mutate = new BinaryMutate(MutationPercentage);
            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitnessBinary);

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.OnRunComplete += OnRunComplete;

            //run the GA
            running = true;
            ga.Run(TerminateFunction);
        }

        private double CalculateFitnessBinary(Chromosome chromosome)
        {
            double fitness = 0.0; // Value between 0 and 1. 1 is the fittest
            int numberMonsters = 0;
            int numberItens = 0;

            int numberOfMonstersThatBelongToHorde = 0;
            int numberOfMonstersOnMapObject = 0;

            string binaryString = chromosome.ToBinaryString();

            for (int i = 0; i < cells.Count; i++)
            {
                string s = binaryString.Substring(i * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
                {
                    //Turtle
                    if (BelongToHorde(i, binaryString))
                    {
                        numberOfMonstersThatBelongToHorde++;
                    }
                    if (originalMap.CloseToElement(cells[i], "lock") ||
                       originalMap.CloseToElement(cells[i], "wall_button") ||
                       originalMap.CloseToElement(cells[i], "dungeon_door_iron") ||
                       originalMap.CloseToElement(cells[i], "castle_door_portcullis") ||
                       originalMap.CloseToElement(cells[i], "iron_key"))
                    {
                        numberOfMonstersOnMapObject++;
                    }
                    numberMonsters++;
                }
                else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
                {
                    //Mummy
                    if (BelongToHorde(i, binaryString))
                    {
                        numberOfMonstersThatBelongToHorde++;
                    }
                    if (originalMap.CloseToElement(cells[i], "lock") ||
                       originalMap.CloseToElement(cells[i], "wall_button") ||
                       originalMap.CloseToElement(cells[i], "dungeon_door_iron") ||
                       originalMap.CloseToElement(cells[i], "castle_door_portcullis") ||
                       originalMap.CloseToElement(cells[i], "iron_key"))
                    {
                        numberOfMonstersOnMapObject++;
                    }
                    numberMonsters++;
                }
                else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
                {
                    //Skeleton
                    if (BelongToHorde(i, binaryString))
                    {
                        numberOfMonstersThatBelongToHorde++;
                    }
                    if (originalMap.CloseToElement(cells[i], "lock") ||
                       originalMap.CloseToElement(cells[i], "wall_button") ||
                       originalMap.CloseToElement(cells[i], "dungeon_door_iron") ||
                       originalMap.CloseToElement(cells[i], "castle_door_portcullis") ||
                       originalMap.CloseToElement(cells[i], "iron_key"))
                    {
                        numberOfMonstersOnMapObject++;
                    }
                    numberMonsters++;
                }
                else if (j == 33 || j == 34 || j == 35)
                {
                    //Cudgel
                    numberItens++;
                }
                else if (j == 36 || j == 37 || j == 38)
                {
                    //Machete
                    numberItens++;
                }
                else if (j == 39 || j == 40 || j == 41)
                {
                    //Rapier
                    numberItens++;
                }
                else if (j == 42 || j == 43 || j == 44)
                {
                    //Battle Axe
                    numberItens++;
                }
                else if (j == 45 || j == 46 || j == 47 || j == 48)
                {
                    //Potion
                    numberItens++;
                }
                else if (j == 49 || j == 50 || j == 51)
                {
                    //Borra
                    numberItens++;
                }
                else if (j == 52 || j == 53 || j == 54)
                {
                    //Bread
                    numberItens++;
                }
                else if (j == 55 || j == 56)
                {
                    //Peasant cap
                    numberItens++;
                }
                else if (j == 57 || j == 58)
                {
                    //Peasant breeches
                    numberItens++;
                }
                else if (j == 59)
                {
                    //Peasant tunic
                    numberItens++;
                }
                else if (j == 60)
                {
                    //Sandals
                    numberItens++;
                }
                else if (j == 61)
                {
                    //Leather cap
                    numberItens++;
                }
                else if (j == 62)
                {
                    //Leather brigandine
                    numberItens++;
                }
                else if (j == 63)
                {
                    //Leather pants
                    numberItens++;
                }
                else if (j == 64)
                {
                    //Leather boots
                    numberItens++;
                }
                else
                {
                    //Nothing
                }
            }

            /* Objective of Max Itens*/
            double maxItensFitness = func(numberItens, MaxItens);

            /* Objective of Max Monster*/
            double maxMonstersFitness = func(numberMonsters, MaxMonsters);

            /* Objective of Hordes */
            double hordesFitness = func(numberOfMonstersThatBelongToHorde, MaxMonsters);

            /* Objective of MapObjects */
            double mapObjectsFitness = func(numberOfMonstersOnMapObject, MaxMonsters);

            /* Percentages of all fitness */

            /*----------------------------*/
            fitness = (hordesFitness + maxMonstersFitness + mapObjectsFitness + maxItensFitness) / 4.0;
            return fitness;
        }

        private bool CloseToElement(int i, string binaryString, string elementType)
        {
            List<Cell> neighbours = new List<Cell>();

            Cell cell = cells[i];

            foreach (Cell c in cells)
            {
                if (c.X == cell.X - 1 && c.Y == cell.Y - 1) neighbours.Add(c);
                if (c.X == cell.X && c.Y == cell.Y - 1) neighbours.Add(c);
                if (c.X == cell.X + 1 && c.Y == cell.Y - 1) neighbours.Add(c);

                if (c.X == cell.X - 1 && c.Y == cell.Y) neighbours.Add(c);
                if (c.X == cell.X + 1 && c.Y == cell.Y) neighbours.Add(c);

                if (c.X == cell.X - 1 && c.Y == cell.Y + 1) neighbours.Add(c);
                if (c.X == cell.X && c.Y == cell.Y + 1) neighbours.Add(c);
                if (c.X == cell.X + 1 && c.Y == cell.Y + 1) neighbours.Add(c);
            }

            foreach (Cell n in neighbours)
            {
                int k = cells.IndexOf(n);

                string s = binaryString.Substring(k * APIClass.NUMBER_GENES, APIClass.NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
                {
                    //Turtle
                    if (elementType.Equals("turtle")) return true;
                }
                else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
                {
                    //Mummy
                    if (elementType.Equals("mummy")) return true;
                }
                else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
                {
                    //Skeleton
                    if (elementType.Equals("skeleton_trooper")) return true;
                }
                else if (j == 33 || j == 34 || j == 35)
                {
                    //Cudgel
                    if (elementType.Equals("cudgel")) return true;
                }
                else if (j == 36 || j == 37 || j == 38)
                {
                    //Machete
                    if (elementType.Equals("machete")) return true; ;
                }
                else if (j == 39 || j == 40 || j == 41)
                {
                    //Rapier
                    if (elementType.Equals("rapier")) return true;
                }
                else if (j == 42 || j == 43 || j == 44)
                {
                    //Battle Axe
                    if (elementType.Equals("battle_axe")) return true;
                }
                else if (j == 45 || j == 46 || j == 47 || j == 48)
                {
                    //Potion
                    if (elementType.Equals("potion_healing")) return true;
                }
                else if (j == 49 || j == 50 || j == 51)
                {
                    //Borra
                    if (elementType.Equals("borra")) return true;
                }
                else if (j == 52 || j == 53 || j == 54)
                {
                    //Bread
                    if (elementType.Equals("bread")) return true;
                }
                else if (j == 55 || j == 56)
                {
                    //Peasant cap
                    if (elementType.Equals("peasant_cap")) return true;
                }
                else if (j == 57 || j == 58)
                {
                    //Peasant breeches
                    if (elementType.Equals("peasant_breeches")) return true;
                }
                else if (j == 59)
                {
                    //Peasant tunic
                    if (elementType.Equals("peasant_tunic")) return true;
                }
                else if (j == 60)
                {
                    //Sandals
                    if (elementType.Equals("sandals")) return true;
                }
                else if (j == 61)
                {
                    //Leather cap
                    if (elementType.Equals("leather_cap")) return true;
                }
                else if (j == 62)
                {
                    //Leather brigandine
                    if (elementType.Equals("leather_brigandine")) return true;
                }
                else if (j == 63)
                {
                    //Leather pants
                    if (elementType.Equals("leather_pants")) return true;
                }
                else if (j == 64)
                {
                    //Leather boots
                    if (elementType.Equals("leather_boots")) return true;
                }
                else
                {
                    //Nothing
                    return false;
                }
            }
            return false;
        }

        private bool BelongToHorde(int i, string binaryString)
        {
            if (CloseToElement(i, binaryString, "turtle")) return true;
            if (CloseToElement(i, binaryString, "mummy")) return true;
            if (CloseToElement(i, binaryString, "skeleton_trooper")) return true;
            return false;
        }

        private static double func(double x, double c)
        {
            if (x == c) return 1.0;
            if (x < c) return -c / (x - 2.0 * c);
            if (x > c) return c / x;
            return 0.0;
        }

        private bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation)
        {
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
