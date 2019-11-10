using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Localization
{
    public class Language
    {
        Dictionary<string, string> Keys = new Dictionary<string, string>();
        public string GetString(string key)
        {
            if (Keys.ContainsKey(key))
            {
                return Keys[key];
            }
            else
            {
                return key;
            }
        }
    }
}
