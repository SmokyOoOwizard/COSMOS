using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS
{
    public static class GameData
    {
        public static bool WorldPause { get; private set; }
        public static DateTime CurrentDate { get; private set; }
        public static PlayerData PlayerData { get; private set; }
    }
}
