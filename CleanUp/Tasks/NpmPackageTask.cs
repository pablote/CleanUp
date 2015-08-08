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
	class NpmPackageTask : CleanUpTask
    {
        private readonly ILog logger;

		public NpmPackageTask()
        {
			logger = LogManager.GetLogger(typeof(NpmPackageTask));
        }

        public override bool CanProcess(string path)
        {
            return !IsDirectory(path) && Path.GetFileName(path) == "package.json";
        }

        public override void Process(string path)
        {
			string workingDirectory = Path.GetDirectoryName(path);
			
            logger.Info(string.Format("Found npm package. Cleaning up: {0}", path));
			logger.Info(string.Format("Using path: {0}", workingDirectory));

			ProcessStartInfo psInfo = new ProcessStartInfo("npm", "prune --production");
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.WindowStyle = ProcessWindowStyle.Normal;
            psInfo.UseShellExecute = false;
			psInfo.WorkingDirectory = workingDirectory;

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

            logger.Info("Npm packacge cleaned successfully");
        }
    }
}
