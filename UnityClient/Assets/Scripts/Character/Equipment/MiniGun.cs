using UnityEngine;

namespace COSMOS.Equipment
{
    public class MiniGun : Weapon<MiniGunObjectController>
    {
        public float Rate;
        float timer = 0;

        public MiniGun(float rate = 0.1f)
        {
            Rate = rate;
        }

        public override void CreateObjectController()
        {
            Log.Info("CREATE MINI GUB OBJECT CONTROLLER");
            GameObjectController = new GameObject("MiniGunObjectController", typeof(MiniGunObjectController)).
                GetComponent<MiniGunObjectController>();
        }

        public override void Fire(float delta)
        {
            timer -= delta;
            if (timer <= 0)
            {
                if (GameObjectController != null)
                {
                    GameObjectController.Fire();
                }
                else
                {
                    Log.Error(GameObjectController != null);
                }
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
