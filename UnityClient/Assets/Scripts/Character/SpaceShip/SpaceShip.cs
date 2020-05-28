using COSMOS.Equipment;
using COSMOS.SpaceShip.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace COSMOS.SpaceShip
{
    public partial class SpaceShip : Character.Character
    {
        public float HullWeight { get; protected set; }
        public float TotalWeight { get; protected set; }
        public float MaxWeight { get; protected set; }

        public float FreeVolume { get; protected set; }
        public float MaxVolume { get; protected set; }

        public float MaxSpeed { get; protected set; } = 20;

        public SpaceShip()
        {
            InitEquipments();
        }
    }
}
