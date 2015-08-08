using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Filters
{
    /// <summary>
    /// Filter all files within mercurial repos except the repo directory itself.
    /// </summary>
    class HgRepoFilesFilter : CleanUpFilter
    {
        public override bool ShouldFilter(string path)
        {
            return path.Contains(".hg") && !(IsDirectory(path) && Path.GetFileName(path) == ".hg");        
        }
    }
}
