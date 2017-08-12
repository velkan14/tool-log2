using System;
using System.Collections.Generic;
using System.Linq;
using GAF;
using GAF.Operators;
using System.Diagnostics;
//using Log2CyclePrototype.Algorithm;
using Povoater.Utilities;
using System.Drawing;
using Povoater.LoG2API;

//using Log2CyclePrototype.LoG2API;


namespace Povoater
{

    public enum PopulationCarryMethod
    {
        TopPercent,
        Random
    }

    public enum Objective
    {
        NarrowPaths,
        Rooms
    }

    public class ObjectiveAlgorithmTestClass
    {

        public static Population NoveltyPopulation { get; set; }

        private static string mutationDir = @"D:\GOG Games\Custom Levels\test4\mutations";

        public static event EventHandler OnObjectiveAlgorithmComplete;
        public static event EventHandler OnGenerationTerminated;

        //public bool Running = false;

        private GeneticAlgorithm _ga;
        private Population _currentPopulation;
        private Elite _elite;
        private Crossover _crossover;
        private CustomCrossover _customCrossover;
        private CustomSwapMutate _cs;
        private SwapMutate _mutate;
        private Delegate _callback;
        private Map _currentMap, _originalMap;
        private Population _previousObjectivePopulation;
        public static bool _recievedAlgorithmFinished;
        private List<Chromosome> _chosenOnes;
        private Random _rng;
 

        

        public bool Running { get; set; }
        public int GenerationLimit { get; set; }
        public int InitialPopulationSize { get; set; }
        //public double PercentChromosomesToInject{ get; set; }
        public float PercentNoveltyChromosomesToRecieve { get; set; }
        public float PercentObjectiveChromosomesToKeep { get; set; }
        public PopulationCarryMethod NextPopulationCarryMethod { get; set; }
        public CrossoverT CrossoverTypeSelected { get; set; }
        public float PercentUserSketchInfluence { get; set; }
        public bool Initialized { get; internal set; }
        public double PercentElitism { get; set; }
        public double PercentMutation { get; set; }
        public List<Point> UserSelectionPositiveFocus { get; set; }
        public float UserSelectionWeight { get; set; } = 1.5f;
        public bool ResetOnNextRun = false;
        public Objective CurrentObjective = Objective.NarrowPaths;
        public int TargetWalkableCellCount { get; set; } = 700;

        public ObjectiveAlgorithmTestClass(){
            Running = false;
            GenerationLimit = 40;
            InitialPopulationSize = 10;
            //PercentChromosomesToInject = 0;
            PercentNoveltyChromosomesToRecieve = 0;
            PercentMutation = 0.04;
            PercentElitism = 0.15;
            _previousObjectivePopulation = null;
            _chosenOnes = new List<Chromosome>();
            _rng = GAF.Threading.RandomProvider.GetThreadRandom();
            NextPopulationCarryMethod = PopulationCarryMethod.Random;
            CrossoverTypeSelected = CrossoverT.FourByFourSquare;
            PercentUserSketchInfluence = 10/100f;
            Initialized = false;
            UserSelectionPositiveFocus = new List<Point>();
        }

        public ObjectiveAlgorithmTestClass(Population initPop)
        {
            _previousObjectivePopulation = initPop;

            Running = false;
            GenerationLimit = 40;
            InitialPopulationSize = 10;
            //PercentChromosomesToInject = 0;
            PercentNoveltyChromosomesToRecieve = 0;
            PercentMutation = 0.04;
            PercentElitism = 0.15;
            _previousObjectivePopulation = null;
            _chosenOnes = new List<Chromosome>();
            NextPopulationCarryMethod = PopulationCarryMethod.Random;
            CrossoverTypeSelected = CrossoverT.FourByFourSquare;
            PercentUserSketchInfluence = 10/100f;
            UserSelectionPositiveFocus = new List<Point>();
            Initialized = true;
        }


        public void NewObjectiveRun(Map map, Delegate onCompleteFuntion)
        {
            //bestDist = 999;
            i = 0;
            c = 0;

            if (CurrentObjective == Objective.NarrowPaths)
                TargetWalkableCellCount = 600;
            else TargetWalkableCellCount = 700;

            try
            {
                Debug.WriteLine("Objective algorithm test setup started.");

                //NoveltyPopulation = null;
                _recievedAlgorithmFinished = false;

                _callback = onCompleteFuntion;

                _originalMap = map;
                _currentMap = CloneUtilities.CloneJson<Map>(map);
                _currentPopulation = new Population();
                Debug.WriteLine("Checkpoint 1 done.");

                if (_previousObjectivePopulation != null && !ResetOnNextRun)
                {

                    _currentPopulation.Solutions.AddRange(CreatePopulationFromPrevious(_currentMap, _previousObjectivePopulation, NoveltyPopulation));                    
                }
                else
                {                    
                    _currentPopulation.Solutions.AddRange(InitializePopulation(_currentMap));
                    Initialized = true;
                    ResetOnNextRun = false;
                }

                Debug.WriteLine("Checkpoint 2 done.");

                //create GA based on population generated
                _ga = new GeneticAlgorithm(_currentPopulation, CalculateFitness2);

                //add GA operators
                AddOperators();

                _ga.OnGenerationComplete += OnObjectiveGenerationComplete;
                _ga.OnRunComplete += OnObjectiveComplete;

                Debug.WriteLine("Objective algorithm Setup Complete.");
            }
            catch (Exception e) { DebugUtilities.DebugException(e); }
        }





        #region GENETIC OPERATORS

