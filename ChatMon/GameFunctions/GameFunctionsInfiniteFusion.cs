using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[assembly: ComVisible(true)]
namespace ChatMon.GameFunctions
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class GameFunctionsInfiniteFusion
    {
        public async Task<string> ResolveImage(string pokemondata)
        {
            if (pokemondata == null || pokemondata == "") return ""; // Sanity check for faulty download attempt

            if(Regex.IsMatch(pokemondata, @"^\d+$")) // Regular, non-fusion sprite, just a straight up pokemon number
            {
                string returner = StaticSettings.DOWNLOAD_URL_BASE + "pokemonfusion_images/" + pokemondata + ".png";
                string tosave = StaticSettings.DOWNLOAD_DIRECTORY + "pokemonfusion_images/" + pokemondata + ".png";
                string targeturl = await TryDownload(tosave, "https://gitlab.com/pokemoninfinitefusion/autogen-fusion-sprites/-/raw/master/Battlers/" + pokemondata + "/" + pokemondata + "." + pokemondata + ".png");

                if (targeturl != "")
                    return targeturl;
                else
                    return "Network error";
            } else {
                var data = Regex.Match(pokemondata, "B(\\d*)H(\\d*)");
                var thebase = data.Groups[1].Value;
                var thehead = data.Groups[2].Value;
                string returner = StaticSettings.DOWNLOAD_URL_BASE + "pokemonfusion_images/" + pokemondata + ".png";
                string tosave = StaticSettings.DOWNLOAD_DIRECTORY + "pokemonfusion_images/" + pokemondata + ".png";

                string targeturl = await TryDownload(tosave, "https://gitlab.com/pokemoninfinitefusion/customsprites/-/raw/master/CustomBattlers/" + thehead + "." + thebase + ".png");

                if (targeturl != "") // If it's successful, return
                    return targeturl;

                targeturl = await TryDownload(tosave, "https://gitlab.com/pokemoninfinitefusion/autogen-fusion-sprites/-/raw/master/Battlers/" + thehead + "/" + thehead + "." + thebase + ".png");

                if (targeturl != "")
                    return targeturl;
                else
                    return "Network error";
            }
        }

        private async Task<string> TryDownload(string save_target, string url)
        {
            Directory.CreateDirectory(StaticSettings.DOWNLOAD_DIRECTORY + "pokemonfusion_images");
            string baseurl = StaticSettings.DOWNLOAD_URL_BASE + "pokemonfusion_images/";
            string returner = baseurl + save_target.Replace(StaticSettings.DOWNLOAD_DIRECTORY + "pokemonfusion_images/", "");
            if (!File.Exists(save_target))
            {
                HttpClient client = new HttpClient();
                var dt = await client.GetAsync(url);
                if (dt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var fs = File.Open(save_target, FileMode.Create))
                        await dt.Content.CopyToAsync(fs);
                    return returner;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return returner;
            }
        }
    }
}
