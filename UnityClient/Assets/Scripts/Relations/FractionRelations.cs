using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Relations
{
    public static class FractionRelations
    {
        public static IReadOnlyDictionary<Fraction, List<KeyValuePair<Fraction, float>>> Relations
        {
            get
            {
                return new ReadOnlyDictionary<Fraction, List<KeyValuePair<Fraction, float>>>(relations);
            }
        }
        static Dictionary<Fraction, List<KeyValuePair<Fraction, float>>> relations;
    }
}
