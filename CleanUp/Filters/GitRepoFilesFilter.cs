using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Filters
{
    /// <summary>
    /// Filter all files within git repos except the repo directory itself.
    /// </summary>
    class GitRepoFilesFilter : CleanUpFilter
    {
        public override bool ShouldFilter(string path)
        {
            return path.Contains(".git") && !(IsDirectory(path) && Path.GetFileName(path) == ".git");        
        }
    }
}
