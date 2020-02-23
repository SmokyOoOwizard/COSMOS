using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Character
{
    public class AbilityBonus
    {
        public enum Operation
        {
            Multiply,
            Divide,
            Add,
            Subtract
        }
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
