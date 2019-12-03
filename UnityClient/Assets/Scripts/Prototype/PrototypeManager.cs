using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using COSMOS.HelpfullStuff;
using System.Collections.Concurrent;

namespace COSMOS.Prototype
{
    [Manager]
    public static class PrototypeManager
    {
        public readonly static Dictionary<Type, Func<string, object>> UnityTypes = new Dictionary<Type, Func<string, object>>()
        {
            { typeof(Vector2), (s) =>{ return (object)Parser.ParseVector2(s); } },
            { typeof(Vector2Int), (s) =>{ return (object)Parser.ParseVector2Int(s); } },
            { typeof(Vector3), (s) =>{ return (object)Parser.ParseVector3(s); } },
            { typeof(Vector3Int), (s) =>{ return (object)Parser.ParseVector3Int(s); } },
            { typeof(Vector4), (s) =>{ return (object)Parser.ParseVector4(s); } },
            { typeof(Quaternion), (s) =>{ return (object)Parser.ParseQuaternion(s); } },
            { typeof(Rect), (s) =>{ return (object)Parser.ParseRect(s); } },
            { typeof(Color), (s) =>{ return (object)Parser.ParseColor(s); } },
            { typeof(Vector2?), (s) =>{ return (object)Parser.ParseVector2(s); } },
            { typeof(Vector2Int?), (s) =>{ return (object)Parser.ParseVector2Int(s); } },
            { typeof(Vector3?), (s) =>{ return (object)Parser.ParseVector3(s); } },
            { typeof(Vector3Int?), (s) =>{ return (object)Parser.ParseVector3Int(s); } },
            { typeof(Vector4?), (s) =>{ return (object)Parser.ParseVector4(s); } },
            { typeof(Quaternion?), (s) =>{ return (object)Parser.ParseQuaternion(s); } },
            { typeof(Rect?), (s) =>{ return (object)Parser.ParseRect(s); } },
            { typeof(Color?), (s) =>{ return (object)Parser.ParseColor(s); } },
            { typeof(RectOffset), (s) =>{ return (object)Parser.ParseRect(s); } }
        };
        public readonly static Dictionary<Type, Func<string, object>> StandardTypes = new Dictionary<Type, Func<string, object>>()
        {
            { typeof(sbyte), (s) =>{ return (object)Parser.ParseSByte(s); } },
            { typeof(byte), (s) =>{ return (object)Parser.ParseByte(s); } },
            { typeof(short), (s) =>{ return (object)Parser.ParseShort(s); } },
            { typeof(ushort), (s) =>{ return (object)Parser.ParseUShort(s); } },
            { typeof(int), (s) =>{ return (object)Parser.ParseInt(s); } },
            { typeof(uint), (s) =>{ return (object)Parser.ParseUInt(s); } },
            { typeof(long), (s) =>{ return (object)Parser.ParseLong(s); } },
            { typeof(ulong), (s) =>{ return (object)Parser.ParseULong(s); } },
            { typeof(char), (s) =>{ return (object)Parser.ParseChar(s); } },
            { typeof(float), (s) =>{ return (object)Parser.ParseFloat(s); } },
            { typeof(double), (s) =>{ return (object)Parser.ParseDouble(s); } },
            { typeof(decimal), (s) =>{ return (object)Parser.ParseDecimal(s); } },
            { typeof(bool), (s) =>{ return (object)Parser.ParseBool(s); } },
            { typeof(sbyte?), (s) =>{ return (object)Parser.ParseSByteN(s); } },
            { typeof(byte?), (s) =>{ return (object)Parser.ParseByteN(s); } },
            { typeof(short?), (s) =>{ return (object)Parser.ParseShortN(s); } },
            { typeof(ushort?), (s) =>{ return (object)Parser.ParseUShortN(s); } },
            { typeof(int?), (s) =>{ return (object)Parser.ParseIntN(s); } },
            { typeof(uint?), (s) =>{ return (object)Parser.ParseUIntN(s); } },
            { typeof(long?), (s) =>{ return (object)Parser.ParseLongN(s); } },
            { typeof(ulong?), (s) =>{ return (object)Parser.ParseULongN(s); } },
            { typeof(char?), (s) =>{ return (object)Parser.ParseCharN(s); } },
            { typeof(float?), (s) =>{ return (object)Parser.ParseFloatN(s); } },
            { typeof(double?), (s) =>{ return (object)Parser.ParseDoubleN(s); } },
            { typeof(decimal?), (s) =>{ return (object)Parser.ParseDecimalN(s); } },
            { typeof(bool?), (s) =>{ return (object)Parser.ParseBoolN(s); } },
            { typeof(string), (s) =>{ return s; } }
        };
        public readonly static Dictionary<Type, Func<string, object>> OtherTypes = new Dictionary<Type, Func<string, object>>();
        static Dictionary<string, Type> SignaturesName = new Dictionary<string, Type>();
        static Delogger.Logger Log = new Delogger.Logger();

