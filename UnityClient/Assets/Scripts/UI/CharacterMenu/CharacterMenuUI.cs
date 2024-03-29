﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Core.Paterns;
using UnityEngine;

namespace COSMOS.UI
{
    public class CharacterMenuUI : SingletonMono<CharacterMenuUI>
    {
        public const string PREFAB_ID = @"Prefabs\UI\CharacterMenu";
        public CharacterMenuEquipmentsTab InventoryObj;
        public CharacterMenuAbilityTab StatsObj;
        public GameObject SkillsObj;

        public void ShowInvenotory()
        {
            if(!InventoryObj.gameObject.activeSelf)
            {
                InventoryObj.gameObject.SetActive(true);
                InventoryObj.Refresh();
            }
            StatsObj.gameObject.SetActive(false);
            //SkillsObj.SetActive(false);
        }
        public void ShowStats()
        {
            if (!StatsObj.gameObject.activeSelf)
            {
                StatsObj.gameObject.SetActive(true);
                StatsObj.Init();
            }
            InventoryObj.gameObject.SetActive(false);
            //SkillsObj.SetActive(false);
        }
        public void ShowSkills()
        {
            //if (!SkillsObj.activeSelf)
            //{
            //    SkillsObj.SetActive(true);
            //}
            //InventoryObj.SetActive(false);
            //StatsObj.SetActive(false);
        }


        public static CharacterMenuUI Spawn()
        {
            if (instance == null)
            {
                GameObject prefab = AssetsDatabase.LoadGameObject(PREFAB_ID);
                GameObject obj = Instantiate(prefab);
                obj.GetComponent<CharacterMenuUI>().InitPatern();
            }
            return instance;
        }
    }
}
