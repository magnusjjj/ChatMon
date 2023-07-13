using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatMon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            
        }

        private async Task Setup()
        {
            if (!Directory.Exists("html/game/pokemon/pokemon.json-master")) {
                if (!File.Exists("master.zip")) { 
                    MessageBox.Show("Because this is the first time you run the tool, we need to download and extract some files. This will take a little bit.");
                    HttpClient client = new HttpClient();
                    using (var s = await client.GetStreamAsync("https://github.com/fanzeyi/pokemon.json/archive/refs/heads/master.zip"))
                    {
                        using (var fs = new FileStream("master.zip", FileMode.CreateNew))
                        {
                            await s.CopyToAsync(fs);
                        }
                    }
                }
                ZipFile.ExtractToDirectory("master.zip", "html/game/pokemon/");
            }
            //await c.DownloadFileTaskAsync(new Uri("https://github.com/fanzeyi/pokemon.json/archive/refs/heads/master.zip"), "pokemonjson.zip");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Setup();
            CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
            Options.AdditionalBrowserArguments = "--enable-features=msWebView2EnableDraggableRegions";
            CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(null, null, Options);
            await webView.EnsureCoreWebView2Async(env);

            webView.DefaultBackgroundColor = System.Drawing.Color.Transparent;

            webView.CoreWebView2.WindowCloseRequested += (sender, e) => { this.Close(); };

 // We need to wait until the web window has actually loaded
#if DEBUG
            // webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}"); // Unless we use a javascript build script stage here, it caches. Took a while to figure out.
            webView.CoreWebView2.OpenDevToolsWindow();
            webView.Source = new Uri("http://localhost:8080/");
#else
                    webView.CoreWebView2.SetVirtualHostNameToFolderMapping("nuzlocke.example",
                    Environment.GetEnvironmentVariable("CHATMON_DEBUG") == "TRUE" ? System.IO.Path.GetFullPath("..\\..\\..\\html\\") : System.IO.Path.GetFullPath(".\\html\\"),
                    CoreWebView2HostResourceAccessKind.Allow); // The .example domain name here is INCREDIBLY important. Otherwise there is a delay of 4 seconds while it tries to name resolve.
                    // Also, if the CHATMON_DEBUG is active, we point to the actual source directory, because otherwise visual studio won't send the changed files :(
                    await webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}"); // Unless we use a javascript build script stage here, it caches. Took a while to figure out.
                    webView.Source = new Uri("https://nuzlocke.example/index.html");
#endif
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived; // If the code wants to poke us for whatever reason. Not used yet.

        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {

            string themessage = e.WebMessageAsJson;
            MessageFromChatMon mfjs = JsonSerializer.Deserialize<MessageFromChatMon>(themessage);
            /*if (mfjs.type == "ChatMonDone")
            {
                webView.CoreWebView2.PostWebMessageAsJson("{\"type\":\"GetSettings\"}");
            }*/

            if(mfjs.type == "ChatMonSettings")
            {
                ChatMonSettings settings = JsonSerializer.Deserialize<ChatMonSettings>(themessage);
                GlobalKeybinder.OnShutUp -= GlobalKeybinder_OnShutUp;
                GlobalKeybinder.OnShutUp += GlobalKeybinder_OnShutUp;
                GlobalKeybinder.Register(this, (settings.settings.key_alt ? 0x1 : 0) | (settings.settings.key_ctrl ? 0x2 : 0), settings.settings.key);
            }
        }

        private void GlobalKeybinder_OnShutUp()
        {
            webView.CoreWebView2.PostWebMessageAsJson("{\"type\":\"ShutUp\"}");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
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
        }
        class ChatMonSettings
        {
            public string type { get; set; }
            public InnerChatMonSettings settings { get; set; }
        }
    }
}
