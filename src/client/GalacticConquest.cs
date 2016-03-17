using EasyHook;
using GalacticConquest.Module;
using GalacticConquest.Patch;
using System;
using System.Windows.Forms;

namespace GalacticConquest
{
    public class GalacticConquest : IEntryPoint
	{
		// Modules
		public static ModuleCEF ModuleCEF;

		// Patches
		public static PatchRenderer PatchRenderer;
		public static PatchCore PatchCore;

		public GalacticConquest(RemoteHooking.IContext context, String channelName)
		{
		}

		public void Run(RemoteHooking.IContext InContext, String channelName)
		{
			try {
				//Load modules
				ModuleCEF = new ModuleCEF();
				ModuleCEF.OpenPage("http://shadowfita.github.io/galactic-conquest");
				//Apply hooks
				PatchRenderer = new PatchRenderer();
				PatchCore = new PatchCore();
				//Keep this thread running
				while (true)
				{
					//Kill CEF if requested
					if (ModuleCEF.ShuttingDown)
						ModuleCEF.Shutdown();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "Galactic Conquest Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
    }
}
