using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Race
{
    public class Race
    {
        public static IReadOnlyList<Race> Races
        {
            get
            {
                return races.AsReadOnly();
            }
        }
        static List<Race> races = new List<Race>();

        public lstring Name { get; protected set; }
        public Dictionary<Race, float> relations = new Dictionary<Race, float>();
    }
}
