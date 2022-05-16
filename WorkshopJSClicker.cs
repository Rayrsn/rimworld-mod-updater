using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace RimworldModUpdater
{
    public class WorkshopJSClicker
    {
        public delegate void WorkshopBtnClick(string id = null);

        public event WorkshopBtnClick Clicked;

        public void doDownload()
        {
            Log.Debug("_____updater.doDownload()");
            Clicked?.Invoke();
        }

        public void doDownloadId(string id)
        {
            Log.Debug("_____updater.doDownloadId({0})", id);
            Clicked?.Invoke(id);
        }
    }
}
