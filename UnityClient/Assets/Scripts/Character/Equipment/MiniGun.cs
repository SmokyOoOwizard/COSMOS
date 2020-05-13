using UnityEngine;

namespace COSMOS.Equipment
{
    public class MiniGun : Weapon<MiniGunObjectController>
    {
        public float Rate;
        float timer = 0;

        public MiniGun(float rate = 0.1f)
        {
            GameObjectController = new GameObject("MiniGunObjectController", typeof(MiniGunObjectController)).
                GetComponent<MiniGunObjectController>();
            Rate = rate;
        }

        public override void Fire(float delta)
        {
            timer -= delta;
            if (timer <= 0)
            {
                GameObjectController.Fire();
                timer = Rate;
            }
        }
    }
    public class MiniGunObjectController : OutsideWeaponObjectController
    {
        public void Fire()
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            obj.transform.position = transform.position;
            var r = obj.AddComponent<Rigidbody>();
            r.AddForce(transform.forward * 100, ForceMode.Impulse);
            r.useGravity = false;
            GameObject.Destroy(obj, 1);
        }
    }
}
