using GalacticConquest.Core;
using GalacticConquest.Core.WebRenderer;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Threading;
using Xilium.CefGlue;

namespace GalacticConquest.Module
{
	public class ModuleCEF
	{
		private CefBrowser browser;

		//CEF display buffer
		private IntPtr buffer = IntPtr.Zero;
		//SharpDX texture
		private Texture texture = null;
		private Mutex mutex = new Mutex();

		public ModuleCEF()
		{
			CefRuntime.Load();

			CefMainArgs cefMainArgs = new CefMainArgs(new string[0]);
			WebRendererApp cefApp = new WebRendererApp();
			CefRuntime.ExecuteProcess(cefMainArgs, cefApp, IntPtr.Zero);

			CefSettings cefSettings = new CefSettings();
			cefSettings.MultiThreadedMessageLoop = true;
			cefSettings.SingleProcess = true;
			cefSettings.LogSeverity = CefLogSeverity.Info;
            cefSettings.LogFile = "cef.log";
			cefSettings.RemoteDebuggingPort = 20480;

			CefRuntime.Initialize(cefMainArgs, cefSettings, cefApp, IntPtr.Zero);
			CefWindowInfo cefWindowInfo = CefWindowInfo.Create();
			cefWindowInfo.SetAsWindowless(Process.GetCurrentProcess().MainWindowHandle, false);

			CefBrowserSettings cefBrowserSettings = new CefBrowserSettings();

			WebRendererClient cefClient = new WebRendererClient(1920, 1080, this);

			CefBrowserHost.CreateBrowser(cefWindowInfo, cefClient, cefBrowserSettings, "http://shadowfita.github.io/galactic-conquest");
		}

		internal void OnBrowserCreated(CefBrowser browser)
		{
			this.browser = browser;
			browser.GetHost().CloseBrowser();
        }

		public void SetBuffer(IntPtr buff)
		{
			buffer = buff;
		}

		public Texture GetTexture(Device device)
		{
			if (mutex.WaitOne())
			{
				if (texture == null)
					texture = new Texture(device, 1920, 1080, 1, Usage.Dynamic, Format.A8R8G8B8, Pool.Default);
				if (buffer != IntPtr.Zero)
				{
					DataRectangle rect = texture.LockRectangle(0, LockFlags.Discard);
					MemoryUtils.memcpy(rect.DataPointer, buffer, new UIntPtr(1920 * 1080 * 4));
					texture.UnlockRectangle(0);
				}
				mutex.ReleaseMutex();
			};

			return texture;
		}

		public bool ShuttingDown { get; private set; } = false;

		public void Shutdown()
		{
			CefRuntime.Shutdown();
			ShuttingDown = false;
		}

		public void WaitForShutdown()
		{
			ShuttingDown = true;
			//Wait until the main thread kills CEF
			while (ShuttingDown) { }
		}
	}
}
