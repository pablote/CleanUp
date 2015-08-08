using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanUp.Filters;
using CleanUp.Tasks;
using log4net;

namespace CleanUp
{
    class CleanUpController
    {
        private ILog logger;
        private FileSystemWalker fsWalker;
        private List<CleanUpTask> tasks;
        private List<CleanUpFilter> filters;

        public CleanUpController()
        {
            logger = LogManager.GetLogger(typeof(CleanUpController));
            this.fsWalker = new FileSystemWalker();
            this.fsWalker.OnPath += fsWalker_OnPath;
            this.tasks = new List<CleanUpTask>();
            this.filters = new List<CleanUpFilter>();
        }

        public void RegisterTask(CleanUpTask task)
        {
            this.tasks.Add(task);
        }

        public void RegisterFilter(CleanUpFilter filter)
        {
            this.filters.Add(filter);
        }

        public void Work(string basePath)
        {
            this.fsWalker.Run(basePath);
        }

        private void fsWalker_OnPath(object sender, string path)
        {
            if (filters.Any(f => f.ShouldFilter(path)))
                return;

            foreach (var task in this.tasks)
            {
                try
                {
                    if (task.CanProcess(path))
                        task.Process(path);
                }
                catch (Exception ex)
                {
                    logger.Error("Error while executing task.", ex);
                }
            }
        }
    }
}
