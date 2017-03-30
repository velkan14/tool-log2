using GAF;
using GAF.Operators;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    class NoveltyAlgorithmTestClass
    {
        public static List<Chromosome> ChromosomesToReceive { get; set; }

        private static string fileDir = @"D:\GOG Games\Custom Levels\test4\mutations";

        public static event EventHandler<AlgorithmEventArgs> OnNoveltyAlgorithmFinished;


        public static bool _recievedAlgorithmFinished;
        public static bool _recievedGenerationEndEvent;
        private GeneticAlgorithm _ga;
        private Population _population;
        private Elite _elite;
        //private CustomCrossover _cc;
        private CustomCrossover _customCrossover;
        private CustomSwapMutate _cs;
        private Crossover _crossover;
        private SwapMutate _mutate;
        private Delegate _callback;
        private Map _currentMap, _originalMap;
        private double _userDisturbance, _targetDisturbance;
        private Population _previousPop;
        private bool _sentFinishEvent;
        private Random _rng;
        

        public static bool Running { get; set; }
        public int GenerationLimit { get; set; }
        public int InitialPopulationSize { get; set; }
        public float PercentChromosomesToInject { get; set; }
        public int NumChromosomesToRecieve { get; set; }
        public CrossoverType CrossoverTypeSelected { get; set; }
        public bool Initialized { get; internal set; }
        public double PercentElitism { get; set; }
        public double PercentMutation { get; set; }
        public List<Point> UserSelectionPositiveFocus { get; set; }
        public float UserSelectionWeight { get; set; } = 0.15f;
        public bool ResetOnNextRun = false;



        public NoveltyAlgorithmTestClass() {
            Running = false;
            GenerationLimit = 60;
            InitialPopulationSize = 20;
            PercentChromosomesToInject = 0;
            NumChromosomesToRecieve = 0;
            PercentMutation = 0.15;
            PercentElitism = 0.15;
            Initialized = false;
            _sentFinishEvent = false;
            Initialized = false;
            UserSelectionPositiveFocus = new List<Point>();
            _rng = GAF.Threading.RandomProvider.GetThreadRandom();

        }

        public NoveltyAlgorithmTestClass(Population initPop)
        {
            _previousPop = initPop;

            Running = false;
            GenerationLimit = 60;
            InitialPopulationSize = 20;
            PercentChromosomesToInject = 0;
            NumChromosomesToRecieve = 0;
            PercentMutation = 0.15;
            PercentElitism = 0.15;
            Initialized = false;
            _sentFinishEvent = false;
            Initialized = true;
            UserSelectionPositiveFocus = new List<Point>();
            _rng = GAF.Threading.RandomProvider.GetThreadRandom();

        }

        public void NewNoveltyRun(Map map, Delegate onCompleteFuntion)
        {
            
            Debug.WriteLine("Setup started.");
            ChromosomesToReceive = null;
            _recievedAlgorithmFinished = false;
            _recievedGenerationEndEvent = false;
            _sentFinishEvent = false;

            _originalMap = map;
            _currentMap = CloneUtilities.CloneJson<Map>(map);

            _population = new Population();
            //randomly generated population 
            //population = new Population(POPULATION_SIZE, CHROMOSOME_SIZE);
            if (_previousPop != null && !ResetOnNextRun)
            {

                foreach (var c in _previousPop.Solutions) //because they keep fitness value!
                {
                    var tmpC = new Chromosome(c.Genes);
                    _population.Solutions.Add(tmpC);   
                }
            }            
            else
            {
                _population.Solutions.AddRange(InitializePopulation(_currentMap, null));
                Initialized = true;
                ResetOnNextRun = false;
            }


            //create GA based on population generated
            _ga = new GeneticAlgorithm(_population, CalculateNoveltyFitness);

            //add GA operators
            AddOperators();

            _ga.OnGenerationComplete += OnNoveltyGenerationComplete;
            _ga.OnRunComplete += OnNoveltyComplete;



            Debug.WriteLine("Novelty Setup Complete.");
        }



        #region GENETIC OPERATORS

        /// <summary>
        /// Sets up the operators to be used during the evolutionary runs
        /// </summary>
        private void AddOperators()
        {
            //_elite = new Elite((int)(PercentElitism*100)); //param: elitism percentage (int)

            //_crossover = new Crossover(0.85)
            //{
            //    CrossoverType = CrossoverType.DoublePoint
            //};
            //_cc = new CustomCrossover(0.85, true, CustomCrossover.Crossover2DShape.FourByFourSquare);
            ////mutate = new BinaryMutate(0.04);
            //_mutate = new SwapMutate(PercentMutation); //quite high
            //_cs = new CustomSwapMutate(PercentMutation);

            //_ga.Operators.Add(_elite);
            //_ga.Operators.Add(_cc);
            ////_ga.Operators.Add(_mutate);
            //_ga.Operators.Add(_cs);



            //elitism
            _elite = new Elite((int)(PercentElitism * 100)); //param: elitism percentage (int)
            _ga.Operators.Add(_elite);


            //crossover
            switch (CrossoverTypeSelected)
            {
                case CrossoverType.TwoByTwoSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.TwoByTwoSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverType.ThreeByThreeSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.ThreeByThreeSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverType.FourByFourSquare:
                    _customCrossover = new CustomCrossover(0.8, true, CustomCrossover.Crossover2DShape.FourByFourSquare);
                    _ga.Operators.Add(_customCrossover);
                    break;
                case CrossoverType.SinglePoint:
                    _crossover = new Crossover(0.8)
                    {
                        CrossoverType = GAF.Operators.CrossoverType.SinglePoint
                    };
                    _ga.Operators.Add(_crossover);
                    break;
                case CrossoverType.DoublePoint:
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
                Debug.WriteLine("Novelty Algorithm terminated!");
                Logger.AppendText("Novelty Algorithm terminated!");
                //Console.WriteLine(ga.UseMemory);
                Running = false;
                return true;
            }
            else return false;
        }


        private double normalize(double min, double max, double value)
        {
            return (value - min) / (max - min);
        }

        int TargetWalkableCellCount = 600;
        private double CalculateNoveltyFitness(Chromosome chromosome)
        {
            double fitness = 0;
            var chromosomeGenes = chromosome.Genes;
            float cellOffsetPenalty = 0;

            //var startEndPoints = SearchUtils.FindStartEndPointsCoordinates(chromosome); //start e end points teem de ser walkable!!
            //if (startEndPoints == null)
            //{
            //    Debug.WriteLine("POINTS NULL");
            //    return 0.0;
            //}

            var start = _currentMap.StartPoint;
            var end = _currentMap.EndPointList[0];
            
                        
            ////make sure start and at least 1 end point are walkable
            //if (!startEndPoints[0].IsWalkable || !startEndPoints[1].IsWalkable)
            //{
            //    Debug.WriteLine("POINTS NOT WALKABLE");
            //    return 0.0;
            //}

            for (int i = 0; i < chromosomeGenes.Count; i++)
            {
                //grab a cell from the candidate chromosome
                Cell c = (Cell)chromosomeGenes[i].ObjectValue;
                int cellX = c.X, cellY = c.Y;

                if ((start.x == cellX && start.y == cellY) || (end.X == cellX && end.Y == cellY))
                    if (!c.IsWalkable)
                        return 0.0;

                if (_originalMap.Cells[i].CellType != c.CellType)
                {
                    //Debug.WriteLine("Derp");
                    //Logger.AppendText("cenas");
                    fitness += 1;
                }
            }

            fitness = fitness / 1024;

            //if (UserSelectionPositiveFocus.Count > 0) //Take into account the affect of user selection on fitness function
            //{
            //    int dif = 0;
            //    //fetch all cells in selection
            //    foreach (var p in UserSelectionPositiveFocus)
            //    {
            //        Cell c = (Cell)chromosome.Genes[p.Y * _currentMap.Width + p.X].ObjectValue;
            //        if (c != null)
            //        {
            //            if (c.CellType != _currentMap.GetCellAt(c.X, c.Y).CellType)
            //                dif++;
            //        }
            //        else Debug.WriteLine("CELL NULL");
            //    }
            //    float changePercent = dif / (float)UserSelectionPositiveFocus.Count;
            //    //Logger.AppendText(changePercent.ToString());
            //    fitness += (changePercent * fitness) * UserSelectionWeight;
            //}

            //Debug.WriteLine("Chromosome " + chromosome.Id + " fitness: " + fitness);
            //normalize with max: walkable cell or whole map cells?

            cellOffsetPenalty = System.Math.Abs(TargetWalkableCellCount - APIClass.CountWalkableCellsInChromosome(chromosome)) / 1024f;
            fitness -= cellOffsetPenalty;

            return fitness;
        }


        private void OnNoveltyComplete(object sender, GaEventArgs e)
        {

            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];
            
            _previousPop = new Population();
            //e.Population.Solutions.Sort();
            foreach(var c in e.Population.Solutions){
                var tmpC = new Chromosome(c.Genes);
                _previousPop.Solutions.Add(tmpC);
            }
            //MainForm.PreviousNoveltyPopulation = _previousPop;

            if (!_sentFinishEvent)
            {
                OnNoveltyAlgorithmFinished(this, new AlgorithmEventArgs(_previousPop));
                _sentFinishEvent = true;
            }

            Debug.WriteLine("Best Novelty Fitness: " + chromosome.Fitness);
            //Logger.AppendText("Best Novelty Fitness: " + chromosome.Fitness);
            //PrintSolution(chromosome);
        }



        private void OnNoveltyGenerationComplete(object sender, GaEventArgs e)
        {

            if (PercentChromosomesToInject == 0 || _recievedAlgorithmFinished)
            {
                Debug.WriteLine("Obj ended, proceding...");
                return;
            }

            //var cenas = 0;
            ////meter lock para ler valor??
            //lock (MainForm.fileLock)
            //{
            //    cenas = MainForm.syncCounter;
            //}

            ////arrived first
            //if (cenas == 0) {

            //    lock (MainForm.fileLock)
            //    {
            //        MainForm.syncCounter = 1;
            //        ObjectiveAlgorithmTestClass.ChromosomesToReceive = e.Population.GetTop(NumChromosomesToInject);
            //    }

            //    Running = false; //is paused
                
            //    Logger.AppendText("Novelty thread paused.");
            //    MainForm.noveltySyncHandle.WaitOne(); //should pause
            //    Logger.AppendText("Novelty thread resuming.");
                
            //}
            //else
            //{

            //    lock (MainForm.fileLock)
            //    {
            //        MainForm.syncCounter = 0;
            //    }

            //    //inject chromosomes
            //    ObjectiveAlgorithmTestClass.ChromosomesToReceive = e.Population.GetTop(NumChromosomesToInject);
            //    MainForm.objectiveSyncHandle.Set();
            //    MainForm.noveltySyncHandle.WaitOne();
            //}

            //Running = true;


            //InjectChromosome(chromosome);
            //Debug.WriteLine("------------- Generation Finished -------------");
            ////PrintSolution(chromosome);

        }


        #endregion


        #region POPULATION




        private List<Chromosome> InitializePopulation(Map map, List<Chromosome> previousPopulation)
        {

            //var can_break = false;
            //var rng = new Random();
            List<Chromosome> cl = new List<Chromosome>();

            //Calculate how many new chromosomes we need to create
            var numChromosomeToCreate = InitialPopulationSize;

            //RANDOM INDIVIDUAL CELL MUTATIONS
            //create i new mutations
            for (int i = 0; i < numChromosomeToCreate; i++)
            {
                //Create a map clone with empty cells
                var mapClone = map.CloneJson();

                List<Cell> emptyCells = new List<Cell>();

                for (int y = 0; y < map.Width; y++)
                {
                    for (int x = 0; x < map.Height; x++)
                    {
                        Cell newEmptyCell = new Cell(x, y, APIClass.UnwalkableCellValue);
                        emptyCells.Add(newEmptyCell);
                    }
                }
                mapClone.Cells = emptyCells;

                int numNewCells = TargetWalkableCellCount;// (int)(map.Cells.Count / 2.0f);//map.WalkableCells.Count;// map.WalkableCells.Count;

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
                if (mapClone.EndPointList == null || (mapClone.EndPointList != null && mapClone.EndPointList.Count > 0))
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
                        var tmpC = mapClone.Cells.Find(ec => (ec.X == ep.X && ec.Y == ep.Y));
                        tmpC.IsEndingPoint = true;
                        tmpC.EndPoint = ep;
                        tmpC.IsWalkable = true;
                        tmpC.CellType = APIClass.WalkableCellValue;
                    }
                }
                //Logger.AppendText("Starting point @ " + mapClone.StartPoint.X + "," + mapClone.StartPoint.Y);
                //Logger.AppendText("Ending point @ " + mapClone.EndPoint.X + "," + mapClone.EndPoint.Y);

                var newC = CreateCromosome(mapClone);
                //var newC = CreateBaseCromosome(mapClone, 70);
                //Debug.WriteLine("Chromosome disturbance created: " + numChanges);
                cl.Add(newC);
                //WriteUtilities.WriteMutations(i, mutationDir, newC, map.GroundFirstIndex);
            }

            return cl;
        }


        #endregion


        #region CHROMOSOME


        /// <summary>
        /// Creates a new chromosome from an ArrayList representing a map
        /// </summary>
        /// <param name="map"></param>
        private Chromosome CreateCromosome(Map map)
        {
            Chromosome newChromosome = new Chromosome();

            foreach (Cell cell in map.Cells)
            {
                //b.Append(cell);
                newChromosome.Genes.Add(new Gene(cell));
            }
            //initialChromosome = new Chromosome(b.ToString()); //save initial chromosome?
            //return initialChromosome;
            //Cell t = (newChromosome.FirstGene().ObjectValue) as Cell;
            //Debug.WriteLine(t.CellType);

            return newChromosome;
        }
        

        #endregion



        public void Run()
        {
            Debug.WriteLine("Executing novelty GA.");
            Logger.AppendText("Executing novelty GA.");
            Running = true;
            _ga.Run(TerminateFunction); // runasync doesnt work so 
        }


        
        static internal void ObjectiveAlgorithm_GenerationEnd(object sender, EventArgs e)
        {
            Debug.WriteLine("Obj gen end event received!");
            //throw new NotImplementedException();
            _recievedGenerationEndEvent = true;
            if (!Running) //caso o evento chegue enquanto ainda esta a correr
            {
                MainForm.noveltySyncHandle.Set(); //release o nov quando estiverem os 2 a espera
            }
        }

        static internal void ObjectiveAlgorithm_Finished(object sender, EventArgs e)
        {
            Debug.WriteLine("Obj finished event received!");
            lock (MainForm.fileLock)
            {
                _recievedAlgorithmFinished = true;
                if(!Running)
                    MainForm.noveltySyncHandle.Set();
            }
        }
    }
}
