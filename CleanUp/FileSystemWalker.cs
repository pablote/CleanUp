using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;

namespace CleanUp
{
    class FileSystemWalker
    {
        private ILog logger;

        public event EventHandler<string> OnPath;

        public FileSystemWalker()
        {
            logger = LogManager.GetLogger(typeof(FileSystemWalker));
        }

        public void Run(string basePath)
        {
            Stack<string> paths = new Stack<string>();
            paths.Push(basePath);

            while (paths.Count > 0)
            {
                string currentPath = paths.Pop();

                try
                {
                    if (IsDirectory(currentPath))
                    {
                        foreach (string filename in Directory.GetFiles(currentPath))
                            paths.Push(filename);

                        foreach (string subdir in Directory.GetDirectories(currentPath))
                            paths.Push(subdir);
                    }

                    RaiseOnPath(currentPath);
                }
                catch (Exception ex)
                {
                    logger.Warn(string.Format("Unable to process path: {0}. Error: {1}", currentPath, ex.Message));
                }
            }
        }

        private void RaiseOnPath(string path)
        {
            if (OnPath != null)
                OnPath(this, path);
        }

        private bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