        static Dictionary<Type, Signature> Signatures = new Dictionary<Type, Signature>();
        [InitAsyncMethod(int.MinValue)]
        static Task Init()
        {
            Log.Tags.Add("Parse");
            Log.Tags.Add("Prototype");
            return Task.Factory.StartNew(() =>
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    ConcurrentDictionary<Type, Signature> signatures = new ConcurrentDictionary<Type, Signature>();
                    types.AsParallel().ForAll((type) =>
                    {
                        if (!type.IsAbstract)
                        {
                            var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { }, null);
                            if (c != null)
                            {
                                if (!signatures.ContainsKey(type))
                                {
                                    var att = type.GetCustomAttributes(typeof(BindProtoAttribute), true);
                                    if (att != null && att.Length > 0)
                                    {
                                        string name = (att[0] as BindProtoAttribute).Name;
                                        if (!SignaturesName.ContainsKey(name))
                                        {
                                            Signature tmp = new Signature(type);
                                            signatures.TryAdd(type, tmp);
                                        }
                                        else
                                        {
                                            var tmpType = SignaturesName[name];
                                            if (tmpType == type)
                                            {
                                                Log.Error("already have this parse name: " + name + " of type: " + type);
                                            }
                                            else
                                            {
                                                Log.Error("already have this parse name: " + name + " of" + type + " but another type: " + tmpType + " has this parse name");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log.Error("there can be no prototype without a constructor without parameters. Type: " + type);
                            }

                            var methods = type.GetMethods();
                            var parseMethod = methods.First((x) =>
                            {
                                if(x.GetCustomAttribute<ParseMethodAttribute>() != null)
                                {
                                    if (x.IsStatic)
                                    {
                                        if (x.ReturnType == typeof(object))
                                        {
                                            var parameters = x.GetParameters();
                                            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                Log.Error("paramets should be \"string\" Type: " + type + " method: " + x.Name);
                                            }
                                        }
                                        else
                                        {
                                            Log.Error("return type should be is object Type: " + type + " method: " + x.Name);
                                        }
                                    }
                                    else
                                    {
                                        Log.Error("parse method should be static Type: " + type + " method: " + x.Name);
                                    }
                                }
                                return false;
                            });
                            if(parseMethod != null)
                            {
                                OtherTypes.Add(type, (x) => { return parseMethod.Invoke(null, new object[] { x }); });
                            }
                        }
                    });
                    Signatures = signatures.ToDictionary(entry => entry.Key, entry => entry.Value);
                }
            });
        }

        public static T Parse<T>(XmlElement xml)
        {
            return (T)parse(xml);
        }
        public static Task<T> ParseAsync<T>(XmlElement xml)
        {
            return Task.Factory.StartNew<T>(() =>
            {
                return (T)parse(xml);
            });
        }
        static object parse(XmlElement xml, Signature.PFInfo pfi = null)
        {
            if (pfi != null && pfi.isArray)
            {
                return CreateArray(xml, pfi);
            }
            else if (pfi != null && pfi.isCollection)
            {
                return CreateCollection(xml, pfi);
            }
            else if (Signatures.ContainsKey(pfi.Type))
            {
                return CreatePrototype(xml, pfi.Type);
            }
            else
            {
                return CreateValue(xml, pfi.Type);
            }
        }
        static object parse(XmlElement xml, Type type)
        {
            if (Signatures.ContainsKey(type))
            {
                return CreatePrototype(xml, type);
            }
            else
            {
                return CreateValue(xml, type);
            }
        }
        static object CreateCollection(XmlElement xml, Signature.PFInfo pfi)
        {
            return null;
        }
        static object CreateArray(XmlElement xml, Signature.PFInfo pfi)
        {
            if (pfi.Type.GetArrayRank() < 2)
            {
                List<object> tmpObjects = new List<object>();
                foreach (XmlElement child in xml)
                {
                    object tmp = parse(child, pfi.Type);
                    if(tmp != null)
                    {
                        tmpObjects.Add(tmp);
                    }
                    else
                    {
                        Log.Error("parse value is null Type: " + pfi.Type + " Block: " + child.OuterXml);
                    }
                }
                Array tmpArray = Array.CreateInstance(pfi.Type, tmpObjects.Count);
                for (int i = 0; i < tmpObjects.Count; i++)
                {
                    tmpArray.SetValue(tmpObjects[i], i);
                }
                return tmpArray;
            }
            else
            {
                Log.Error("this parser cant parse arrays more that 1 rank");
                return null;
            }
        }
        static object CreateValue(XmlNode xml, Type type)
        {
            Func<string, object> func = null;

            if (StandardTypes.ContainsKey(type))
            {
                func = StandardTypes[type];
            }
            else if (UnityTypes.ContainsKey(type))
            {
                func = UnityTypes[type];
            }
            else if (OtherTypes.ContainsKey(type))
            {
                func = OtherTypes[type];
            }

            if(func != null)
            {
                return func(xml.Value);
            }
            else
            {
                Log.Error("parser cant parse this type: " + type + " with this value: " + xml.OuterXml);
                return null;
            }
        }
        static object CreatePrototype(XmlElement xml, Type type)
        {
            if (xml == null)
            {
                return null;
            }
            Signature signature = null;
            if(type != null && Signatures.ContainsKey(type))
            {
                signature = Signatures[type];
            }
            else if(SignaturesName.ContainsKey(xml.Name))
            {
                signature = Signatures[SignaturesName[xml.Name]];
            }

            if (signature != null)
            {
                object instance = Activator.CreateInstance(signature.Type);

                foreach (XmlElement child in xml)
                {
                    if (signature.PFs.ContainsKey(child.Name))
                    {
                        Signature.PFInfo pfi = signature.PFs[child.Name];
                        object value = parse(child, pfi);

                        if (value != null)
                        {
                            pfi.SetValue(instance, value);
                        }
                        else
                        {
                            Log.Panic("value cannot be null");
                        }
                    }
                    else
                    {
                        Log.Warning("value cant be parse, because cannot found the property or field  with this name: " + child.Name + " xml block:\n" + child.OuterXml);
                    }
                }

                return instance;
            }
            return null;
        }

    }
}
