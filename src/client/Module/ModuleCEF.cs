using Chromium;
using GalacticConquest.Core;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Chromium.Event;

namespace GalacticConquest.Module
{
	public class ModuleCEF
	{
		private CfxBrowser browser;
		private CfxClient client;
		private CfxRenderHandler renderHandler;
		private CfxLifeSpanHandler lifeSpanHandler;
		private CfxLoadHandler loadHandler;

		//CEF display buffer
		private IntPtr buffer = IntPtr.Zero;
		//SharpDX texture
		private Texture texture = null;
		private Mutex mutex = new Mutex();

		public ModuleCEF()
		{
			CfxApp application = new CfxApp();
			application.OnBeforeCommandLineProcessing += app_OnBeforeCommandLineProcessing;

			int exitCode = CfxRuntime.ExecuteProcess(null);
			if (exitCode >= 0)
			{
				Environment.Exit(exitCode);
			}

			CfxSettings settings = new CfxSettings();
			settings.WindowlessRenderingEnabled = true;
			settings.NoSandbox = true;
			settings.BrowserSubprocessPath = "CEFProcess.exe";
			settings.LogFile = "cef.log";
			settings.RemoteDebuggingPort = 20480;
			settings.SingleProcess = false;

			if (!CfxRuntime.Initialize(settings, null))
				MessageBox.Show("Failed to load CEF");
		}

		public void OpenPage(string url)
		{
			if (browser == null)
			{
				renderHandler = new CfxRenderHandler();
				renderHandler.GetViewRect += renderHandler_GetViewRect;
				renderHandler.OnPaint += renderHandler_OnPaint;

				lifeSpanHandler = new CfxLifeSpanHandler();
				lifeSpanHandler.OnAfterCreated += lifeSpanHandler_OnAfterCreated;

				loadHandler = new CfxLoadHandler();

				client = new CfxClient();
				client.GetLifeSpanHandler += (sender, e) => e.SetReturnValue(lifeSpanHandler);
				client.GetRenderHandler += (sender, e) => e.SetReturnValue(renderHandler);
				client.GetLoadHandler += (sender, e) => e.SetReturnValue(loadHandler);

				CfxBrowserSettings settings = new CfxBrowserSettings();
				settings.WindowlessFrameRate = 60;

				CfxWindowInfo windowInfo = new CfxWindowInfo();
				windowInfo.SetAsWindowless(Process.GetCurrentProcess().Handle, false);

				CfxBrowserHost.CreateBrowser(windowInfo, client, url, settings, null);
			}
			else
			{
				browser.FocusedFrame.LoadUrl(url);
			}
		}

		private void app_OnBeforeCommandLineProcessing(object sender, CfxOnBeforeCommandLineProcessingEventArgs e)
		{
			e.CommandLine.AppendSwitch("disable-extensions");
			e.CommandLine.AppendSwitch("disable-gpu");
			e.CommandLine.AppendSwitch("disable-gpu-compositing");
			e.CommandLine.AppendSwitch("enable-begin-frame-scheduling");
		}

		private void renderHandler_GetViewRect(object sender, CfxGetViewRectEventArgs e)
		{
			e.Rect.X = 0;
			e.Rect.Y = 0;
			e.Rect.Width = 1920;
			e.Rect.Height = 1080;
			e.SetReturnValue(true);
		}

		private void renderHandler_OnPaint(object sender, CfxOnPaintEventArgs e)
		{
			buffer = e.Buffer;
		}

		private void lifeSpanHandler_OnAfterCreated(object sender, CfxOnAfterCreatedEventArgs e)
		{
			browser = e.Browser;
		}

		public void SetMousePosition(int x, int y)
		{
			if(browser != null)
			{
				CfxMouseEvent mouseEvent = new CfxMouseEvent();
				mouseEvent.Modifiers = (uint)CfxEventFlags.None;
				mouseEvent.X = x;
				mouseEvent.Y = y;
				browser.Host.SendMouseMoveEvent(mouseEvent, false);
			}
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

		public void Shutdown()
		{
			CfxRuntime.Shutdown();
		}
	}
}
