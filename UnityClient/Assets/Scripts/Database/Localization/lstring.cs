using COSMOS.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class lstring
{
    public readonly string Key;
    public Language Language { get; protected set; }
    public string Value { get; protected set; }

    public lstring(string key)
    {
        Key = key;
    }

    public string GetString()
    {
        if(Languages.CurrentLanguage == Language)
        {
            return Value;
        }
        else
        {
            Language = Languages.CurrentLanguage;
            return Value = Language.GetString(Key);
        }
    }

    public static implicit operator lstring(string key)
    {
        return new lstring(key);
    }
    public static implicit operator string(lstring l)
    {
        return l.GetString();
    }
}
