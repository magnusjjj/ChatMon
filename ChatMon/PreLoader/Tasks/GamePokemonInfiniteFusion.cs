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
                    SendProgress("Cloning images! This downloads a massive amount of files! Do not turn off ChatMon or you will have to delete files manually!\n" + message, 50, 50);                
                }
                return true;
            };

            Repository.Clone("https://github.com/magnusjjj/ChatMonFusion", "./html/game/infinitefusion/", co);

            SendProgress("Completely done!", 100, 100);
            SendMessage(new
            {
                type = "SetupDone"
            });
            return Task.CompletedTask;
        }
    }
}
