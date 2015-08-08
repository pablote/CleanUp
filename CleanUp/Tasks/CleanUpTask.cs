using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Tasks
{
    abstract class CleanUpTask
    {
        public abstract bool CanProcess(string path);

        public abstract void Process(string path);

        protected bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
