using GalacticConquest.Module;
using Xilium.CefGlue;

namespace GalacticConquest.Core.WebRenderer
{
	public class WebRendererClient : CefClient
	{
		private WebRendererHandler _renderHandler;
		private WebRendererLifeSpanHandler _lifeSpanHandler;


		public WebRendererClient(int windowWidth, int windowHeight, ModuleCEF cefModule)
		{
			_renderHandler = new WebRendererHandler(windowWidth, windowHeight, cefModule);
			_lifeSpanHandler = new WebRendererLifeSpanHandler(cefModule);
        }

		protected override CefLifeSpanHandler GetLifeSpanHandler()
		{
			return _lifeSpanHandler;
		}

		protected override CefRenderHandler GetRenderHandler()
		{
			return _renderHandler;
		}
	}
}
