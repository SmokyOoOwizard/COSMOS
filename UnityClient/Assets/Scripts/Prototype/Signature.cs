using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Prototype
{
    public class Signature
    {
        public class PFInfo
        {
            public PropertyInfo Property { get; protected set; }
            public FieldInfo Field { get; protected set; }
            public Type Type { get; protected set; }
            public bool isGeneric { get; protected set; } = false;
            public bool isCollection { get; protected set; } = false;
            public bool isArray { get; protected set; } = false;
            public PFInfo(MemberInfo info)
            {
                if (info.MemberType == MemberTypes.Field)
                {
                    Field = (FieldInfo)info;
                    Type = Field.FieldType;
                }
                else if (info.MemberType == MemberTypes.Property)
                {
                    Property = (PropertyInfo)info;
                    Type = Property.PropertyType;
                }
                else
                {
                    throw new InvalidCastException();
                }

                isArray = Type.IsArray;
                isGeneric = Type.IsGenericType;
                if(Type.GetInterfaces() != null)
                {

                    isCollection = Type.GetInterfaces().Any((t) => { return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>); });
                }

            }

            public void SetValue(object instance, object value)
            {
                if (Field != null)
                {
                    Field.SetValue(instance, value);
                }
                else
                {
                    Property.SetValue(instance, value);
                }
            }
            public object GetValue(object instance)
            {
                if (Field != null)
                {
                    return Field.GetValue(instance);
                }
                else
                {
                    return Property.GetValue(instance);
                }
            }
        }
        public string Name { get; protected set; }
        public Type Type { get; protected set; }
        public Dictionary<string, PFInfo> PFs;
        public Signature(Type type)
        {
            BindProtoAttribute att = type.GetCustomAttribute(typeof(BindProtoAttribute), true) as BindProtoAttribute;
            if (att != null)
            {
                Type = type;
                Name = att.Name;
                if (string.IsNullOrEmpty(Name))
                {
                    Name = type.FullName;
                }
                PFs = new Dictionary<string, PFInfo>();

                const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var properies = type.GetFields(bindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(bindingFlags)).ToArray();
                if (properies != null && properies.Length > 0)
                {
                    foreach (var prop in properies)
                    {
                        var propertyAtt = prop.GetCustomAttribute<BindProtoAttribute>(true);
                        if (propertyAtt != null)
                        {
                            string parseName = propertyAtt.Name;
                            if (string.IsNullOrEmpty(parseName))
                            {
                                parseName = prop.Name;
                            }
                            if (!PFs.ContainsKey(parseName))
                            {
                                PFs.Add(parseName, new PFInfo(prop));
                            }
                            else
                            {
                                Log.Error("already have this parse name: " + parseName + " property: " + prop);
                            }
                        }
                    }
                }
            }
        }
    }
}