        /// <summary>
        /// Sets up the operators to be used during the evolutionary runs
        /// </summary>
        private void AddOperators()
        {
            //elitism
            _elite = new Elite((int)(PercentElitism * 100)); //param: elitism percentage (int)
            _ga.Operators.Add(_elite);


            //crossover
            switch (CrossoverTypeSelected)
            {
                case CrossoverT.TwoByTwoSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.TwoByTwoSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverT.ThreeByThreeSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.ThreeByThreeSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverT.FourByFourSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.FourByFourSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverT.SinglePoint:
                    _crossover = new Crossover(0.8)
                    {
                        CrossoverType = GAF.Operators.CrossoverType.SinglePoint
                    };
                    _ga.Operators.Add(_crossover);
                    break;
                case CrossoverT.DoublePoint:
                    _crossover = new Crossover(0.8)
                    {
                        CrossoverType = GAF.Operators.CrossoverType.DoublePoint
                    };
                    _ga.Operators.Add(_crossover);
                    break;
            }

            //Mutation
            //_mutate = new SwapMutate(0.04);
            _cs = new CustomSwapMutate(PercentMutation);
            _ga.Operators.Add(_cs);

        }


        #endregion


        #region FITNESS & TERMINATION


        private double normalize(double min, double max, double value)
        {
            return (value - min) / (max - min);
        }


        int c = 0;
        /// <summary>
        /// Calculate fitness for a certain chromosome
        /// </summary>
        /// <param name="chromosome"> Chromosome to evaluate </param>
        /// <returns></returns>
        private double CalculateFitness(Chromosome chromosome)
        {
            double fitness = 0;
            List<Node> biggestPath = new List<Node>();
            int walkableCells = 0;
            double linearity = 0;
            float roomInibition = 5f;

            //WriteUtilities.WriteSolution(c++, DirectoryManager.MutationsDir, chromosome, _currentMap.GroundFirstIndex);

            var startEndPoints = SearchUtils.FindStartEndPointsCoordinates(chromosome); //start e end points teem de ser walkable!!

            if (startEndPoints == null)
                return 0.0;

            //make sure start and at least 1 end point are walkable
            if (!startEndPoints[0].IsWalkable)
                return 0.0;
            var totalWalkable = 0;
            for (int p = 0; p < startEndPoints.Length; p++)
                if (startEndPoints[p].IsWalkable)
                    totalWalkable++;
            if (totalWalkable == 0)
                return 0.0;

            walkableCells = APIClass.CountWalkableCellsInChromosome(chromosome);

            if (startEndPoints != null){ //if there are start and end coords, evaluate proximity to 

                var allPaths = SearchUtils.GetAllPaths(chromosome);

                double eval = 0.0;
                for(int i = 1; i<=startEndPoints.Length - 1; i++){
                    var pointPaths = SearchUtils.StartEndPointsInSamePath(startEndPoints[0], startEndPoints[i], allPaths);

                    if (pointPaths.Item1 != -1 && pointPaths.Item2 != -1){ //if valid
                        //somar a fitness e devidir por num de endpoints
                        if (pointPaths.Item1 != pointPaths.Item2){ //not in same path
                            var startEndDist = (SearchUtils.ManhattanDistanceBetweenPoints(startEndPoints[0].X, startEndPoints[0].Y, startEndPoints[i].X, startEndPoints[i].Y));
                            var pathDist = SearchUtils.GetRemainingDistanceBetweenPaths(allPaths[pointPaths.Item1], allPaths[pointPaths.Item2]);
                            //Logger.AppendText(/*"S["+startEndPoints[0].X+","+startEndPoints[0].Y+"] E["+ startEndPoints[1].X + "," + startEndPoints[1].Y + "] */Start path: " +pointPaths.Item1 + " - End path: "+pointPaths.Item2 + " - Dist: " + pathDist.ToString());
                            //Logger.AppendText(startEndDist.ToString());
                            eval += (startEndDist - pathDist)/startEndDist;
                        }
                        else{
                            eval += 1.0;
                        }
                    }
                    else if (pointPaths.Item1 == -1 && pointPaths.Item2 != -1)
                    {
                        Debug.WriteLine("NO START POINT!!!");
                    }
                    else if (pointPaths.Item1 != -1 && pointPaths.Item2 == -1)
                    {
                        Debug.WriteLine("NO END POINT!!!");
                    }
                    else Debug.WriteLine("NO START & END POINTS!!!");
                }
                var nv = SearchUtils.CountIndividualNeighbours(chromosome);
                linearity = SearchUtils.EvaluateLinearityValue(chromosome);

                linearity = linearity / (float)allPaths.Count;

                //fitness = (eval/(startEndPoints.Length-1)) + ((linearity + ((nv[2]*roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4]*roomInibition))/walkableCells)) / 1024.0); //TODO normalize?
                //fitness = (eval / (startEndPoints.Length - 1)) + (linearity + ((nv[2] * roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4] * roomInibition)))); //TODO normalize?
                fitness = (eval / (startEndPoints.Length - 1)) + (linearity + ((nv[2] + nv[1] + nv[3] - nv[0] /*- (nv[3]) - (nv[4])*/ )))/1024f; //TODO normalize?

            }
            else
            {
                var nv = SearchUtils.CountIndividualNeighbours(chromosome); 
                linearity = SearchUtils.EvaluateLinearityValue(chromosome);
                //fitness = (linearity + ((nv[2]*roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4] * roomInibition)) / walkableCells)) / 1024.0;
                //fitness = (linearity + ((nv[2] * roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4] * roomInibition))));
                fitness = (linearity + ((nv[2] + nv[1] + nv[3] - nv[0] /*- (nv[3]) - (nv[4])*/ )))/1024f;
            }


            //if(UserSelectionPositiveFocus.Count > 0) //Take into account the affect of user selection on fitness function
            //{
            //    int dif = 0;
            //    //fetch all cells in selection
            //    foreach(var p in UserSelectionPositiveFocus)
            //    {
            //        Cell c = chromosome.Genes[p.Y*_currentMap.Width + p.X].ObjectValue as Cell;
            //        if (c != null)
            //        {
            //            if (c.CellType != _currentMap.GetCellAt(c.X, c.Y).CellType)
            //                dif++;
            //        }
            //        else Debug.WriteLine("CELL NULL");
            //    }
            //    float changePercent = dif/(float)UserSelectionPositiveFocus.Count;
            //    //Logger.AppendText(changePercent.ToString());
            //    fitness += (changePercent * fitness) * UserSelectionWeight;
            //}

            //fitness = normalize(0,  ,fitness);

            //Debug.WriteLine("Chromosome " + chromosome.Id + " BiggestPath: " + linearity + " fitness: " + fitness);

            return fitness;
        }

