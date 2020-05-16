using COSMOS.Equipment;
using UnityEngine;

namespace COSMOS.SpaceShip
{
    public class OutsideInventorySlot : InventorySlot
    {
        public Vector3 Position { get; protected set; }
        public Vector3 Rotation { get; protected set; }
        public float DefaultRotation { get; protected set; }
        public float RotationRangeMin { get; protected set; }
        public float RotationRangeMax { get; protected set; }

        public OutsideInventorySlot(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    public class SideEngineSlot : OutsideInventorySlot
    {
        public enum Side
        {
            Left,
            Right
        }
        public Side SlotSide { get; protected set; }

        public SideEngineSlot(Vector3 position, Vector3 rotation) : base(position, rotation)
        {

        }
    }
}
