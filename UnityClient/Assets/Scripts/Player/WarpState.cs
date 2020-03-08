using COSMOS.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Player
{
    public class WarpState
    {
        public SolarSystem From;
        public SolarSystem To;
        public WarpStatus Status;
        public DateTime StartChargeTime;
        public DateTime EndChargeTime;
        public DateTime EndWarpTime;


    }
}