        //double bestDist = 999;

        float cellOffsetPenalty = 0;
        private double CalculateFitness2(Chromosome chromosome)
        {
            double fitness = 0;

            var start = _currentMap.StartPoint;
            var end = _currentMap.EndPointList[0];

            Cell startGene = (Cell)chromosome.Genes[start.x + _currentMap.Width * start.y].ObjectValue;
            Cell endGene = (Cell)chromosome.Genes[end.x + _currentMap.Width * end.y].ObjectValue;

            if (!endGene.IsWalkable || !startGene.IsWalkable)
                return 0.0;

            var allPaths = SearchUtils.GetAllPaths(chromosome);

            double startEndEval = 0.0;
            //for (int i = 1; i <= startEndPoints.Length - 1; i++)
            //{
                var pointPaths = SearchUtils.StartEndPointsInSamePath(start.x, start.y, end.x, end.y, allPaths);

                if (pointPaths.Item1 != -1 && pointPaths.Item2 != -1)
                { //if valid
                  //somar a fitness e devidir por num de endpoints
                    if (pointPaths.Item1 != pointPaths.Item2)
                    { //not in same path

                        var startEndDist = (SearchUtils.ManhattanDistanceBetweenPoints(start.x, start.y, end.x, end.y));
                        var pathDist = SearchUtils.GetRemainingDistanceBetweenPaths(allPaths[pointPaths.Item1], allPaths[pointPaths.Item2]);
                        startEndEval += ((startEndDist - pathDist) / startEndDist) * 1.3f;
                        //startEndEval -= (pathDist / (float)startEndDist) * 0.5f;
                        //if (pathDist < bestDist)
                        //{
                        //    bestDist = pathDist;                            
                        //}
                    }
                    else {
                        startEndEval += 1.0 * 2f;
                    }
                    //Debug.WriteLine("BEST DIST: " + bestDist);
                }
                else if (pointPaths.Item1 == -1 && pointPaths.Item2 != -1)
                {
                    Debug.WriteLine("NO START POINT!!!");
                }
                else if (pointPaths.Item1 != -1 && pointPaths.Item2 == -1)
                {
                    Debug.WriteLine("NO END POINT!!!");
                }
                else Debug.WriteLine("NO START & END POINTS!!!");
            //}


            switch (CurrentObjective)
            {
                case Objective.NarrowPaths:
                    {
                        var nv = SearchUtils.CountIndividualNeighbours(chromosome);
                        float linearity = SearchUtils.EvaluateLinearityValue(chromosome) / (float)(allPaths.Count + nv[4] + nv[3]);
                        linearity = linearity * 0.15f;
                        //Debug.WriteLine(linearity);
                        //fitness = (eval/(startEndPoints.Length-1)) + ((linearity + ((nv[2]*roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4]*roomInibition))/walkableCells)) / 1024.0); //TODO normalize?
                        //fitness = (eval / (startEndPoints.Length - 1)) + (linearity + ((nv[2] * roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4] * roomInibition)))); //TODO normalize?
                        fitness = startEndEval + (linearity + ((nv[2] + nv[1] - nv[0] - nv[4] )) / (float)TargetWalkableCellCount); //TODO normalize?

                        cellOffsetPenalty = System.Math.Abs(TargetWalkableCellCount - APIClass.CountWalkableCellsInChromosome(chromosome)) / 1024f;
                        fitness -= cellOffsetPenalty * 0.25f;
                    }
                    break;
                case Objective.Rooms:
                    {
                        var nv = SearchUtils.CountIndividualNeighbours(chromosome);
                        //float linearity = SearchUtils.EvaluateLinearityValue(chromosome) / (float)allPaths.Count;

                        //fitness = (eval/(startEndPoints.Length-1)) + ((linearity + ((nv[2]*roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4]*roomInibition))/walkableCells)) / 1024.0); //TODO normalize?
                        //fitness = (eval / (startEndPoints.Length - 1)) + (linearity + ((nv[2] * roomInibition + nv[1] - nv[0] - (nv[3] * roomInibition) - (nv[4] * roomInibition)))); //TODO normalize?
                        fitness = startEndEval + (/*linearity*/ + (( nv[2] + nv[3] - nv[0])) / (float)TargetWalkableCellCount); //TODO normalize?

                        cellOffsetPenalty = System.Math.Abs(TargetWalkableCellCount - APIClass.CountWalkableCellsInChromosome(chromosome)) / 1024f;
                        fitness -= cellOffsetPenalty;
                    }
                    break;
                default:
                    break;
            }


            return fitness;
        }


