using GAF;
using Povoater.LoG2API;
using Povoater.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System;
using Povoater.Algorithm;
using System.Timers;

namespace Povoater
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

        System.Timers.Timer timer;
        private bool suggestionOnTime;

        public int InnovationPercentage { get; set; }
        public int GuidelinePercentage { get; set; }
        public int UserPercentage { get; set; }
        public int NumberMonsters { get; set; }
        public int NumberItens { get; set; }
        public int HordesPercentage { get; set; }

        public int LeverMaxMonster { get; set; }
        public int LeverMaxItem { get; set; }
        public int LeverAmountHordes { get; set; }
        public int LeverDanger { get; set; }
        public int LeverAccessibility { get; set; }

        public void NextMap()
        {
            IndexMap++;
            if (IndexMap >= suggestionsMap.Count)
            {
                IndexMap--;
            }
        }

        public void PreviousMap()
        {
            IndexMap--;
            if (IndexMap < 0)
            {
                IndexMap = 0;
            }
        }

        public bool HasNextMap()
        {
            if (IndexMap == suggestionsMap.Count - 1)
            {
                return false;
            }
            return true;
        }

        public bool HasPreviousMap()
        {
            if (IndexMap == 0)
            {
                return false;
            }
            return true;
        }

        private void AddSuggestion(Map map)
        {
            if (suggestionsMap.Count > 2)
            {
                suggestionsMap.RemoveAt(0);
            }
            suggestionsMap.Add(map);
            IndexMap = suggestionsMap.Count - 1;
            monsters.UpdateTrackHistory();
            monsters.ReDrawMap();
        }

        internal void NewSuggestion(int innovationPercentage, int guidelinePercentage, int userPercentage, int numberMonsters, int numberItens, int hordesPercentage)
        {
            if (algorithmRunning)
                return;

            if (innovationPercentage == 0 && guidelinePercentage == 0 && userPercentage == 0)
            {
                Logger.AppendText("Algorithm must have some behavior! Please set at lease one of the knobs higher than 0.");
                return;
            }

            timer.Stop();
            RunAlgorithm(innovationPercentage, guidelinePercentage, userPercentage, numberMonsters, numberItens, hordesPercentage);
        }

        public Core(Monsters window)
        {
            this.monsters = window;

            hook = new Hook(this);
            fileWatcher = new FileWatcher(this);
            timer = new System.Timers.Timer(1000 * 120);

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

                if (suggestionsMap.Count == 0)
                {
                    suggestionsMap.Add(OriginalMap);
                    IndexMap = suggestionsMap.Count - 1;
                }

                monsters.MapLoaded();
                monsters.UpdateTrackHistory();
                monsters.ReDrawMap();



                if (!timer.Enabled)
                {

                    timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    timer.Enabled = true;
                    timer.AutoReset = true;
                    Console.WriteLine("Timer set");
                } else
                {
                    Console.WriteLine("Timer not set");
                }

            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer event");

            NewSuggestion(InnovationPercentage, GuidelinePercentage, UserPercentage, NumberMonsters, NumberItens, HordesPercentage);

            suggestionOnTime = true;
        }

        /***********************************************************************************/

        private void RunAlgorithm(int innovationPercentage, int guidelinePercentage, int userPercentage, int numberMonsters, int numberItens, int hordesPercentage)
        {
            int total = LeverAccessibility + LeverAmountHordes + LeverDanger + LeverMaxItem + LeverMaxMonster;
            Logger.AppendText("Total: " + total);
            AlgorithmRunComplete callback = new AlgorithmRunComplete(AlgorithmRunCompleteCallback);

            algorithmRunning = true;

            /*if (innovationAlgorithm != null && innovationAlgorithm.HasSolution)
            {
                innovationAlgorithm = new InnovationPool(monsters, OriginalMap, callback, innovationAlgorithm.Solution);
            }
            else
            {
                innovationAlgorithm = new InnovationPool(monsters, OriginalMap, callback);
            }*/

            if (guidelineAlgorithm != null && guidelineAlgorithm.HasSolution)
            {
                guidelineAlgorithm = new GuidelinePool(monsters, OriginalMap, callback, monsters.AreasManager, numberMonsters, numberItens, hordesPercentage / 100.0, guidelineAlgorithm.Solution)
                {
                    LeverAccessibility = (double)this.LeverAccessibility / (double)total,
                    LeverAmountHordes = (double)this.LeverAmountHordes / (double)total,
                    LeverDanger = (double)this.LeverDanger / (double)total,
                    LeverMaxItem = (double)this.LeverMaxItem / (double)total,
                    LeverMaxMonster = (double)this.LeverMaxMonster / (double)total
                };
            }
            else
            {
                guidelineAlgorithm = new GuidelinePool(monsters, OriginalMap, callback, monsters.AreasManager, numberMonsters, numberItens, hordesPercentage / 100.0)
                {
                    LeverAccessibility = (double)this.LeverAccessibility / (double)total,
                    LeverAmountHordes = (double)this.LeverAmountHordes / (double)total,
                    LeverDanger = (double)this.LeverDanger / (double)total,
                    LeverMaxItem = (double)this.LeverMaxItem / (double)total,
                    LeverMaxMonster = (double)this.LeverMaxMonster / (double)total
                };
            }

            /*if (convergenceAlgorithm != null && convergenceAlgorithm.HasSolution)
            {
                convergenceAlgorithm = new ConvergencePool(monsters, OriginalMap, callback, convergenceAlgorithm.Solution);
            }
            else
            {
                convergenceAlgorithm = new ConvergencePool(monsters, OriginalMap, callback);
            }*/

            //guidelineAlgorithm = new GuidelinePool(monsters, OriginalMap, callback, monsters.AreasManager, numberMonsters, numberItens, hordesPercentage / 100.0);
            innovationAlgorithm = new InnovationPool(monsters, OriginalMap, callback);
            convergenceAlgorithm = new ConvergencePool(monsters, OriginalMap, callback);

            mixAlgorithm = new MixPool(monsters, OriginalMap, new AlgorithmRunComplete(MixRunCompleteCallback))
            {
                MaxMonsters = numberMonsters,
                MaxItens = numberItens,
                HordesPercentage = (double)hordesPercentage / 100.0,
                UserPercentage = (double)userPercentage / 100.0,
                InnovationPercentage = (double)innovationPercentage / 100.0,
                GuidelinePercentage = (double)guidelinePercentage / 100.0,
                LeverAccessibility = (double)this.LeverAccessibility / (double)total,
                LeverAmountHordes = (double)this.LeverAmountHordes / (double)total,
                LeverDanger = (double)this.LeverDanger / (double)total,
                LeverMaxItem = (double)this.LeverMaxItem / (double)total,
                LeverMaxMonster = (double)this.LeverMaxMonster / (double)total

            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                convergenceAlgorithm.Run();
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                innovationAlgorithm.Run();
            }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                guidelineAlgorithm.Run();
            }));
        }

        void AlgorithmRunCompleteCallback()
        {
            if (innovationAlgorithm.HasSolution && convergenceAlgorithm.HasSolution && guidelineAlgorithm.HasSolution)
            {
                //var conv = convergenceAlgorithm.Solution.GetTop(1)[0];
                //var inno = innovationAlgorithm.Solution.GetTop(1)[0];
                //var obj = guidelineAlgorithm.Solution.GetTop(1)[0];

                //Logger.AppendText("Innovation: " + inno.Fitness);
                //Logger.AppendText("Convergence: " + conv.Fitness);
                //Logger.AppendText("Objective: " + obj.Fitness);

                //suggestionsMap.Add(ChromosomeUtils.MapFromChromosome(OriginalMap, inno));
                //suggestionsMap.Add(ChromosomeUtils.MapFromChromosome(OriginalMap, conv));
                //suggestionsMap.Add(ChromosomeUtils.MapFromChromosome(OriginalMap, obj));

                AlgorithmRunComplete callback = new AlgorithmRunComplete(MixRunCompleteCallback);

                ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
                {
                    mixAlgorithm.Run(monsters.AreasManager, convergenceAlgorithm.Solution, innovationAlgorithm.Solution, guidelineAlgorithm.Solution);
                }));
            }
        }

        void MixRunCompleteCallback()
        {
            Chromosome c = mixAlgorithm.Solution.GetTop(1)[0];
            Logger.AppendText("Fitness: " + c.Fitness);

            /*if (monsters.USelection.HasSelection)
            {
                AddSuggestion(ChromosomeUtils.MapFromChromosome(OriginalMap, c, monsters.USelection.GetSelectedPoints()));
            }
            else AddSuggestion(ChromosomeUtils.MapFromChromosome(OriginalMap, c));*/

            AddSuggestion(ChromosomeUtils.MapFromChromosome(OriginalMap, c));

            algorithmRunning = false;

            if (suggestionOnTime)
            {
                monsters.NotifyUser();
                suggestionOnTime = false;
            }
            
            timer.Start();
        }

        public void ResetTimer()
        {
            if (timer.Enabled)
            {
                if (!algorithmRunning)
                {
                    timer.Stop();
                    timer.Start();
                }
            }
        }

        internal void ReloadLOG()
        {
            hook.ReloadLOG();
            LoadMapFromFile();
        }

        internal void ShowPovoater()
        {
            hook.ShowPovoater();
        }
    }
}
