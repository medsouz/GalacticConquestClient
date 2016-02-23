using GalacticConquest.Module;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace GalacticConquest.Core.WebRenderer
{
	public class WebRendererHandler : CefRenderHandler
	{
		private readonly int _windowHeight;
		private readonly int _windowWidth;

		private ModuleCEF cefModule;
		private Mutex mutex = new Mutex();

		public WebRendererHandler(int windowWidth, int windowHeight, ModuleCEF cefModule)
		{
			_windowWidth = windowWidth;
			_windowHeight = windowHeight;
			this.cefModule = cefModule;
		}

		protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
		{
			return GetViewRect(browser, ref rect);
		}

		protected override bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
		{
			screenX = viewX;
			screenY = viewY;
			return true;
		}

		protected override bool GetViewRect(CefBrowser browser, ref CefRectangle rect)
		{
			rect.X = 0;
			rect.Y = 0;
			rect.Width = _windowWidth;
			rect.Height = _windowHeight;
			return true;
		}

		protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
		{
			return false;
		}

		protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
		{
		}

		protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, IntPtr buffer, int width, int height)
		{
			cefModule.SetBuffer(buffer);
		}

		protected override void OnScrollOffsetChanged(CefBrowser browser)
		{
		}

		protected override void OnCursorChange(CefBrowser browser, IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
		{
		}
	}
}