        private double CalculateFitness3(Chromosome chromosome)
        {
            double fitness = 0;
            List<Node> biggestPath = new List<Node>();
            

            var startEndPoints = SearchUtils.FindStartEndPointsCoordinates(chromosome); //start e end points teem de ser walkable!!

            if (startEndPoints == null)
                return 0.0;

            //make sure start and at least 1 end point are walkable
            if (!startEndPoints[0].IsWalkable)
                return 0.0;
            var totalWalkable = 0;
            for (int p = 0; p < startEndPoints.Length; p++)
                if (startEndPoints[p].IsWalkable)
                    totalWalkable++;
            if (totalWalkable == 0)
                return 0.0;
            
            int pCounter = 0;
            var cMaze = SearchUtils.ChromosomeToMaze(chromosome);
            cMaze.AllowDiagonals = false;
            var solution = cMaze.FindPath(startEndPoints[0].X, startEndPoints[0].Y, startEndPoints[1].X, startEndPoints[1].Y);
            if (solution != null)
            {
                foreach (var n in solution)
                {
                    if (n == 100)
                        pCounter++;
                }
                DebugUtilities.Debug2DMazeArray(solution, 32, 32);
                fitness += pCounter;
            }
            else
            {
                //Debug.WriteLine("No valid solution.");
                var nv = SearchUtils.CountIndividualNeighbours(chromosome);
                fitness = (nv[2] + nv[1] + nv[3]) / 1024f;

            }

            return fitness;
        }


        /// <summary>
        /// Determines when to halt algorithm execution
        /// </summary>
        /// <param name="population"></param>
        /// <param name="currentGeneration"></param>
        /// <param name="currentEvaluation"></param>
        /// <returns></returns>
        private bool TerminateFunction(Population population,
                                int currentGeneration,
                                long currentEvaluation)
        {
            //Debug.WriteLine("HELLO!");
            //example termination criterion 
            //return currentEvaluation >= 40000;
            //return after 100 generations
            if (currentGeneration > GenerationLimit)
            {
                Debug.WriteLine("Objective Algorithm terminated!");
                Logger.AppendText("Objective Algorithm terminated!");
                //Console.WriteLine(ga.UseMemory);
                Running = false;
                return true;
            }
            else return false;
        }


        /// <summary>
        /// Returns the Gene list of nth best solution of the current population
        /// </summary>
        /// <param name="solNum"></param>
        /// <returns></returns>
        public List<Gene> GetBestNthSolution(int solNum)
        {
            if (solNum > _currentPopulation.Solutions.Count)
                return null;
            return _currentPopulation.GetTop(solNum)[solNum - 1].Genes;
        }




        private void OnObjectiveComplete(object sender, GaEventArgs e)
        {
            //foreach(var c in _currentPopulation.Solutions)
            //{
            //    WriteUtilities.WriteSolution(_currentPopulation.Solutions.IndexOf(c), DirectoryManager.ProjectDir + @"\solution_history", c, APIClass.UnwalkableCellValue);
            //}

            var bestChromosome = e.Population.GetTop(1)[0];
            _chosenOnes.Add(bestChromosome); //don't create a new one, keep the fitness
            _callback.DynamicInvoke(bestChromosome.Genes);


            Debug.WriteLine("Best Objective Fitness: " + bestChromosome.Fitness);
            //Logger.AppendText("Best Objective Fitness: " + bestChromosome.Fitness);

            //Logger.AppendText("Cell variation penalty:" + cellOffsetPenalty);

            //e.Population.Solutions.Sort(); // sort so it orders by fitness
            _previousObjectivePopulation = new Population();
            foreach (var c in e.Population.Solutions)
            {
                var tmpC = new Chromosome(c.Genes);
                _previousObjectivePopulation.Solutions.Add(tmpC);
            }
            
            if (!_recievedAlgorithmFinished)
                MainForm.objectiveSyncHandle.WaitOne();

        }


        int i = 0;
        private void OnObjectiveGenerationComplete(object sender, GaEventArgs e)
        {            
            //WriteUtilities.WriteSolution(i++, DirectoryManager.ProjectDir + @"\solution_history", e.Population.GetTop(1)[0], APIClass.UnwalkableCellValue);
            if (PercentNoveltyChromosomesToRecieve == 0 || _recievedAlgorithmFinished)
                return;
        }




#endregion


#region POPULATION

        List<Cell> emptyCells;
        Map mapClone;
        public Chromosome CreateRandomIndividual(Map map)
        {
            //Create a map clone with empty cells
            mapClone = map.CloneJson();
            emptyCells = new List<Cell>();

            for (int y = 0; y < map.Width; y++)
            {
                for (int x = 0; x < map.Height; x++)
                {
                    Cell newEmptyCell = new Cell(x, y, APIClass.UnwalkableCellValue);
                    emptyCells.Add(newEmptyCell);
                }
            }
            mapClone.Cells = emptyCells;

            int numNewCells = Convert.ToInt32(System.Math.Round((map.Width * map.Height) / 2.0f));// RandomPopulationNumCells * 30;// map.WalkableCells.Count;

            //add the remainder as random
            for (int l = 0; l < numNewCells; l++)
            {
                var rc = mapClone.Cells[_rng.Next(mapClone.Cells.Count)]; //select random cell
                if (rc.IsWalkable)
                {
                    rc.IsWalkable = false;
                    rc.CellType = APIClass.UnwalkableCellValue;
                }
                else
                {
                    rc.IsWalkable = true;
                    rc.CellType = APIClass.WalkableCellValue;
                }
                //TODO: if neighbours are walkable, tell them there is a new neighbour
            }

            //var clonedCells = map.Cells.CloneJson();

            //place start and end points
            if (mapClone.StartPoint != null)
            {
                var tmpC = mapClone.Cells.Find(sc => (sc.X == mapClone.StartPoint.x && sc.Y == mapClone.StartPoint.y));
                tmpC.IsStartingPoint = true;
                tmpC.StartPoint = mapClone.StartPoint;
                tmpC.IsWalkable = true;
                tmpC.CellType = APIClass.WalkableCellValue;
            }
            if (mapClone.EndPointList != null && mapClone.EndPointList.Count > 0)
            {
                foreach (var ep in mapClone.EndPointList)
                {
                    var tmpC = mapClone.Cells.Find(ec => (ec.X == ep.x && ec.Y == ep.y));
                    tmpC.IsEndingPoint = true;
                    tmpC.EndPoint = ep;
                    tmpC.IsWalkable = true;
                    tmpC.CellType = APIClass.WalkableCellValue;
                }
            }
            return CreateCromosome(mapClone);
        }


