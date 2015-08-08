using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Tasks
{
    class OutputToConsoleTask : CleanUpTask
    {
        public override bool CanProcess(string path)
        {
            return true;
        }

        public override void Process(string path)
        {
            Console.WriteLine(path);
        }
    }
}
