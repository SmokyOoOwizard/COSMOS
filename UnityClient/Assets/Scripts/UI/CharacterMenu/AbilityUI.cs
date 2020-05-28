using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Character;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace COSMOS.UI
{
    public class AbilityUI : MonoBehaviour
    {
        public const string PREFAB_ID = @"Prefabs\UI\AbilitySlotUI";

        [SerializeField]
        Image abilityIcon;
        [SerializeField]
        TextMeshProUGUI nameText;
        [SerializeField]
        TextMeshProUGUI totalValueText;
        [SerializeField]
        TextMeshProUGUI detailValueText;

        //public Ability CurrentAbility { get; private set; }
        //
        //
        //public void Init(Ability ability)
        //{
        //    if(ability == null)
        //    {
        //        return;
        //    }
        //    if(CurrentAbility != null)
        //    {
        //        CurrentAbility.RemoveListener(OnAbilityUpdate);
        //    }
        //    CurrentAbility = ability;
        //    CurrentAbility.AddListener(OnAbilityUpdate);
        //    updateStuff();
        //}

        //private void OnAbilityUpdate(uint flags, EventArg args)
        //{
        //    updateStuff();
        //}

        //void updateStuff()
        //{
        //    abilityIcon.sprite = AssetsDatabase.LoadSprite(CurrentAbility.IconID);
        //    lstring name = CurrentAbility.LKeyName;
        //    nameText.SetText(name);
        //    totalValueText.SetText(CurrentAbility.CalculatedStatWithBonuses + "");
        //    detailValueText.SetText(CurrentAbility.DetailTotalStat);
        //}

        //public static AbilityUI Spawn(Ability ability)
        //{
        //    GameObject prefab = AssetsDatabase.LoadGameObject(PREFAB_ID);
        //    if(prefab == null)
        //    {
        //        Log.Error("prefab for abilityUI not found. path:" + PREFAB_ID);
        //        return null;
        //    }
        //
        //    GameObject go = GameObject.Instantiate(prefab);
        //    AbilityUI abilityUI = go.GetComponent<AbilityUI>();
        //    abilityUI.Init(ability);
        //    return abilityUI;
        //}
    }
}