        List<Chromosome> cl;
        /// <summary>
        /// Create an initial random population with the original user map included
        /// </summary>
        /// <param name="map"> Original user map </param>
        /// <param name="previousPopulation"> Previous population to be carried over to the new run </param>
        /// <returns></returns>
        public List<Chromosome> InitializePopulation(Map map)
        {

            //var can_break = false;
            //var rng = GAF.Threading.RandomProvider.GetThreadRandom();
            cl = new List<Chromosome>();
            //Add the original map
            //var tmpMap = map.CloneJson();
            //tmpMap.Id = 0;
            //Chromosome _initialChromosome = CreateCromosome(tmpMap);

            //var userSketchChromosomes = System.Math.Ceiling(InitialPopulationSize * (PercentUserSketchInfluence/100f));

            //for (int i = 0; i < userSketchChromosomes; i++)
            //{
            //    cl.Add(_initialChromosome);
            //    Debug.WriteLine("[ObjAlg] Added user chromosome.");
            //}

            //Calculate how many new chromosomes we need to create
            var numChromosomeToCreate = InitialPopulationSize;// -userSketchChromosomes;
            //if (numChromosomeToCreate < 0) //cap it so we dont have neg number
            //    numChromosomeToCreate = 0;


            //RANDOM INDIVIDUAL CELL MUTATIONS
            //create i new mutations
            for (int i = 0; i < numChromosomeToCreate; i++)
            {
                //Create a map clone with empty cells
                mapClone = map.CloneJson();
                emptyCells = new List<Cell>();

                for (int y = 0; y < map.Width; y++){
                    for (int x = 0; x < map.Height; x++){
                        Cell newEmptyCell = new Cell(x, y, APIClass.UnwalkableCellValue);
                        emptyCells.Add(newEmptyCell);
                    }
                }
                mapClone.Cells = emptyCells;

                //TODO try more or less cells (similar to existing walkable cells from user sketch?)
                //num cells to generate for each chromosome 
                int numNewCells = Convert.ToInt32(System.Math.Round((map.Width * map.Height) / 2.0f));// RandomPopulationNumCells * 30;// map.WalkableCells.Count;

                numNewCells = 1024;

                //add the remainder as random
                for (int l = 0; l < numNewCells; l++)
                {
                    var rc = mapClone.Cells[_rng.Next(mapClone.Cells.Count)]; //select random cell
                    if (rc.IsWalkable)
                    {
                        rc.IsWalkable = false;
                        rc.CellType = APIClass.UnwalkableCellValue;
                    }
                    else
                    {
                        rc.IsWalkable = true;
                        rc.CellType = APIClass.WalkableCellValue;
                    }
                    //TODO: if neighbours are walkable, tell them there is a new neighbour
                }

                //var clonedCells = map.Cells.CloneJson();

                //place start and end points
                if (mapClone.StartPoint == null)
                {
                    //var randomCell = mapClone.Cells[rng.Next(mapClone.Cells.Count)]; //select a walkable cell
                    ////var candidateStartPoint = mapClone.Cells.Find(cell => (cell.X == randomCell.X && cell.Y == randomCell.Y)); //select the same from the cloned cells
                    //int randomDir = rng.Next(4);
                    //StartingPoint sp = new StartingPoint("starting_location", randomCell.X, randomCell.Y, randomDir, 0, "random_starting_point"); //create a new starting point
                    //mapClone.StartPoint = sp;
                    //randomCell.IsStartingPoint = true;
                    //randomCell.StartPoint = sp;
                    //randomCell.IsWalkable = true;
                    //randomCell.CellType = APIClass.WalkableCellValue;
                }
                else //create walkable cell and set it as starting point?? vs pick another cell?
                {
                    var tmpC = mapClone.Cells.Find(sc => (sc.X == mapClone.StartPoint.x && sc.Y == mapClone.StartPoint.y));
                    tmpC.IsStartingPoint = true;
                    tmpC.StartPoint = mapClone.StartPoint;
                    tmpC.IsWalkable = true;
                    tmpC.CellType = APIClass.WalkableCellValue;
                }
                if (mapClone.EndPointList == null || (mapClone.EndPointList != null && mapClone.EndPointList.Count == 0))
                {
                    //mapClone.EndPointList = new List<EndingPoint>();
                    ////var randomCell = mapClone.WalkableCells[rng.Next(mapClone.WalkableCells.Count)]; //select a walkable cell
                    //var randomCell = mapClone.Cells[rng.Next(map.Width * map.Height)]; //select a RANDOM cell location
                    ////var candidateEndPoint = clonedCells.Find(cell => (cell.X == randomCell.X && cell.Y == randomCell.Y)); //select the same from the cloned cells
                    //int randomDir = rng.Next(4);
                    //EndingPoint ep = new EndingPoint("castle_arena_stairs_down", randomCell.X, randomCell.Y, randomDir, 0, "random_ending_point_" + mapClone.EndPointList.Count + 1); //create a new ending point
                    //randomCell.IsEndingPoint = true;
                    //randomCell.EndPoint = ep;
                    //if (!randomCell.IsWalkable)
                    //{
                    //    randomCell.IsWalkable = true;
                    //    randomCell.CellType = APIClass.WalkableCellValue;
                    //}
                    //mapClone.EndPointList.Add(ep);
                }
                else
                {
                    foreach (var ep in mapClone.EndPointList)
                    {
                        var tmpC = mapClone.Cells.Find(ec => (ec.X == ep.x && ec.Y == ep.y));
                        tmpC.IsEndingPoint = true;
                        tmpC.EndPoint = ep;
                        tmpC.IsWalkable = true;
                        tmpC.CellType = APIClass.WalkableCellValue;
                    }
                }
                //Logger.AppendText("Starting point @ " + mapClone.StartPoint.X + "," + mapClone.StartPoint.Y);
                //Logger.AppendText("Ending point @ " + mapClone.EndPointList[0].X + "," + mapClone.EndPointList[0].Y);

                //var newC = CreateCromosome(mapClone);
                //var newC = CreateBaseCromosome(mapClone, 70);
                //Debug.WriteLine("Chromosome disturbance created: " + numChanges);
                cl.Add(CreateCromosome(mapClone));
                //WriteUtilities.WriteMutations(i, mutationDir, newC, map.GroundFirstIndex);
                //WriteUtilities.WriteSolution(i, DirectoryManager.ProjectDir + @"\solution_history", newC, APIClass.UnwalkableCellValue);

            }




            return cl;
        }

