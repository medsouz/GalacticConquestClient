using Chromium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEFProcess
{
	class Program
	{
		static void Main(string[] args)
		{
			int exitCode = CfxRuntime.ExecuteProcess();
			if (exitCode >= 0)
			{
				Environment.Exit(exitCode);
			}
		}
	}
}
