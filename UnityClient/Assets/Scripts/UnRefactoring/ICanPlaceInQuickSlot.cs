using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Player
{
    public interface ICanPlaceInQuickSlot
    {
        string LKeyName { get; }
        string IconID { get; }

    }
}