        List<Chromosome> tmpObjectivePopulation;
        private List<Chromosome> CreatePopulationFromPrevious(Map currMap, Population prevObjPop, Population novPop)
        {
            //var rng = GAF.Threading.RandomProvider.GetThreadRandom();
            tmpObjectivePopulation = new List<Chromosome>();

            //CHECK THIS
            int numUserSketchChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentUserSketchInfluence));
            int numNoveltyChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentNoveltyChromosomesToRecieve));
            int numObjectiveChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentObjectiveChromosomesToKeep));//InitialPopulationSize - numUserSketchChromosomes - numNoveltyChromosomes;

            if (NextPopulationCarryMethod == PopulationCarryMethod.Random)
            {
                //var previousPopulationCopy = prevObjPop.CloneJson();

                var novChromosomesToAdd = new List<Chromosome>();
                var userChromosomesToAdd = new List<Chromosome>();
                if (numUserSketchChromosomes > 0)
                {
                    for (int i = 0; i < numUserSketchChromosomes; i++)
                    {
                        var userChromosome = CreateCromosome(currMap);
                        //previousPopulationCopy.Solutions.RemoveAt(rng.Next(previousPopulationCopy.Solutions.Count));
                        userChromosomesToAdd.Add(userChromosome);
                    }
                }
                if (numNoveltyChromosomes > 0)
                {
                    List<Chromosome> tmpNoveltyPopulation = NoveltyPopulation.Solutions;
                    for (int j = 0; j < numNoveltyChromosomes; j++)
                    {
                        int pos = _rng.Next(tmpNoveltyPopulation.Count);
                        var novChromosome = new Chromosome(tmpNoveltyPopulation[pos].Genes);
                        tmpNoveltyPopulation.RemoveAt(pos);
                        //previousPopulationCopy.Solutions.RemoveAt(rng.Next(previousPopulationCopy.Solutions.Count));
                        novChromosomesToAdd.Add(novChromosome);
                    }
                }
                //ONLY ADD THEM LATER, SO SE DONT REMOVE THE ONES WE JUST ADDED, BY ACCIDENT
                //if (userChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(userChromosomesToAdd);
                //if (novChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(novChromosomesToAdd);



                if (userChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(userChromosomesToAdd);
                if (novChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(novChromosomesToAdd);

                var previousPopulationCopy = prevObjPop.CloneJson();

                if (numObjectiveChromosomes > 0)
                {
                    for (int k = 0; k < numObjectiveChromosomes; k++)
                    {
                        var r = _rng.Next(previousPopulationCopy.Solutions.Count);
                        var c = previousPopulationCopy.Solutions.ElementAt(k);
                        tmpObjectivePopulation.Add(new Chromosome(c.Genes));
                        previousPopulationCopy.Solutions.Remove(c);
                    }
                }

            }
            else if (NextPopulationCarryMethod == PopulationCarryMethod.TopPercent)
            {
                //prevObjPop.Solutions.RemoveRange(numObjectiveChromosomes, prevObjPop.Solutions.Count - numNoveltyChromosomes);

                var userChromosomesToAdd = new List<Chromosome>();
                var novChromosomesToAdd = new List<Chromosome>();
                if (numUserSketchChromosomes > 0)
                {
                    //if(prevObjPop.Solutions.Count > 0 && numUserSketchChromosomes < prevObjPop.Solutions.Count)
                    //    foreach (var c in prevObjPop.GetBottom(numUserSketchChromosomes))
                    //        prevObjPop.Solutions.Remove(c);
                    for (int i = 0; i < numUserSketchChromosomes; i++)
                    {
                        var userChromosome = CreateCromosome(currMap);
                        //prevObjPop.DeleteLast();
                        //prevObjPop.Solutions.Add(userChromosome);
                        userChromosomesToAdd.Add(userChromosome);
                    }
                }
                if (numNoveltyChromosomes > 0)
                {
                    var topNov = novPop.GetTop(numNoveltyChromosomes);
                    //if (prevObjPop.Solutions.Count > 0 && numNoveltyChromosomes < prevObjPop.Solutions.Count)
                    //{
                    //    topNov = novPop.GetTop(numNoveltyChromosomes);
                    //    foreach (var c in prevObjPop.GetBottom(numNoveltyChromosomes))
                    //        prevObjPop.Solutions.Remove(c);
                    //}
                    for (int j = 0; j < numNoveltyChromosomes; j++)
                    {
                        var novChromosome = new Chromosome(topNov[j].Genes);
                        //prevObjPop.DeleteLast();
                        //prevObjPop.Solutions.Add(novChromosome);
                        novChromosomesToAdd.Add(novChromosome);
                    }
                }
                //if (userChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(userChromosomesToAdd);
                //if (novChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(novChromosomesToAdd);


                if (userChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(userChromosomesToAdd);
                if (novChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(novChromosomesToAdd);

                if (numObjectiveChromosomes > 0) { 
                    foreach (var c in prevObjPop.GetTop(numObjectiveChromosomes))
                    {
                        tmpObjectivePopulation.Add(new Chromosome(c.Genes));
                    }
                }

            }
            
            //int v = 0;
            //foreach (var c in prevObjPop.Solutions) //create the new population by creating new chromosome (without fitness) from the previous pop
            //{
            //    var tmpC = new Chromosome(c.Genes);
            //    //WriteUtilities.WriteSolution(v++,DirectoryManager.PopulationSaveDir,tmpC, APIClass.UnwalkableCellValue);
            //    tmpObjectivePopulation.Add(tmpC);
            //}           

            return tmpObjectivePopulation;
        }

        private List<Chromosome> CreatePopulationFromPrevious2(Map currMap, Population prevObjPop, Population novPop)
        {
            //var rng = GAF.Threading.RandomProvider.GetThreadRandom();
            List<Chromosome> tmpObjectivePopulation = new List<Chromosome>();

            //CHECK THIS
            int numUserSketchChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentUserSketchInfluence));
            int numNoveltyChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentNoveltyChromosomesToRecieve));
            int numObjectiveChromosomes = (int)System.Math.Ceiling(InitialPopulationSize * (PercentObjectiveChromosomesToKeep));//InitialPopulationSize - numUserSketchChromosomes - numNoveltyChromosomes;

            if (NextPopulationCarryMethod == PopulationCarryMethod.Random)
            {
                //var previousPopulationCopy = prevObjPop.CloneJson();

                var novChromosomesToAdd = new List<Chromosome>();
                var userChromosomesToAdd = new List<Chromosome>();
                if (numUserSketchChromosomes > 0)
                {
                    for (int i = 0; i < numUserSketchChromosomes; i++)
                    {
                        var userChromosome = CreateCromosome(currMap);
                        //previousPopulationCopy.Solutions.RemoveAt(rng.Next(previousPopulationCopy.Solutions.Count));
                        userChromosomesToAdd.Add(userChromosome);
                    }
                }
                if (numNoveltyChromosomes > 0)
                {
                    List<Chromosome> tmpNoveltyPopulation = NoveltyPopulation.Solutions;
                    for (int j = 0; j < numNoveltyChromosomes; j++)
                    {
                        int pos = _rng.Next(tmpNoveltyPopulation.Count);
                        var novChromosome = new Chromosome(tmpNoveltyPopulation[pos].Genes);
                        tmpNoveltyPopulation.RemoveAt(pos);
                        //previousPopulationCopy.Solutions.RemoveAt(rng.Next(previousPopulationCopy.Solutions.Count));
                        novChromosomesToAdd.Add(novChromosome);
                    }
                }
                //ONLY ADD THEM LATER, SO SE DONT REMOVE THE ONES WE JUST ADDED, BY ACCIDENT
                //if (userChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(userChromosomesToAdd);
                //if (novChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(novChromosomesToAdd);



                if (userChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(userChromosomesToAdd);
                if (novChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(novChromosomesToAdd);

                var previousPopulationCopy = prevObjPop.CloneJson();

                if (numObjectiveChromosomes > 0)
                {
                    _chosenOnes.OrderBy(c => c.Fitness); //order them first by fitness ?

                    if (_chosenOnes.Count > numObjectiveChromosomes / 2.0f) //take chosen ones, add half and generate other half random
                    {
                        var chosenCopy = _chosenOnes.CloneJson();
                        var numChosen = numObjectiveChromosomes / 2.0f;

                        for (int i = 0; i < numObjectiveChromosomes; i++)
                        {
                            if (i < numChosen) //add best ones on the first half
                            {
                                var randIndex = _rng.Next(chosenCopy.Count);
                                var randChosen = chosenCopy[randIndex];
                                tmpObjectivePopulation.Add(new Chromosome(randChosen.Genes)); //create new one to clear fitness
                                chosenCopy.Remove(randChosen);
                            }
                            else //add random for the next
                            {
                                tmpObjectivePopulation.Add(CreateRandomIndividual(currMap));
                            }
                        }
                    }
                    else //add all chosen ones, generate random for the rest
                    {
                        var numRand = numObjectiveChromosomes - _chosenOnes.Count;
                        foreach (var c in _chosenOnes)
                            tmpObjectivePopulation.Add(new Chromosome(c.Genes));
                        for (int i = 0; i < numRand; i++)
                            tmpObjectivePopulation.Add(CreateRandomIndividual(currMap));
                    }
                }
            }
            else if (NextPopulationCarryMethod == PopulationCarryMethod.TopPercent)
            {
                //prevObjPop.Solutions.RemoveRange(numObjectiveChromosomes, prevObjPop.Solutions.Count - numNoveltyChromosomes);

                var userChromosomesToAdd = new List<Chromosome>();
                var novChromosomesToAdd = new List<Chromosome>();
                if (numUserSketchChromosomes > 0)
                {
                    //if(prevObjPop.Solutions.Count > 0 && numUserSketchChromosomes < prevObjPop.Solutions.Count)
                    //    foreach (var c in prevObjPop.GetBottom(numUserSketchChromosomes))
                    //        prevObjPop.Solutions.Remove(c);
                    for (int i = 0; i < numUserSketchChromosomes; i++)
                    {
                        var userChromosome = CreateCromosome(currMap);
                        //prevObjPop.DeleteLast();
                        //prevObjPop.Solutions.Add(userChromosome);
                        userChromosomesToAdd.Add(userChromosome);
                    }
                }
                if (numNoveltyChromosomes > 0)
                {
                    var topNov = novPop.GetTop(numNoveltyChromosomes);
                    //if (prevObjPop.Solutions.Count > 0 && numNoveltyChromosomes < prevObjPop.Solutions.Count)
                    //{
                    //    topNov = novPop.GetTop(numNoveltyChromosomes);
                    //    foreach (var c in prevObjPop.GetBottom(numNoveltyChromosomes))
                    //        prevObjPop.Solutions.Remove(c);
                    //}
                    for (int j = 0; j < numNoveltyChromosomes; j++)
                    {
                        var novChromosome = new Chromosome(topNov[j].Genes);
                        //prevObjPop.DeleteLast();
                        //prevObjPop.Solutions.Add(novChromosome);
                        novChromosomesToAdd.Add(novChromosome);
                    }
                }
                //if (userChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(userChromosomesToAdd);
                //if (novChromosomesToAdd.Count > 0)
                //    prevObjPop.Solutions.AddRange(novChromosomesToAdd);


                if (userChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(userChromosomesToAdd);
                if (novChromosomesToAdd.Count > 0)
                    tmpObjectivePopulation.AddRange(novChromosomesToAdd);

                if (numObjectiveChromosomes > 0)
                {
                    _chosenOnes.OrderBy(c => c.Fitness); //order them first by fitness

                    if(_chosenOnes.Count > numObjectiveChromosomes/2.0f) //take chosen ones, add half and generate other half random
                    {
                        for(int i = 0; i<numObjectiveChromosomes; i++)
                        {
                            if (i < numObjectiveChromosomes / 2.0f) //add best ones on the first half
                            {
                                var chosen = _chosenOnes[i];
                                tmpObjectivePopulation.Add(new Chromosome(chosen.Genes)); //create new one to clear fitness
                            }
                            else //add random for the next
                            {
                                tmpObjectivePopulation.Add(CreateRandomIndividual(currMap));
                            }
                        }
                    }
                    else //add all chosen ones, generate random for the rest
                    {
                        var numRand = numObjectiveChromosomes - _chosenOnes.Count;
                        foreach(var c in _chosenOnes)
                            tmpObjectivePopulation.Add(new Chromosome(c.Genes));
                        for (int i = 0; i < numRand; i++)
                            tmpObjectivePopulation.Add(CreateRandomIndividual(currMap));
                    }
                }

            }

            //int v = 0;
            //foreach (var c in prevObjPop.Solutions) //create the new population by creating new chromosome (without fitness) from the previous pop
            //{
            //    var tmpC = new Chromosome(c.Genes);
            //    //WriteUtilities.WriteSolution(v++,DirectoryManager.PopulationSaveDir,tmpC, APIClass.UnwalkableCellValue);
            //    tmpObjectivePopulation.Add(tmpC);
            //}           

            return tmpObjectivePopulation;
        }

#endregion



#region CHROMOSOME


        /// <summary>
        /// Creates a new chromosome from an ArrayList representing a map
        /// </summary>
        /// <param name="map"></param>
        public Chromosome CreateCromosome(Map map)
        {
            Chromosome newChromosome = new Chromosome();

            foreach (Cell cell in map.Cells)
            {
                //b.Append(cell);
                cell.Id = map.Id;
                newChromosome.Genes.Add(new Gene(cell));
            }

            return newChromosome;
        }


#endregion


        /// <summary>
        /// Starts an [Async] Objective-centric Genetic Algorithm with the specified parameters
        /// </summary>
        public void Run()
        {
            Debug.WriteLine("Executing objective GA.");
            Logger.AppendText("Executing objective GA.");
            _ga.Run(TerminateFunction); // runasync doesnt work so well? (needed for )
            Running = true;
        }



        static internal void NoveltyAlgorithm_Finished(object sender, AlgorithmEventArgs e)
        {
            Debug.WriteLine("Received novelty pop!");
            NoveltyPopulation = e._p;
            lock (MainForm.fileLock)
            {
                _recievedAlgorithmFinished = true;
                MainForm.objectiveSyncHandle.Set();
            }
        }

    }



    public class AlgorithmEventArgs : EventArgs
    {
        public Population _p;
        public AlgorithmEventArgs(Population p) { _p = p; }
    }


}