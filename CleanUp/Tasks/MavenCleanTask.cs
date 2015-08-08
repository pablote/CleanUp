using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CleanUp.Tasks
{
    class MavenCleanTask : CleanUpTask
    {
		private readonly ILog logger;

		public MavenCleanTask()
        {
			logger = LogManager.GetLogger(typeof(MavenCleanTask));
        }

        public override bool CanProcess(string path)
        {
			return !IsDirectory(path) && Path.GetFileName(path) == "pom.xml";
        }

        public override void Process(string path)
        {
            logger.Info(string.Format("Found maven pom file. Cleaning up: {0}", path));

			ProcessStartInfo psInfo = new ProcessStartInfo("mvn", "clean");
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.WindowStyle = ProcessWindowStyle.Normal;
            psInfo.UseShellExecute = false;
            psInfo.WorkingDirectory = Path.GetDirectoryName(path);

            Process ps = System.Diagnostics.Process.Start(psInfo);
            ps.WaitForExit(60 * 1000);

            using (System.IO.StreamReader myOutput = ps.StandardOutput)
            {
                string output = myOutput.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(output))
                    logger.Info(output);
            }

            using (System.IO.StreamReader myError = ps.StandardError)
            {
                string error = myError.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(error))
                    logger.Error(error);
            }

            logger.Info("Maven project cleaned successfully");
        }
    }
}
