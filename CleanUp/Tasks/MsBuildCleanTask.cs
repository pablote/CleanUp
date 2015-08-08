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
    class MsBuildCleanTask : CleanUpTask
    {
        private const string msBuildPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe";
		private const string xbuildPath = "xbuild";
		private readonly ILog logger;
        private string configuration;

        public MsBuildCleanTask(string configuration)
        {
            logger = LogManager.GetLogger(typeof(MsBuildCleanTask));
            this.configuration = configuration;
        }

        public override bool CanProcess(string path)
        {
            return !IsDirectory(path) && Path.GetExtension(path) == ".sln";
        }

        public override void Process(string path)
        {
            logger.Info(string.Format("Found solution file. Cleaning up: {0}", path));

			string executableTool = isMono() ? xbuildPath : msBuildPath;

			ProcessStartInfo psInfo = new ProcessStartInfo(executableTool, string.Format("{0} /t:Clean /p:Configuration={1}", path, this.configuration));
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

            logger.Info("Solution cleaned successfully");
        }

		private bool isMono()
		{
			return Type.GetType("Mono.Runtime") != null ;
		}
    }
}
