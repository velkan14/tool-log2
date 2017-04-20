using GAF;
using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System;

namespace Log2CyclePrototype
{
    public class Core
    {
        /***************** CORE **********************/
        private Hook hook;
        private FileWatcher fileWatcher;
        
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

            innovationAlgorithm = new InnovationAlgorithm();
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

                APIClass.ChromosomeFromMap(OriginalMap);

                IndexMap = suggestionsMap.Count - 1;

                interfaceWindow.MapLoaded();
                interfaceWindow.UpdateTrackHistory();
                interfaceWindow.ReDrawMap();
            }
        }


        /*********************************************/

        private Monsters interfaceWindow;
        
        private bool innovationRunning = false;
        private delegate void AlgorithmRunComplete(Chromosome solution);
        
        private InnovationAlgorithm innovationAlgorithm;

        /*******************************************************/
        /***************Algorithms Percentage*******************/
        /*******************************************************/
        public float ObjectivePercentage { get; set; }
        public float InnovationPercentage { get; set; }
        public float UserPercentage { get; set; }

        /*******************************************************/

        /*******************************************************/
        /*********************Objective*************************/
        /*******************************************************/
        public int MaxMonsters { get; set; }
        public float HordesPercentage { get; set; }
        public float MapObjectsPercentage { get; set; }

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
            if (innovationRunning)
                return;

            if (InnovationPercentage == 0 && ObjectivePercentage == 0 && UserPercentage == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            AlgorithmRunComplete callback = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            //NOVELTY SETUP
            /*innovationAlgorithm.InitialPopulation = InitialPopulationInnovation;
            innovationAlgorithm.GenerationLimit = GenerationsInnovation;
            innovationAlgorithm.MutationPercentage = MutationPercentageInnovation / 100.0;
            innovationAlgorithm.ElitismPercentage = ElitismPercentageInnovation;
            innovationAlgorithm.CrossoverType = CrossoverType;*/

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                innovationRunning = true;
                innovationAlgorithm.Run(CurrentMap, callback);
                innovationRunning = false;
            }));
        }

        void AlgorithmRunCompleteCallback(Chromosome solution)
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");

            Map tmpMap = APIClass.MapFromChromosome(OriginalMap, solution); //create map from chromosome. should pass genes?

            suggestionsMap.Add(tmpMap);
            IndexMap = suggestionsMap.Count - 1;
            interfaceWindow.UpdateTrackHistory();
            interfaceWindow.ReDrawMap();
        }

        internal void ReloadLOG()
        {
            hook.ReloadLOG();
            LoadMapFromFile();
        }
    }
}
