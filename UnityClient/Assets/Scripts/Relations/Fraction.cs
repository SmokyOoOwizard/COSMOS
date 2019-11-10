using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Relations
{
    public class Fraction
    {
        public static IReadOnlyList<Fraction> Fractions
        {
            get
            {
                return fractions.AsReadOnly();
            }
        }
        static List<Fraction> fractions = new List<Fraction>();

        public lstring Name;

    }
}
