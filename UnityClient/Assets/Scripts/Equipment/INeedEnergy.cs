using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Equipment
{
    public interface INeedEnergy
    {
        float EnergyConsumption { get; }
        float PowerPercent { set; }
    }
}
