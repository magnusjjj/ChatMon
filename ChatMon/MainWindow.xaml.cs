using ChatMon.PreLoader.Tasks;
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
            WebMessageHandler.MainWindow = this;
            PreLoaderTaskBase.MainWindow = this;
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
            Options.AdditionalBrowserArguments = "--enable-features=msWebView2EnableDraggableRegions";
            CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(null, null, Options);
            await webView.EnsureCoreWebView2Async(env);

            webView.DefaultBackgroundColor = System.Drawing.Color.Transparent;

            webView.CoreWebView2.WindowCloseRequested += (sender, e) => { this.Close(); };


            Directory.CreateDirectory(StaticSettings.DOWNLOAD_DIRECTORY); // Expose http://download.example as the download directory
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(StaticSettings.DOWNLOAD_DOMAIN,
                    System.IO.Path.GetFullPath(StaticSettings.DOWNLOAD_DIRECTORY),
                    CoreWebView2HostResourceAccessKind.Allow);

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
                    webView.Source = new Uri("http://nuzlocke.example/index.html");
#endif
            webView.CoreWebView2.WebMessageReceived += WebMessageHandler.WebMessageReceived; // If the code wants to poke us for whatever reason. Not used yet.
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            webView.CoreWebView2.AddHostObjectToScript("infinitefusion", new GameFunctions.GameFunctionsInfiniteFusion());
        }

        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (e.Uri.StartsWith("https://twitch.tv/") || e.Uri.StartsWith("https://www.patreon.com/SlightlyTango") || e.Uri.StartsWith("https://github.com/magnusjjj/ChatMon"))
            {
                e.Handled = true;
                Process.Start("explorer", e.Uri);
            }

        }
    }
}
