using COSMOS.Equipment;

namespace COSMOS.Equipment
{
    public abstract class OutsideEquipment : Equipment
    {
        public virtual OutsideEquipmentObjectController GameObjectController { get; protected set; }
    }
    public abstract class OutsideEquipment<T> : OutsideEquipment where T : OutsideEquipmentObjectController
    {
        public new virtual T GameObjectController
        {
            get { return base.GameObjectController as T; }
            protected set { base.GameObjectController = value as T; }
        }
    }
}
