using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatMon.PreLoader.Tasks
{
    abstract public class PreLoaderTaskBase
    {
        public static MainWindow MainWindow { get; set; }
        protected void SendProgress(string progress, double percent1, double percent2)
        {
            SendMessage(new
            {
                type = "ChatMonProgress",
                message = progress,
                percent1,
                percent2
            });
        }

        protected void SendMessage(object message)
        {
            MainWindow.Dispatcher.Invoke(() => {
                MainWindow.webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message));
            });
        }

        abstract public Task Do();
    }
}
