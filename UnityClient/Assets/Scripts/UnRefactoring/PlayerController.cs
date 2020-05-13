﻿using COSMOS.Core.Paterns;
using COSMOS.Space;
using COSMOS.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COSMOS.Player
{
    public class PlayerController : SingletonMono<PlayerController>
    {
        public CameraController cmc;
        public Vector3 mousePos;
        public Character.CharacterController Character;
        public SpaceShip.SpaceShipController Ship;

        // Start is called before the first frame update
        private void Awake()
        {
            InitPatern();
            //GameData.PlayerCharacter = new COSMOS.Character.Character();

        }
        void Start()
        {
            SetCharacter(Character as IControllable);
            cmc.Target = Character.transform;
            cmc.distanceToTarget = 5;
            cmc.Height = 10;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetCharacter(Character as IControllable);
                cmc.Target = Character.transform;
                cmc.distanceToTarget = 5;
                cmc.Height = 10;

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetCharacter(Ship as IControllable);
                cmc.Target = Ship.transform;
                cmc.distanceToTarget = 25;
                cmc.Height = 45;

            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                CharacterMenuUI.Spawn().gameObject.SetActive(!CharacterMenuUI.instance.gameObject.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                GalaxyMapUI.Spawn().gameObject.SetActive(!GalaxyMapUI.instance.gameObject.activeSelf);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (GameData.CurrentControllableObject != null && GameData.CurrentControllableObject.Exist())
            {

                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");

                Plane horPlane = new Plane(Vector3.up, GameData.CurrentControllableObject.GetPos());
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance = 0;
                if (horPlane.Raycast(ray, out distance))
                {
                    mousePos = ray.GetPoint(distance);
                }


                Vector3 p = mousePos - GameData.CurrentControllableObject.GetPos();
                p.y = 0;

                GameData.CurrentControllableObject.Move(new Vector2(x, y));
                if (p != Vector3.zero)
                {
                    GameData.CurrentControllableObject.Rotate((float)Math.Round(Quaternion.LookRotation(p, Vector3.up).eulerAngles.y, 2));
                }
                GameData.CurrentControllableObject.Selected();
            }
        }
        public void SetCharacter(IControllable character)
        {
            if(character != null && character.Exist())
            {
                if (GameData.CurrentControllableObject is SpaceShip.SpaceShipController)
                {
                    SpaceShip.SpaceShipController ssc = GameData.CurrentControllableObject as SpaceShip.SpaceShipController;
                    ssc.WarpChargeStart += (x) => Log.Info("START");
                    ssc.WarpStart += (x) => { SolarSystemManager.LoadSystem("TestName2"); Log.Info("WARP"); };
                }

                GameData.CurrentControllableObject = character;
            }
        }
    }
}