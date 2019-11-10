using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Skills.Ability
{
    public class AbilityStatBonus
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }
        public string StatName;
        public float Value;
        public Operation StatOperation; 


        public float Apply(float s)
        {
            switch (StatOperation)
            {
                case Operation.Add:
                    s += Value;
                    break;
                case Operation.Subtract:
                    s -= Value;
                    break;
                case Operation.Multiply:
                    s *= Value;
                    break;
                case Operation.Divide:
                    s /= Value;
                    break;
            }
            return s;
        }
    }
}
