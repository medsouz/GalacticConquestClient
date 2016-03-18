using GalacticConquest.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GalacticConquest.Patch
{
	public class PatchCore
	{
		Hook<ExitProcessDelegate> ExitGameHook = null;

		public PatchCore()
		{
			//This function doesn't actually close the game but it runs right before the window is destroyed. It'll suit our needs.
			ExitGameHook = new Hook<ExitProcessDelegate>(new IntPtr(0x5DDC30), new ExitProcessDelegate(ExitGame), this);
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		delegate char ExitProcessDelegate();

		char ExitGame()
		{
			//Close CEF before the game closes. This prevents a background process from running after the game has closed.
			GalacticConquest.WaitForShutdown();
			// Call the original function
			return ExitGameHook.original();
		}
	}
}
