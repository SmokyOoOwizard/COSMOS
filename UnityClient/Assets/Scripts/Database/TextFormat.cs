using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Database
{
    public static class TextFormat
    {
        static readonly Dictionary<Type, Func<string, string>> typesFormates = new Dictionary<Type, Func<string, string>>();
        public static void AddParseType(Type type, Func<string, string> parseMethod)
        {
            if (typesFormates.ContainsKey(type))
            {
                typesFormates[type] = parseMethod;
            }
            else
            {
                typesFormates.Add(type, parseMethod);
            }
        }
        public static string ParseDefaultTag(string tag)
        {
            return tag;
        }
        public static string ParseTag(string tag, object obj)
        {
            if (typesFormates.ContainsKey(obj.GetType()))
            {
                return typesFormates[obj.GetType()].Invoke(tag);
            }
            return ParseDefaultTag(tag);
        }
        public static string[] ParseTags(string[] tags, object obj)
        {
            Func<string, string> parse = null;
            if (typesFormates.ContainsKey(obj.GetType()))
            {
                parse = typesFormates[obj.GetType()];
            }
            string[] returns = new string[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                returns[i] = parse(tags[i]);
            }
            return returns;
        }
        public static string Format(string rawText)
        {
            return rawText;
        }
    }
}
