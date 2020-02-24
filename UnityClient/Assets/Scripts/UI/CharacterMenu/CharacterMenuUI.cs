using System;
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
        public GameObject InventoryObj;
        public GameObject StatsObj;
        public GameObject SkillsObj;

        public void ShowInvenotory()
        {
            if(!InventoryObj.activeSelf)
            {
                InventoryObj.SetActive(true);
            }
            StatsObj.SetActive(false);
            //SkillsObj.SetActive(false);
        }
        public void ShowStats()
        {
            if (!StatsObj.activeSelf)
            {
                StatsObj.SetActive(true);
            }
            InventoryObj.SetActive(false);
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
    }
}