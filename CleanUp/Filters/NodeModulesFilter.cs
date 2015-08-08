using System;
using CleanUp.Filters;
using System.IO;

namespace CleanUp
{
	/// <summary>
	/// Filters out all files within the node_modules folder
	/// </summary>
	class NodeModulesFilter : CleanUpFilter
	{
		public NodeModulesFilter ()
		{
		}

		public override bool ShouldFilter(string path)
		{
			return path.Contains("node_modules") && !(IsDirectory(path) && Path.GetFileName(path) == "node_modules");        
		}
	}
}

