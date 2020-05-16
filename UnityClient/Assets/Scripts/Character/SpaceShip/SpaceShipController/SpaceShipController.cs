using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Core.HelpfulStuff;
using COSMOS.Equipment;
using COSMOS.Player;
using COSMOS.SpaceShip.Equipment;
using UnityEngine;

namespace COSMOS.SpaceShip
{
    [RequireComponent(typeof(Rigidbody))]
    public partial class SpaceShipController : MonoBehaviour, IControllable
    {
        public bool Enable;
        public bool EnableInputControl;
        public float TargetRotation;
        public Vector3 torque;

        public SpaceShip SpaceShip { get; protected set; }
        private new Rigidbody rigidbody;
        PID AngleCont, VelocityCont;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            CreateDebugShip();
            GameData.PlayerCharacter = SpaceShip;

            AngleCont = new PID();
            AngleCont.Kp = 9.244681f / 2;
            AngleCont.Ki = 0;
            AngleCont.Kd = 0.06382979f / 2;

            VelocityCont = new PID();
            VelocityCont.Kp = 33.7766f / 2;
            VelocityCont.Ki = 0;
            VelocityCont.Kd = 0.2553191f / 2;
        }
        private void Update()
        {
            EnergyUpdate();
            WarpingProcess();
        }
        private void FixedUpdate()
        {
            if (Enable)
            {
                if (EnableInputControl)
                {
                    TurnUpdate();
                }
            }
        }
        #region IControllable
        public bool Exist()
        {
            return this != null;
        }
        public Vector3 GetPos()
        {
            return transform.position;
        }
        public void Move(Vector2 dir)
        {
            if (Enable)
            {
                if (rigidbody.velocity.magnitude > SpaceShip.MaxSpeed)
                {
                    rigidbody.AddForce(-rigidbody.velocity, ForceMode.Force);
                    return;
                }
                if (EnableInputControl)
                {
                    UseForwardEngine(Mathf.Clamp(dir.y, 0, 1));
                    UseBrakingEngine(Mathf.Clamp(dir.y, -1, 0));
                    UseSideEngine(Mathf.Clamp(dir.x, -1, 1));
                }
            }
        }
        public void Rotate(float angle)
        {
            if (Enable && EnableInputControl)
            {
                TargetRotation = angle;
                if (TargetRotation > 180)
                {
                    TargetRotation -= 360;
                }
            }
        }
        public void Selected()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Brake();
            }
            if (Input.GetMouseButton(0))
            {
                if (SpaceShip.Weapons.Count > 0)
                {
                    SpaceShip.Weapons[0].Fire(Time.deltaTime);
                }
            }
        }
        #endregion
        public void Brake()
        {
            if (EnableInputControl)
            {
                rigidbody.AddTorque(-rigidbody.angularVelocity * 0.9f, ForceMode.Force);
                Vector3 local = transform.InverseTransformVector(rigidbody.velocity);
                //
                UseForwardEngine(Mathf.Abs(Mathf.Clamp((local.z / SpaceShip.MainEngine.MaxForce), -1, 0)));
                UseBrakingEngine(Mathf.Abs(Mathf.Clamp((local.z / SpaceShip.BrakingEngine.MaxForce), 0, 1)));
                UseSideEngine(Mathf.Clamp((-local.x / SpaceShip.SideEngines.MaxForce), -1, 1));

                if(rigidbody.velocity.sqrMagnitude < 0.1f)
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }
        public float GetSpeed()
        {
            return rigidbody.velocity.magnitude;
        }
        public float GetSpeedSqr()
        {
            return rigidbody.velocity.sqrMagnitude;
        }
        public Vector2 GetVelocity()
        {
            return new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
        }
        #region Engines
        public void UseForwardEngine(float power)
        {
            float f = SpaceShip.UseForwardEngine(Mathf.Abs(power));
            if(f > 0)
            {
                rigidbody.AddForce(transform.forward * f, ForceMode.Force);
            }
        }
        public void UseBrakingEngine(float power)
        {
            float f = SpaceShip.UseBrakingEngine(Mathf.Abs(power));
            if (f > 0)
            {
                rigidbody.AddForce(transform.forward * f * -1, ForceMode.Force);
            }
        }
        public void UseSideEngine(float power)
        {
            float f = SpaceShip.UseSideEngine(Mathf.Abs(power));
            if(f > 0)
            {
                rigidbody.AddForce(transform.right * Mathf.Sign(power) * f, ForceMode.Force);
            }
        }
        private void TurnUpdate()
        {
            float target = (Quaternion.Inverse(transform.rotation) * Quaternion.Euler(0, TargetRotation, 0)).eulerAngles.y;
            if (target > 180)
            {
                target -= 360;
            }
            float dt = Time.fixedDeltaTime;

            // The target angle is the user-driven angle that the ship will be (hopefully) 
            // steered towards. The target angle is represented in Unity's scene view by a 
            // white line.
            float targetAngle = transform.eulerAngles.y + (Mathf.Clamp(target, -1, 1)) * SpaceShip.TurnEngines.MaxForce * dt;
            //targetAngle = TargetY;// * MaxRotationSpeed * dt;

            // The angle controller drives the ship's angle towards the target angle.
            // This PID controller takes in the error between the ship's rotation angle 
            // and the target angle as input, and returns a signed torque magnitude.
            float angleError = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);
            float torqueCorrectionForAngle = AngleCont.GetOutput(angleError, dt);

            // The angular veloicty controller drives the ship's angular velocity to 0.
            // This PID controller takes in the negated angular velocity of the ship, 
            // and returns a signed torque magnitude.
            float angularVelocityError = -rigidbody.angularVelocity.y;
            float torqueCorrectionForAngularVelocity = VelocityCont.GetOutput(angularVelocityError, dt);

            // The total torque from both controllers is applied to the ship. If we've got 
            // our gains right, then this force will correctly steer the ship to the target 
            // angle and try to hold it there. The torque vector is represented in Unity's 
            // scene view by a red line.
            torque = transform.up * (torqueCorrectionForAngle + torqueCorrectionForAngularVelocity);

            float f = (float)Math.Round(Mathf.Clamp01(Mathf.Abs(torqueCorrectionForAngle / 10)), 2);
            //Debug.Log((float)Math.Round(Mathf.Clamp01(Mathf.Abs(torque.y / 10)) * Hull.SteeringEngines.FuelConsumption, 2));
            if (SpaceShip.UseTurnEngine(f) > 0)
            {
                //GetComponent<Rigidbody>().AddRelativeTorque(torque);
                rigidbody.AddTorque(torque);
            }
        }
        #endregion
        public void CreateDebugShip()
        {
            SpaceShip = new SpaceShip();
            var set = new InventorySet();
            set.LKeyName = "Equipment test";
            var slot = new InventorySlot();
            slot.AddCheckRule((item) => item is MiniGun);
            set.AddSlot(slot);
            SpaceShip.EquipmentController.AddInventorySet(set);
            set.AddSlot(new InventorySlot());

            Debug.Log(SpaceShip.EquipmentController.AddItem(new MiniGun()));

            set = new InventorySet();
            set.LKeyName = "Inventory test";
            set.AddSlot(new InventorySlot());
            SpaceShip.InventoryController.AddInventorySet(set);

            SpaceShip.ReplaceEngine(new TurnEngine(250, "fuel", 1));
            SpaceShip.ReplaceEngine(new SideEngine(20, "fuel", 1));
            SpaceShip.ReplaceEngine(new MainEngine(20, "fuel", 1));
            SpaceShip.ReplaceEngine(new BrakingEngine(20, "fuel", 1));
            SpaceShip.AddEquipment(new WarpEngine());

            SpaceShip.AddEquipment(new Tank("fuel", 100, 100));
            SpaceShip.AddEquipment(new Tank("DarkEnergy", 1000, 1000));

            SpaceShip.AddEquipment(new MiniGun());
        }
    }
}
