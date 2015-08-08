using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using System.Diagnostics;

namespace CleanUp.Tasks
{
    /// <summary>
    /// Executes "git gc --aggressive" on the working directory
    /// </summary>
    class GitGcTask : CleanUpTask
    {
        private ILog logger;

        public GitGcTask()
        {
            logger = LogManager.GetLogger(typeof(GitGcTask));
        }

        public override bool CanProcess(string path)
        {
            return IsDirectory(path) && Path.GetFileName(path) == ".git";
        }

        public override void Process(string path)
        {
            logger.Info(string.Format("Found git repo. Cleaning up: {0}", path));

            ProcessStartInfo psInfo = new ProcessStartInfo("git", "gc --aggressive");
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.WindowStyle = ProcessWindowStyle.Normal;
            psInfo.UseShellExecute = false;
            psInfo.WorkingDirectory = Path.GetDirectoryName(path);

            Process ps = System.Diagnostics.Process.Start(psInfo);
            ps.WaitForExit(10 * 60 * 1000);

            using (System.IO.StreamReader myOutput = ps.StandardOutput)
            {
                string output = myOutput.ReadToEnd();

                if(!string.IsNullOrWhiteSpace(output))
                    logger.Info(output);
            }

            using (System.IO.StreamReader myError = ps.StandardError)
            {
                string error = myError.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(error))
                    logger.Error(error);
            }

            logger.Info("Repo cleaned successfully");
        }
    }
}
