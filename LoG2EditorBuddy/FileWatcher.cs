using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    class FileWatcher
    {
        private FileSystemWatcher fsw;
        private Core core;

        int count = 0;
        string lastFileText;

        public FileWatcher(Core core)
        {
            this.core = core;

            
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Start()
        {

            lastFileText = System.IO.File.ReadAllText(DirectoryManager.DungeonFilePath);

            try
            {
                fsw = new FileSystemWatcher(DirectoryManager.ScriptsDir, "dungeon.lua");

                /* Watch for changes in LastAccess and LastWrite times, and
                the renaming of files or directories. */
                //fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                //   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                fsw.NotifyFilter = NotifyFilters.LastWrite;

                //add event handlers
                fsw.Changed += new FileSystemEventHandler(FileChanged);

                // Begin watching.
                fsw.EnableRaisingEvents = true;

                Logger.AppendText("Watcher started for: " + DirectoryManager.DungeonFilePath + "\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            count++;
            if (count < 2) return; //needed because watcher fires twice

            string fileText = System.IO.File.ReadAllText(DirectoryManager.DungeonFilePath);

            if (!fileText.Equals(lastFileText))
            {
                lastFileText = fileText;

                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

                core.FileChanged = true;
            }

            count = 0;
        }

    }
}
