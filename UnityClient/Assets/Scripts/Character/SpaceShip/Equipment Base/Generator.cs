using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Equipment;
using UnityEngine;

namespace COSMOS.SpaceShip.Equipment
{
    public class Generator : COSMOS.Equipment.Equipment
    {
        public string FuelType { get; protected set; }
        public float GeneratePerTick { get; protected set; }
        public float FuelConsumption { get; protected set; }
    }
}
