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
        private MD5 md5;

        int count = 0;
        byte[] lastHash;

        public FileWatcher(Core core)
        {
            this.core = core;

            
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Start()
        {
            md5 = MD5.Create();

            using (var stream = File.OpenRead(DirectoryManager.DungeonFilePath))
            {
                lastHash = md5.ComputeHash(stream);
            }

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

            byte[] hash;

            using (var stream = File.OpenRead(DirectoryManager.DungeonFilePath))
            {
                hash = md5.ComputeHash(stream);
            }

            if (!Equality(hash, lastHash))
            {
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

                core.LoadMapFromFile();
            }

            count = 0;
        }

        public bool Equality(byte[] a1, byte[] b1)
        {
            int i;
            if (a1.Length == b1.Length)
            {
                i = 0;
                while (i < a1.Length && (a1[i] == b1[i])) //Earlier it was a1[i]!=b1[i]
                {
                    i++;
                }
                if (i == a1.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
