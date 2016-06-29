using Chromium;
using EasyHook;
using GalacticConquest.Core;
using GalacticConquest.Module;
using GalacticConquest.Patch;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace GalacticConquest
{
    public class GalacticConquest : IEntryPoint
	{
		public static bool ShuttingDown { get; private set; } = false;

		// Modules
		public static ModuleCEF ModuleCEF;

		// Patches
		public static PatchRenderer PatchRenderer;
		public static PatchCore PatchCore;

		public GalacticConquest(RemoteHooking.IContext context)
		{
			SHA1CryptoServiceProvider cryptoProvider = new SHA1CryptoServiceProvider();
			FileStream exeStream = new FileStream(Process.GetCurrentProcess().MainModule.FileName, FileMode.Open, FileAccess.Read);
			string sha1sum = BitConverter.ToString(cryptoProvider.ComputeHash(exeStream)).Replace("-", "").ToLower();
			exeStream.Close();

			SWBF2Version version = SWBF2Version.getVersion(sha1sum);
			MessageBox.Show(version.name + " | " + sha1sum, "SWBF2 Version");
		}

		public void Run(RemoteHooking.IContext InContext)
		{
			try {
				//Load modules
				ModuleCEF = new ModuleCEF();
				//ModuleCEF.OpenPage("http://shadowfita.github.io/galactic-conquest");
				//Apply hooks
				PatchRenderer = new PatchRenderer();
				PatchCore = new PatchCore();
				//Start the game
				RemoteHooking.WakeUpProcess();
				//Keep this thread running
				while (!ShuttingDown)
				{
					CfxRuntime.DoMessageLoopWork();
				}
				//Shutdown CEF
				ModuleCEF.Shutdown();
				//Shutdown is finished
				ShuttingDown = false;
            }
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "Galactic Conquest Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void WaitForShutdown()
		{
			ShuttingDown = true;
            while (ShuttingDown){ }
		}
    }
}
