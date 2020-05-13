namespace COSMOS.Equipment
{
    public abstract class Weapon : OutsideEquipment
    {
        public int DamageType;

        public abstract void Fire(float delta);
    }
    public abstract class Weapon<T> : Weapon where T : OutsideWeaponObjectController
    {
        public new virtual T GameObjectController
        {
            get { return base.GameObjectController as T; }
            protected set { base.GameObjectController = value as T; }
        }
    }
}
