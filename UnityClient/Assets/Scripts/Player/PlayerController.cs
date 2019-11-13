using COSMOS.Paterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COSMOS.Player
{
    public class PlayerController : SingletonMono<PlayerController>
    {
        public CameraController cmc;
        public IControllable IC;
        public Vector3 mousePos;
        // Start is called before the first frame update
        void Start()
        {
            //IC = SC;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (IC != null && IC.Exist())
            {

                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");

                Plane horPlane = new Plane(Vector3.up, IC.GetPos());
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance = 0;
                if (horPlane.Raycast(ray, out distance))
                {
                    mousePos = ray.GetPoint(distance);
                }


                Vector3 p = mousePos - IC.GetPos();
                p.y = 0;

                IC.Move(new Vector2(x, y));
                if (p != Vector3.zero)
                {
                    IC.Rotate((float)Math.Round(Quaternion.LookRotation(p, Vector3.up).eulerAngles.y, 2));
                }
                IC.Selected();
            }
        }
        public GameObject GetControllableObject()
        {
            if (PlayerController.instance.IC != null && PlayerController.instance.IC.Exist())
            {
                return ((MonoBehaviour)(PlayerController.instance.IC)).gameObject;
            }
            return null;
        }
    }
}