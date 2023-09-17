using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatMon.PreLoader.Tasks
{
    internal class Download : PreLoaderTaskBase
    {
        string url;
        string tempname;
        string target;
        double stagepercent;

        public Download(string url, string tempname, string target, double stagepercent)
        {
            this.url = url;
            this.tempname = tempname;
            this.target = target;
            this.stagepercent = stagepercent;
        }

        public override async Task Do()
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
                                    double percent = (double)totalBytesRead / contentLength.Value * 100;

                                    SendProgress("Downloading " + (int)(contentLength / 1000) + "kb, read " + (int)(totalBytesRead / 1000) + "kb ", stagepercent, percent);
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
                catch (InvalidDataException e)
                {
                    MessageBox.Show("The zip file downloaded was malformed. This usually means that the download was aborted. Will retry.");
                    File.Delete(tempname);
                    await Do();
                }

            }
        }
    }
}
