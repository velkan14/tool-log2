using GAF;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System;
using Log2CyclePrototype.Algorithm;

namespace Log2CyclePrototype
{
    public class Core
    {
        /***************** CORE **********************/
        private Hook hook;
        private FileWatcher fileWatcher;
        private Monsters interfaceWindow;

        private bool algorithmRunning = false;
        private delegate void AlgorithmRunComplete();
        private InnovationPool innovationAlgorithm;
        private GuidelinePool objectiveAlgorithm;
        private ConvergencePool convergenceAlgorithm;
        private MixAlgorithm mixAlgorithm;
        
        private List<Map> suggestionsMap;
        bool validDirectory = false;

        public Map OriginalMap { get; private set; }
        public Map CurrentMap { get { return suggestionsMap[IndexMap]; } }

        public int CountSuggestions { get { return suggestionsMap.Count; } }

        public int IndexMap { get; set; }

        public bool HasMap { get { if (OriginalMap == null) return false; else return true; } }
        
        public void NextMap()
        {
            IndexMap++;
            if(IndexMap >= suggestionsMap.Count)
            {
                IndexMap--;
            }
        }

        public void PreviousMap()
        {
            IndexMap--;
            if(IndexMap < 0)
            {
                IndexMap = 0;
            }
        }

        internal void NewSuggestion()
        {
            RunAlgorithm();
        }

        public Core(Monsters window)
        {
            this.interfaceWindow = window;

            hook = new Hook(this);
            fileWatcher = new FileWatcher(this);

            suggestionsMap = new List<Map>();
        }

        public bool LoadDirectory(string folderName)
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

                        LoadMapFromFile();
                    }));
                    break;
                }
            }
            return validDirectory;
        }

        internal void LoadMapFromFile()
        {
            if (validDirectory)
            {
                OriginalMap = APIClass.ParseMapFile();
                suggestionsMap.Add(OriginalMap);

                IndexMap = suggestionsMap.Count - 1;

                interfaceWindow.MapLoaded();
                interfaceWindow.UpdateTrackHistory();
                interfaceWindow.ReDrawMap();
            }
        }


        /*********************************************/
       
        /*******************************************************/
        /***************Algorithms Percentage*******************/
        /*******************************************************/
        public float ObjectivePercentage { get; set; }
        public float InnovationPercentage { get; set; }
        public float UserPercentage { get; set; }

        /*******************************************************/

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

        public CrossoverT CrossoverType { get; set; }

        /*******************************************************/

        /***********************************************************************************/

        private void RunAlgorithm()
        {
            if (algorithmRunning)
                return;

            if (InnovationPercentage == 0 && ObjectivePercentage == 0 && UserPercentage == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            AlgorithmRunComplete callback = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            algorithmRunning = true;

            innovationAlgorithm = new InnovationPool();
            objectiveAlgorithm = new GuidelinePool();
            convergenceAlgorithm = new ConvergencePool();
            mixAlgorithm = new MixAlgorithm();

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                convergenceAlgorithm.Run(CurrentMap, callback);
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                innovationAlgorithm.Run(CurrentMap, callback);
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                objectiveAlgorithm.Run(CurrentMap, callback);
            }));

            Logger.AppendText("Started Algorithm");
        }

        void AlgorithmRunCompleteCallback()
        {
            if(innovationAlgorithm.HasSolution && convergenceAlgorithm.HasSolution && objectiveAlgorithm.HasSolution)
            {
                var conv = convergenceAlgorithm.Solution.GetBottom(1)[0];
                var inno = innovationAlgorithm.Solution.GetTop(1)[0];
                var obj = objectiveAlgorithm.Solution.GetTop(1)[0];

                Logger.AppendText("Innovation: " + inno.Fitness);
                Logger.AppendText("Convergence: " + conv.Fitness);
                Logger.AppendText("Objective: " + obj.Fitness);

                /*suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, inno));
                suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, conv));
                suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, obj));*/

                AlgorithmRunComplete callback = new AlgorithmRunComplete(MixRunCompleteCallback);

                ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                {
                    mixAlgorithm.Run(CurrentMap, convergenceAlgorithm.Solution, innovationAlgorithm.Solution, objectiveAlgorithm.Solution, callback);
                }));
            }
        }

        void MixRunCompleteCallback()
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");

            suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, mixAlgorithm.Solution.GetTop(1)[0]));
            IndexMap = suggestionsMap.Count - 1;
            interfaceWindow.UpdateTrackHistory();
            interfaceWindow.ReDrawMap();

            algorithmRunning = false;
        }

        internal void ReloadLOG()
        {
            hook.ReloadLOG();
            LoadMapFromFile();
        }
    }
}
