using GAF;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Log2CyclePrototype
{
    public class Core
    {
        /***************** CORE **********************/
        private Hook hook;
        private FileWatcher fileWatcher;
        private UserSelection userSelection;

        private Map originalMap;
        private List<Map> suggestionsMap;

        bool validDirectory = false;

        public bool hasMap() { if (originalMap == null) return false; else return true; }
        public bool hasSuggestionMap() { if (suggestionsMap.Count == 0) return false; else return true; }
        public Map getMap() { return originalMap; }
        public Map getSuggestionMap() { return suggestionsMap[suggestionsMap.Count - 1]; }
        public UserSelection getUserSelection() { return userSelection; }
        

        public Core(Monsters window)
        {
            this.interfaceWindow = window;

            hook = new Hook(this);
            fileWatcher = new FileWatcher();
            userSelection = new UserSelection();

            suggestionsMap = new List<Map>();
        }

        public bool loadDirectory(string folderName)
        {
            var dirContents = new DirectoryInfo(folderName);
            var files = dirContents.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.ToString().Contains(".dungeon_editor"))
                {
                    Logger.AppendText("Directory is a valid LoG2 Project directory.");
                    validDirectory = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                    {

                        DirectoryManager.ProjectDir = folderName;

                        interfaceWindow.Invoke((MethodInvoker)(() => { hook.Start(); }));
                        fileWatcher.Start();
                        originalMap = APIClass.ParseMapFile();
                        objAlgTest = new ObjectiveAlgorithmTestClass();
                        novAlgTest = new InnovationAlgorithm();
                        //_cellsToDraw = APIClass.CurrentMap.Cells;
                        //Redraw();
                        //NoveltyAlgorithmTestClass.OnNoveltyAlgorithmFinished += ObjectiveAlgorithmTestClass.NoveltyAlgorithm_Finished;
                        ResetAlgorithm();
                        Logger.AppendText("Starting first algorithm run...");
                        RunAlgorithm();
                        interfaceWindow.ReDraw();
                    }));
                    break;
                }
            }
            return validDirectory;
        }


        /*********************************************/

        float objectivePercentage, innovationPercentage, userPercentage;
        int maxMonsters, characterLevel;
        float hordesPercentage, mapObjectsPercentage, endPointsPercentage;
        private Monsters interfaceWindow;

        private bool keepPopulation = false;
        private bool innovationRunning = false, objectiveRunning = false;
        private delegate void AlgorithmRunComplete(List<Gene> s);

        private ObjectiveAlgorithmTestClass objAlgTest;
        private InnovationAlgorithm novAlgTest;

        /*******************************************************/
        /***************Algorithms Percentage*******************/
        /*******************************************************/
        public float ObjectivePercentage
        {
            get { return objectivePercentage; }
            set { objectivePercentage = value; Logger.AppendText("ObjectivePercentage: " + value); }
        }
        public float InnovationPercentage
        {
            get { return innovationPercentage; }
            set { innovationPercentage = value; Logger.AppendText("InnovationPercentage: " + value); }
        }
        public float UserPercentage
        {
            get { return userPercentage; }
            set { userPercentage = value; Logger.AppendText("UserPercentage: " + value); }
        }

        /*******************************************************/

        /*******************************************************/
        /*********************Objective*************************/
        /*******************************************************/
        public int MaxMonsters
        {
            get { return maxMonsters; }
            set { maxMonsters = value; Logger.AppendText("MaxMonsters: " + value); }
        }
        public int CharacterLevel
        {
            get { return characterLevel; }
            set { characterLevel = value; Logger.AppendText("CharacterLevel: " + value); }
        }
        public float HordesPercentage
        {
            get { return hordesPercentage; }
            set { hordesPercentage = value; Logger.AppendText("HordesPercentage: " + value); }
        }
        public float MapObjectsPercentage
        {
            get { return mapObjectsPercentage; }
            set { mapObjectsPercentage = value; Logger.AppendText("MapObjectsPercentage: " + value); }
        }
        public float EndPointsPercentage
        {
            get { return endPointsPercentage; }
            set { endPointsPercentage = value; Logger.AppendText("EndPointsPercentage: " + value); }
        }

        /*******************************************************/

        /*******************************************************/
        /***************Algorithm Paramaters********************/
        /*******************************************************/
        public bool KeepPopulation { get; set; }
        public bool RandomTransferPopulation { get; set; }

        public int InitialPopulationObjective { get; set; }
        public int MutationPercentageObjective { get; set; }
        public int GenerationsObjective { get; set; }
        public int ElitismPercentageObjective { get; set; }

        public int InitialPopulationInnovation { get; set; }
        public int MutationPercentageInnovation { get; set; }
        public int GenerationsInnovation { get; set; }
        public int ElitismPercentageInnovation { get; set; }

        public CrossoverType CrossoverType { get; set; }

        /*******************************************************/

        /***********************************************************************************/

        private void RunAlgorithm()
        {
            if (innovationRunning || objectiveRunning)
                return;

            if (innovationPercentage == 0 && objectivePercentage == 0 && userPercentage == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            if (!keepPopulation)
            {
                objAlgTest.ResetOnNextRun = true;
                //novAlgTest.ResetOnNextRun = true;
            }

            AlgorithmRunComplete callback = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            //OBJECTIVE SETUP
            objAlgTest.InitialPopulationSize = InitialPopulationObjective;
            objAlgTest.GenerationLimit = GenerationsObjective;
            objAlgTest.PercentUserSketchInfluence = UserPercentage;
            if (RandomTransferPopulation)
                objAlgTest.NextPopulationCarryMethod = PopulationCarryMethod.Random;
            else
                objAlgTest.NextPopulationCarryMethod = PopulationCarryMethod.TopPercent;
            objAlgTest.PercentNoveltyChromosomesToRecieve = InnovationPercentage;
            objAlgTest.PercentObjectiveChromosomesToKeep = ObjectivePercentage;
            objAlgTest.CrossoverTypeSelected = CrossoverType;
            objAlgTest.PercentMutation = MutationPercentageObjective / 100.0;
            objAlgTest.PercentElitism = ElitismPercentageObjective / 100.0;
            objAlgTest.UserSelectionPositiveFocus = userSelection.getSelectedPoints();
            //objAlgTest.CurrentObjective = Objective.NarrowPaths; //FIXME

            objAlgTest.NewObjectiveRun(originalMap, callback); //setup


            //NOVELTY SETUP
            novAlgTest.InitialPopulation = InitialPopulationInnovation;
            novAlgTest.GenerationLimit = GenerationsInnovation;
            //novAlgTest.PercentChromosomesToInject = InnovationPercentage;
            novAlgTest.MutationPercentage = MutationPercentageInnovation / 100.0;
            novAlgTest.ElitismPercentage = ElitismPercentageInnovation;
            novAlgTest.CrossoverType = CrossoverType;
            //novAlgTest.UserSelectionPositiveFocus = userSelection.getSelectedPoints();

            //NoveltyAlgorithmTestClass.OnNoveltyAlgorithmFinished += ObjectiveAlgorithmTestClass.NoveltyAlgorithm_Finished;
            //ObjectiveAlgorithmTestClass.OnGenerationTerminated += NoveltyAlgorithmTestClass.ObjectiveAlgorithm_GenerationEnd;
            //ObjectiveAlgorithmTestClass.OnObjectiveAlgorithmComplete += NoveltyAlgorithmTestClass.ObjectiveAlgorithm_Finished;

            //novAlgTest.NewNoveltyRun(originalMap, callback); // setup


            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                objectiveRunning = true;
                //objAlgTest.Run();
                objectiveRunning = false;
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                innovationRunning = true;
                novAlgTest.Run(originalMap);
                innovationRunning = false;
            }));
        }

        bool algorithmsInitialized = false;
        private bool justReset = true;

        void AlgorithmRunCompleteCallback(List<Gene> solution)
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");
            if (!algorithmsInitialized)
            {
                algorithmsInitialized = true; //ignore the initialization solution
                return;
            }

            Map tmpMap = APIClass.MapObjectFromChromosome(new Chromosome(solution)); //create map from chromosome. should pass genes?
            suggestionsMap.Add(tmpMap.CloneJson());
            //_cellsToDraw = solutionChromosomeMap.Cells;
            justReset = false;
        }


        

        private void ResetAlgorithm()
        {
            if (justReset)
                return;

            if (hasMap())
                return;
            algorithmsInitialized = false;
            
            objAlgTest.ResetOnNextRun = true;
            //novAlgTest.ResetOnNextRun = true;


            //_cellsToDraw = APIClass.CurrentMap.Cells;
            //ReDraw();

            //RunAlgorithm();
            justReset = true;
        }

        /*private void ParseMapAndRunAlgorithm()
        {
            if (objRunning || novRunning)
                return;

            try
            {
                currentMap = APIClass.ParseMapFile();

                if (gridPanel.InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { ReDraw(); })); //needed when calling the callback from a different thread
                }
                else
                {
                    ReDraw();
                }

                var dif = APIClass.CalculateDifference(previousMap, currentMap);

                if (dif > 0.0 && dif < 500)
                {
                    //Logger.AppendText("Dif:" + dif.ToString());
                    if (currentMap.EndPointList == null)
                        Logger.AppendText("WARNING: No ending points detected.");

                    if (currentMap.StartPoint == null)
                        Logger.AppendText("WARNING: No start point detected.");
                    RunAlgorithm();
                }
                previousMap = currentMap;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }*/

    }
}
