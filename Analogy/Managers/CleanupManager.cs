using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Analogy.Managers
{
    public class CleanupManager
    {
        private static Lazy<CleanupManager> _instance = new Lazy<CleanupManager>();
        public static CleanupManager Instance => _instance.Value;

        private List<string> FoldersToClean { get; set; }

        public CleanupManager()
        {
            FoldersToClean = new List<string>();
        }

        public void AddFolder(string path)
        {
            if (!FoldersToClean.Contains(path))
            {
                FoldersToClean.Add(path);
            }
        }

        public void Clean()
        {
            foreach (string path in FoldersToClean)
            {

                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path, true);
                    }
                    catch (Exception e)
                    {
                        AnalogyLogger.Instance.LogException($"Error delete folder: {e.Message}",e, "Cleanup");
                    }
                }
            }


        }
    }
}
