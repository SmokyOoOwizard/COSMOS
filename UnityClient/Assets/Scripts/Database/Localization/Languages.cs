using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Localization
{
    [Manager]
    public static class Languages
    {
        static Dictionary<string, Language> AllLanguages = new Dictionary<string, Language>();
        public static Language CurrentLanguage { get; private set; }
        public static void SetCurrentLanguage(string languageName)
        {
            if (AllLanguages.ContainsKey(languageName))
            {
                CurrentLanguage = AllLanguages[languageName];
            }
        }

        [InitMethod(int.MaxValue - 1)]
        public static void LoadLanguages()
        {

        }
    }
}
