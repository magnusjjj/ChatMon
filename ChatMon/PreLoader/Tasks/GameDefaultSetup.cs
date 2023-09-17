using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMon.PreLoader.Tasks
{
    internal class GameDefaultSetup : PreLoaderTaskBase
    {
        public override async Task Do()
        {
            await new Download("https://github.com/fanzeyi/pokemon.json/archive/refs/heads/master.zip", "pokemonpictures.zip", "html/game/pokemon/", 50).Do();
            SendProgress("Completely done!", 100, 100);
            SendMessage(new
                {
                    type = "SetupDone"
                }
            );
        }
    }
}
