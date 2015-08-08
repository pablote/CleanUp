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
    class Program
    {
        static void Main(string[] args)
        {
            // set up logger
            log4net.Config.XmlConfigurator.Configure();
            var logger = LogManager.GetLogger(typeof(Program));

            // set up command line
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                string basePath = options.BasePath ?? Environment.CurrentDirectory;

                logger.Info(string.Format("Starting Cleanup at {0}", basePath));

                var cleanup = new CleanUpController();
                //cleanup.RegisterTask(new OutputToConsoleTask());
                cleanup.RegisterTask(new GitGcTask());
                cleanup.RegisterTask(new MsBuildCleanTask("Debug"));
                cleanup.RegisterTask(new MsBuildCleanTask("Release"));
				cleanup.RegisterTask(new NpmPackageTask());
				cleanup.RegisterTask(new MavenCleanTask());
                cleanup.RegisterFilter(new LongPathsFilter());
                cleanup.RegisterFilter(new GitRepoFilesFilter());
                cleanup.RegisterFilter(new HgRepoFilesFilter());
				cleanup.RegisterFilter(new NodeModulesFilter());
                cleanup.Work(basePath);
            }
        }
    }
}
