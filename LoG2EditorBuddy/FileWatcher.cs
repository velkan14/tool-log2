using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    class FileWatcher
    {
        private FileSystemWatcher fsw;

        public FileWatcher()
        {

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Start()
        {
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

        int count = 0;
        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            count++;
            if (count < 2) return; //needed because watcher fires twice

            //If user decides to apply algorithm solution, press button
            if (APIClass._mapSaved)
            {
                /*SendSaveCommandAndReloadMap(); FIXME*/
            }
            else
            {
                Debug.WriteLine("File save detected");
                /*if (autoSuggestions)
                    ParseMapAndRunAlgorithm(); FIXME*/
            }
            count = 0;
        }
    }
}
