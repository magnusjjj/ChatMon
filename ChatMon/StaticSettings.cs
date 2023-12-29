using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMon
{
    public static class StaticSettings
    {
        public const string DOWNLOAD_DOMAIN = "downloads.example";
        public const string DOWNLOAD_URL_BASE = "http://" + DOWNLOAD_DOMAIN +"/";
        public const string DOWNLOAD_DIRECTORY = "./Downloads/";
    }
}
