using GalacticConquest.Module;
using Xilium.CefGlue;

namespace GalacticConquest.Core.WebRenderer
{
	public class WebRendererLifeSpanHandler : CefLifeSpanHandler
	{
		private ModuleCEF cefModule;

		public WebRendererLifeSpanHandler(ModuleCEF cefModule)
		{
			this.cefModule = cefModule;
		}

		protected override void OnAfterCreated(CefBrowser browser)
		{
			base.OnAfterCreated(browser);
			cefModule.OnBrowserCreated(browser);
		}
	}
}
