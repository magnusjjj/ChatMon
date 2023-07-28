using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
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
using Path = System.IO.Path;

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

        private async Task DownloadWithProgressAndExtract(string url, string tempname, string target, double stagepercent)
        {
            if (!Directory.Exists(target))
            {
                if (!File.Exists(tempname))
                {
                    SendProgress("Starting download of: " + url, stagepercent, 0);
                    HttpClient client = new HttpClient();
                    using (var g = await client.GetAsync(url))
                    {
                        var contentLength = g.Content.Headers.ContentLength;

                        using (var download = await g.Content.ReadAsStreamAsync())
                        {
                            var buffer = new byte[81920];
                            long totalBytesRead = 0;
                            int bytesRead;

                            var fs = new FileStream(tempname, FileMode.CreateNew);

                            while ((bytesRead = await download.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                            {
                                await fs.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                                totalBytesRead += bytesRead;

                                if (!contentLength.HasValue)
                                {
                                    SendProgress("Downloaded " + (int)(totalBytesRead / 1000) + "kb", stagepercent, 10);
                                }
                                else
                                {
                                    double percent = ((double)totalBytesRead / contentLength.Value) * 100;

                                    SendProgress("Downloading " + (int)(contentLength / 1000) + "kb, read " + (int)(totalBytesRead/1000) + "kb ", stagepercent, percent);
                                }
                            }

                            fs.Close();
                        }
                    }
                }

                SendProgress("Extracting zip file " + tempname + "(no progress information available, sorry)", stagepercent, 10);
                try
                {
                    ZipFile.ExtractToDirectory(tempname, target);
                }
                catch(System.IO.InvalidDataException e) {
                    MessageBox.Show("The zip file downloaded was malformed. This usually means that the download was aborted. Will retry.");
                    File.Delete(tempname);
                    await DownloadWithProgressAndExtract(url, tempname, target, stagepercent);
                }
                
            }
        }

        private async Task Setup()
        {
            //SendProgress("Really long text Really long textReally long textReally long textReally long textReally long textReally long textReally long textReally long text", 20, 50);
            await DownloadWithProgressAndExtract("https://github.com/fanzeyi/pokemon.json/archive/refs/heads/master.zip", "pokemonpictures.zip", "html/game/pokemon/", 50);
            SendProgress("Completely done!", 100, 100);
            this.Dispatcher.Invoke(() => {
                webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(new
                {
                    type = "SetupDone"
                }));
            });
        }

        private void SendProgress(string progress, double percent1, double percent2)
        {
            this.Dispatcher.Invoke(() => {
                webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(new
                {
                    type = "ChatMonProgress",
                    message = progress,
                    percent1,
                    percent2
                }));
            });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
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
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (e.Uri.StartsWith("https://twitch.tv/"))
            {
                e.Handled = true;
                Process.Start("explorer", e.Uri);
            }
        }

        private async void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {

            string themessage = e.WebMessageAsJson;
            MessageFromChatMon mfjs = JsonSerializer.Deserialize<MessageFromChatMon>(themessage);
            /*if (mfjs.type == "ChatMonDone")
            {
                webView.CoreWebView2.PostWebMessageAsJson("{\"type\":\"GetSettings\"}");
            }*/

            if(mfjs.type == "StartPreloader")
            {
                await Setup();
            }

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
