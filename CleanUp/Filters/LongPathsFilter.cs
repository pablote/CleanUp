using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Filters
{
    /// <summary>
    /// .Net fails if filepaths contain more than 260 characters, so just filter those.
    /// </summary>
    class LongPathsFilter : CleanUpFilter
    {
        public override bool ShouldFilter(string path)
        {
            return path.Length > 260;
        }
    }
}
