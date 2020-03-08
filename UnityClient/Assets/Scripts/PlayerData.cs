using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Core.EventDispacher;
namespace COSMOS.Player
{
    public class PlayerData : DataWithEvents
    {
        public long Money { get; protected set; }
        public WarpState CurrentWarp;

    }
}
