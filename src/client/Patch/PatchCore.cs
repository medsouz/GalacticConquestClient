using GalacticConquest.Core;
using System;
using System.Runtime.InteropServices;

namespace GalacticConquest.Patch
{
	public class PatchCore
	{
		Hook<ExitProcessDelegate> ExitGameHook = null;
		Hook<WndProcDelegate> WndProcHook = null;

		public PatchCore()
		{
			//This function doesn't actually close the game but it runs right before the window is destroyed. It'll suit our needs.
			ExitGameHook = new Hook<ExitProcessDelegate>(new IntPtr(0x5DDC20), new ExitProcessDelegate(ExitGame), this);
			WndProcHook = new Hook<WndProcDelegate>(new IntPtr(0x621C60), new WndProcDelegate(WndProc), this);
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

		//LRESULT __stdcall WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		delegate int WndProcDelegate(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

		//http://www.pinvoke.net/default.aspx/Constants/WM.html
		int WndProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam)
		{
			if (uMsg == 0x20) //WM_SETCURSOR
				return 0;

			return WndProcHook.original(hwnd, uMsg, wParam, lParam);
		}
	}
}
