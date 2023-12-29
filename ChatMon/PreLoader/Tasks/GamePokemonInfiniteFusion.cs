using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace ChatMon.PreLoader.Tasks
{
    internal class GamePokemonInfiniteFusion : PreLoaderTaskBase
    {
        public override Task Do()
        {
            var co = new CloneOptions();
            co.RecurseSubmodules = true;

            co.OnProgress = (string message) =>
            {
                if(message.Trim().Length > 0) {
                    SendProgress("Fetching the pokedex!\n" + message, 50, 50);                
                }
                return true;
            };

            if (!File.Exists(StaticSettings.DOWNLOAD_DIRECTORY + "ChatMonFusion_clone_done"))
            {
                try { Directory.Delete(StaticSettings.DOWNLOAD_DIRECTORY + "infinitefusion/", true); } catch { };
                Repository.Clone("https://github.com/magnusjjj/ChatMonFusion", StaticSettings.DOWNLOAD_DIRECTORY + "infinitefusion/", co);
                File.Create(StaticSettings.DOWNLOAD_DIRECTORY + "ChatMonFusion_clone_done");
            }

            SendProgress("Completely done!", 100, 100);
            SendMessage(new
            {
                type = "SetupDone"
            });
            return Task.CompletedTask;
        }
    }
}