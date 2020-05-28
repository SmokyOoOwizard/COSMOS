using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core
{
    [Manager]
    public static class AttributeFinder
    {
        private class AttributeInfo
        {
            public Type AttributeType { get; private set; }
            public Dictionary<Attribute, Type> Classes = new Dictionary<Attribute, Type>();
            public Dictionary<Attribute, KeyValuePair<Type, MemberInfo>> Methods =
                new Dictionary<Attribute, KeyValuePair<Type, MemberInfo>>();

            public AttributeInfo(Type type)
            {
                AttributeType = type;
            }

            public KeyValuePair<Attribute, Type>[] GetAttributes()
            {
                if (Classes != null)
                {
                    return Classes.ToArray();
                }
                return null;
            }
        }
        private static Dictionary<Type, AttributeInfo> Attributes = new Dictionary<Type, AttributeInfo>();
        private static Dictionary<Type, AttributeInfo> GenericAttributes = new Dictionary<Type, AttributeInfo>();

        [InitMethod(0)]
        private static void Init()
        {
            Attributes.Clear();
            GenericAttributes.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemble in assemblies)
            {
                if (assemble.FullName.StartsWith("Unity") || assemble.FullName.StartsWith("System"))
                {
                    continue;
                }
                Log.Info(assemble.FullName);
                foreach (var type in assemble.GetTypes())
                {
                    var membersCache = new Dictionary<MemberInfo, List<Attribute>>();
                    var typesCache = new List<Attribute>();

                    var atts = type.GetCustomAttributes(false);
                    if (atts != null && atts.Length > 0)
                    {
                        foreach (Attribute att in atts)
                        {
                            Type t = att.GetType();
                            if (t.IsGenericType)
                            {
                                Type tG = t.GetGenericTypeDefinition();
                                if (!GenericAttributes.ContainsKey(tG))
                                {
                                    GenericAttributes.Add(tG, new AttributeInfo(tG));
                                }
                                if (!GenericAttributes[tG].Classes.ContainsKey(att))
                                {
                                    GenericAttributes[tG].Classes.Add(att, type);
                                }
                            }

                            if (!Attributes.ContainsKey(t))
                            {
                                Attributes.Add(t, new AttributeInfo(t));
                            }
                            if (!Attributes[t].Classes.ContainsKey(att))
                            {
                                Attributes[t].Classes.Add(att, type);
                            }
                        }
                    }

                    foreach (var member in type.GetMembers())
                    {
                        var memberAtts = member.GetCustomAttributes(false);
                        if (memberAtts != null && memberAtts.Length > 0)
                        {
                            foreach (Attribute att in memberAtts)
                            {
                                Type t = att.GetType();
                                if (t.IsGenericType)
                                {
                                    Type tG = t.GetGenericTypeDefinition();
                                    if (!GenericAttributes.ContainsKey(tG))
                                    {
                                        GenericAttributes.Add(tG, new AttributeInfo(tG));
                                    }
                                    if (!GenericAttributes[tG].Methods.ContainsKey(att))
                                    {
                                        GenericAttributes[tG].Methods.Add(att,
                                            new KeyValuePair<Type, MemberInfo>(type, member));
                                    }
                                }

                                if (!Attributes.ContainsKey(t))
                                {
                                    Attributes.Add(t, new AttributeInfo(t));
                                }
                                if (!Attributes[t].Methods.ContainsKey(att))
                                {
                                    Attributes[t].Methods.Add(att, new KeyValuePair<Type, MemberInfo>(type, member));
                                }
                            }
                        }
                    }
                }
            }
        }

        public static KeyValuePair<Attribute, Type>[] GetTypesWithAttribute(Type attributeType)
        {
            if (Attributes.ContainsKey(attributeType))
            {
                return Attributes[attributeType].GetAttributes();
            }
            return null;
        }
        public static KeyValuePair<Attribute, Type>[] GetTypesWithGenericAttribute(Type attributeType)
        {
            if (GenericAttributes.ContainsKey(attributeType))
            {
                return GenericAttributes[attributeType].GetAttributes();
            }
            return null;
        }
    }
}
