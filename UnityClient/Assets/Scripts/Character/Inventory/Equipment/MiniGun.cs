using UnityEngine;
using System.Collections.Generic;

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
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = transform.position;
            var r = obj.AddComponent<Rigidbody>();
            r.AddForce(transform.forward * 100, ForceMode.Impulse);
            r.useGravity = false;
            var damage = obj.AddComponent<BulletDamage>();
            damage.Damage = 10;
            GameObject.Destroy(obj, 1);
        }
    }
    public class BulletDamage : UnityEngine.MonoBehaviour
    {
        public float Damage;

        private void OnCollisionEnter(Collision collision)
        {
            var des = collision.gameObject.GetComponent<DestructibleObject>();
            if (des != null)
            {
                Damage e = new Damage(new KeyValuePair<DamageType, float>[] {
                    new KeyValuePair<DamageType,float>(new DamageType(){ Type = "Physics" }, Damage)  });
                des.ApplyDamage(e);
                Destroy(gameObject);
            }
        }
    }
}
