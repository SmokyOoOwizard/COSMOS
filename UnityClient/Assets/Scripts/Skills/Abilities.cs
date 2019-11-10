using COSMOS.Skills.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace COSMOS
{
    public class Abilities
    {
        List<MainAblity> abilities = new List<MainAblity>();

        public MainAblity GetAblity(string ability)
        {
            return abilities.Find((x) => x.inerName == ability);
        }
    }
}