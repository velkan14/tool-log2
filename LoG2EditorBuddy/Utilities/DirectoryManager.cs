using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Utilities
{
    static class DirectoryManager
    {

        private static string _dungeonFilePath;
        private static string _scriptsDir;
        private static string _mutationDir;
        private static string _projDir;
        private static string _processDir;
        private static string _gogGamesDir;
        private static string _log2GameDir;
        private static string _testSaveDir;
        private static string _popSaveDir;

        public static string ScriptsDir { get { return _scriptsDir; } }
        public static string MutationsDir { get { return _mutationDir; } }
        public static string DungeonFilePath { get { return _dungeonFilePath; } }
        public static string GogGamesDir { get { return _gogGamesDir; } }
        public static string LoG2GameDir { get { return _log2GameDir; } }
        public static string TestSaveDir { get { return _testSaveDir; } }
        public static string PopulationSaveDir { get { return _popSaveDir; } }

        /// <summary>
        /// Get: base LoG2 Project directory
        /// Set: current project dir structure
        /// </summary>
        public static string ProjectDir
        {
            get { return _projDir; }
            set
            {
                _projDir = value;
                _mutationDir = _projDir + @"\mutations";
                _popSaveDir = _projDir + @"\population";
                if (!Directory.Exists(_mutationDir))
                    Directory.CreateDirectory(_mutationDir);
                _scriptsDir = _projDir + @"\mod_assets\scripts";
                _dungeonFilePath = _scriptsDir + @"\dungeon.lua";
                _testSaveDir = _projDir + @"\testSave.lua";
            }
        }

        /// <summary>
        /// Full path to the .exe retrieved from the running process
        /// </summary>
        public static string ProcessDir
        {
            get { return _processDir; }
            set
            {
                _processDir = value;
                //tratar fullpath
                var tmpS = _processDir;
                tmpS = tmpS.Remove(tmpS.LastIndexOf('\\'));

                _log2GameDir = tmpS;

                tmpS = tmpS.Remove(tmpS.LastIndexOf('\\'));

                _gogGamesDir = tmpS;
                //Logger.AppendText(_gogGamesDir);
            }
        }


        public static void StoreLastProjDir()
        {
            //JsonConvert.SerializeObject(ProjectDir);
            File.WriteAllText("PersistentData.pd", JsonConvert.SerializeObject(ProjectDir));
            
        }

        public static void RetrieveLastProjectDir()
        {
            string s = File.ReadAllText("PersistentData.pd");
            var lastDir = JsonConvert.DeserializeObject<string>(s);
            if (Directory.Exists(lastDir))
                ProjectDir = lastDir;
            else ProjectDir = null;
        }


    }
}
