using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using Serilog;

namespace RimworldModUpdater
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        public virtual bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl,
            string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
            IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            Log.Debug("Redirecting popup '{0}' to browser window.", targetUrl);
            chromiumWebBrowser.Load(targetUrl);

            newBrowser = null;
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { return false; }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
    }
}
