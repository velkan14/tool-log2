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

        public Map OriginalMap { get; private set; }
        private List<Map> suggestionsMap;

        bool validDirectory = false;

        public bool HasMap { get { if (OriginalMap == null) return false; else return true; } }
        public bool hasSuggestionMap() { if (suggestionsMap.Count == 0) return false; else return true; }
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
                        OriginalMap = APIClass.ParseMapFile();
                        innovationAlgorithm = new InnovationAlgorithm();

                        RunAlgorithm();
                        interfaceWindow.MapLoaded();
                        interfaceWindow.ReDraw();
                    }));
                    break;
                }
            }
            return validDirectory;
        }


        /*********************************************/

        private Monsters interfaceWindow;
        
        private bool innovationRunning = false;
        private delegate void AlgorithmRunComplete(List<Cell> solution);
        
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
        public int CharacterLevel { get; set; }
        public float HordesPercentage { get; set; }
        public float MapObjectsPercentage { get; set; }
        public float EndPointsPercentage { get; set; }

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
                innovationAlgorithm.Run(OriginalMap, callback);
                innovationRunning = false;
            }));
        }

        void AlgorithmRunCompleteCallback(List<Cell> solution)
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");

            Map tmpMap = APIClass.MapObjectFromChromosome(OriginalMap, solution); //create map from chromosome. should pass genes?

            OriginalMap = tmpMap; //FIXME: isto deve ir para o historico

            interfaceWindow.ReDraw();

            suggestionsMap.Add(tmpMap);
        }

    }
}
