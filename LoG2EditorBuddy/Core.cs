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
using System.Timers;

namespace Log2CyclePrototype
{
    public class Core
    {
        /***************** CORE **********************/
        private Hook hook;
        private FileWatcher fileWatcher;
        private Monsters monsters;

        private bool algorithmRunning = false;
        private delegate void AlgorithmRunComplete();
        private InnovationPool innovationAlgorithm;
        private GuidelinePool guidelineAlgorithm;
        private ConvergencePool convergenceAlgorithm;
        private MixPool mixAlgorithm;
        
        private List<Map> suggestionsMap;
        bool validDirectory = false;

        public Map OriginalMap { get; private set; }
        public Map CurrentMap { get { return suggestionsMap[IndexMap]; } }

        public int CountSuggestions { get { return suggestionsMap.Count; } }

        public int IndexMap { get; set; }

        public bool HasMap { get { if (OriginalMap == null) return false; else return true; } }
        public bool FileChanged { get; set; }
        System.Timers.Timer timer;


        private int InnovationPercentage { get; set; }
        private int GuidelinePercentage { get; set; }
        private int UserPercentage { get; set; }
        private int NumberMonsters { get; set; }
        private int NumberItens { get; set; }
        private int HordesPercentage { get; set; }

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

        internal void NewSuggestion(int innovationPercentage, int guidelinePercentage, int userPercentage, int numberMonsters, int numberItens, int hordesPercentage)
        {
            InnovationPercentage = innovationPercentage;
            GuidelinePercentage = guidelinePercentage;
            UserPercentage = userPercentage;
            NumberMonsters = numberMonsters;
            NumberItens = numberItens;
            HordesPercentage = hordesPercentage;

            if (algorithmRunning)
                return;

            if (innovationPercentage == 0 && guidelinePercentage == 0 && userPercentage == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            monsters.ResetProgress();
            RunAlgorithm(innovationPercentage, guidelinePercentage, userPercentage, numberMonsters, numberItens, hordesPercentage);
        }

        public Core(Monsters window, int innovationPercentage, int guidelinePercentage, int userPercentage, int numberMonsters, int numberItens, int hordesPercentage)
        {
            this.monsters = window;
            InnovationPercentage = innovationPercentage;
            GuidelinePercentage = guidelinePercentage;
            UserPercentage = userPercentage;
            NumberMonsters = numberMonsters;
            NumberItens = numberItens;
            HordesPercentage = hordesPercentage;

            hook = new Hook(this);
            fileWatcher = new FileWatcher(this);
            timer = new System.Timers.Timer(1000 * 60);

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

                        monsters.Invoke((MethodInvoker)(() => { hook.Start(); }));
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
                FileChanged = false;
                suggestionsMap.Add(OriginalMap);

                IndexMap = suggestionsMap.Count - 1;

                monsters.MapLoaded();
                monsters.UpdateTrackHistory();
                monsters.ReDrawMap();

                if (!timer.Enabled)
                {
                    
                    timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    timer.Enabled = true;
                    timer.AutoReset = true;
                    Console.WriteLine("Timer set");
                }
                
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer event");
            if (FileChanged)
            {
                Console.WriteLine("Map Loaded");
                LoadMapFromFile();
            }
            NewSuggestion(InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);
        }

        /***********************************************************************************/

        private void RunAlgorithm(int innovationPercentage, int guidelinePercentage, int userPercentage, int numberMonsters, int numberItens, int hordesPercentage)
        {
            AlgorithmRunComplete callback = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            algorithmRunning = true;

            innovationAlgorithm = new InnovationPool(monsters);
            guidelineAlgorithm = new GuidelinePool(monsters)
            {
                MaxMonsters = numberMonsters,
                MaxItens = numberItens,
                HordesPercentage = hordesPercentage / 100.0
            };
            convergenceAlgorithm = new ConvergencePool(monsters);
            mixAlgorithm = new MixPool(monsters)
            {
                MaxMonsters = numberMonsters,
                MaxItens = numberItens,
                HordesPercentage = hordesPercentage,
                UserPercentage = userPercentage / 100.0,
                InnovationPercentage = innovationPercentage / 100.0,
                GuidelinePercentage = guidelinePercentage / 100.0
            };

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
                guidelineAlgorithm.Run(CurrentMap, monsters.AreasManager, callback);
            }));

            Logger.AppendText("Started Algorithm");
        }

        void AlgorithmRunCompleteCallback()
        {
            if(innovationAlgorithm.HasSolution && convergenceAlgorithm.HasSolution && guidelineAlgorithm.HasSolution)
            {
                var conv = convergenceAlgorithm.Solution.GetTop(1)[0];
                var inno = innovationAlgorithm.Solution.GetTop(1)[0];
                var obj = guidelineAlgorithm.Solution.GetTop(1)[0];

                /*Logger.AppendText("Innovation: " + inno.Fitness);
                Logger.AppendText("Convergence: " + conv.Fitness);
                Logger.AppendText("Objective: " + obj.Fitness);*/

                //suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, inno));
                //suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, conv));
                //suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, obj));*/

                AlgorithmRunComplete callback = new AlgorithmRunComplete(MixRunCompleteCallback);

                ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                {
                    mixAlgorithm.Run(CurrentMap, monsters.AreasManager, convergenceAlgorithm.Solution, innovationAlgorithm.Solution, guidelineAlgorithm.Solution, callback);
                }));
            }
        }

        void MixRunCompleteCallback()
        {
            Debug.WriteLine("Recieved Solution!");
            Logger.AppendText("Suggestion updated!\n");

            Chromosome c = mixAlgorithm.Solution.GetTop(1)[0];
            Logger.AppendText("Fitness: " + c.Fitness);
            suggestionsMap.Add(APIClass.MapFromChromosome(OriginalMap, c));
            IndexMap = suggestionsMap.Count - 1;
            monsters.UpdateTrackHistory();
            monsters.ReDrawMap();

            algorithmRunning = false;
        }

        internal void ReloadLOG()
        {
            hook.ReloadLOG();
            LoadMapFromFile();
        }
    }
}
