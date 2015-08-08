using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Filters
{
    abstract class CleanUpFilter
    {
        public abstract bool ShouldFilter(string path);

        protected bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
