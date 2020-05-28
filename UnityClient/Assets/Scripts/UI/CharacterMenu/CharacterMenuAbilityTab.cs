using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Character;
using COSMOS.Core.Paterns;
using UnityEngine;

namespace COSMOS.UI
{
    public class CharacterMenuAbilityTab : MonoBehaviour
    {
        [SerializeField]
        GameObject Content;

        public void Init()
        {
            //if (GameData.PlayerCharacter != null)
            //{
            //    Ability[] abilities = GameData.PlayerCharacter.GetAbilities();
            //    if (abilities == null || GameData.PlayerCharacter == null)
            //    {
            //        return;
            //    }
            //    foreach (Transform child in Content.transform)
            //    {
            //        GameObject.Destroy(child.gameObject);
            //    }
            //
            //    for (int i = 0; i < abilities.Length; i++)
            //    {
            //        var abilityUI = AbilityUI.Spawn(abilities[i]);
            //        if (abilityUI != null)
            //        {
            //            abilityUI.transform.SetParent(Content.transform);
            //        }
            //    }
            //}
        }

    }
}
