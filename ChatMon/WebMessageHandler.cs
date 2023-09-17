using ChatMon.PreLoader.Tasks;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatMon
{
    internal class WebMessageHandler
    {
        static ChatMonSettings settings;
        public static MainWindow MainWindow;

        public static async void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string themessage = e.WebMessageAsJson;
            MessageFromChatMon mfjs = JsonSerializer.Deserialize<MessageFromChatMon>(themessage);

            if (mfjs.type == "StartPreloader")
            {
                switch (settings.settings.gametype)
                {
                    case "default":
                        await new GameDefaultSetup().Do();
                        break;
                    case "infinitefusion":
                        await new GamePokemonInfiniteFusion().Do();
                        break;
                    default: throw new Exception("No game like" + settings.settings.gametype);
                }
                
            }

            if (mfjs.type == "ChatMonSettings")
            {
                settings = JsonSerializer.Deserialize<ChatMonSettings>(themessage);
                
                GlobalKeybinder.OnShutUp -= GlobalKeybinder_OnShutUp;
                GlobalKeybinder.OnShutUp += GlobalKeybinder_OnShutUp;
                GlobalKeybinder.Register(MainWindow, (settings.settings.key_alt ? 0x1 : 0) | (settings.settings.key_ctrl ? 0x2 : 0), settings.settings.key);
            }
        }

        public static void GlobalKeybinder_OnShutUp()
        {
            MainWindow.Dispatcher.BeginInvoke(() => {
                MainWindow.webView.CoreWebView2.PostWebMessageAsJson("{\"type\":\"ShutUp\"}");
            });
        }

        class MessageFromChatMon
        {
            public string type { get; set; }
            public string content { get; set; }
        }

        class InnerChatMonSettings
        {
            public bool key_ctrl { get; set; } = false;
            public bool key_alt { get; set; } = false;
            public int key { get; set; } = 0;
            public string gametype { get; set; }
        }
        class ChatMonSettings
        {
            public string type { get; set; }
            public InnerChatMonSettings settings { get; set; }
        }
    }
}
