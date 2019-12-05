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
using System.Collections;

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
                ConcurrentDictionary<Type, Signature> signatures = new ConcurrentDictionary<Type, Signature>();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    types.ToList().AsParallel().All((type) =>
                    {
                        {
                            var att = type.GetCustomAttributes(typeof(BindProtoAttribute), true);
                            if (att != null && att.Length > 0)
                            { 
                                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { }, null);
                                if (c != null)
                                {
                                    if (!signatures.ContainsKey(type))
                                    {
                                        string name = (att[0] as BindProtoAttribute).Name;
                                        if (string.IsNullOrEmpty(name))
                                        {
                                            name = type.FullName;
                                        }
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
                                else
                                {
                                    Log.Error("there can be no prototype without a constructor without parameters. Type: " + type);
                                }
                            }
                    
                            var methods = type.GetMethods();
                            var parseMethod = methods.FirstOrDefault((x) =>
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
                            return false;
                        }
                    });
                }
                Signatures = signatures.ToDictionary(entry => entry.Key, entry => entry.Value);
            });
        }

        public static T Parse<T>(XmlElement xml)
        {
            return (T)parseChild(xml, typeof(T));
        }
        public static Task<T> ParseAsync<T>(XmlElement xml)
        {
            return Task.Factory.StartNew<T>(() =>
            {
                return (T)parseChild(xml, null);
            });
        }
        static object parseChild(XmlElement xml, Type type)
        {
            if (type != null && type.IsArray)
            {
                return CreateArray(xml, type);
            }
            else if (type != null && type.GetInterfaces().Any(x => x.IsGenericType && 
                (x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>) || x.GetGenericTypeDefinition() == typeof(ICollection))))
            {
                return CreateCollection(xml, type);
            }
            else if (SignaturesName.ContainsKey(xml.Name) || type != null && Signatures.ContainsKey(type))
            {
                return CreatePrototype(xml, type);
            }
            else
            {
                return CreateValue(xml.InnerText, type);
            }
        }
        static object parseAtt(string value, Type type)
        {
            if (OtherTypes.ContainsKey(type))
            {
                return OtherTypes[type].Invoke(value);
            }
            else
            {
                return CreateValue(value, type);
            }
        }
        static object CreateCollection(XmlElement xml, Type type)
        {
            Log.Info("Create collection " + type);
            //if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>)))
            //{
            //
            //}
            //else
            {
                //if (type is IDictionary)
                //{
                //
                //}
                //else
                {   
                    object collection = Activator.CreateInstance(type);
                    var map = type.GetInterfaceMap(typeof(ICollection<>));
                    MethodInfo methodInfo = map.TargetMethods.FirstOrDefault(x => x.Name == "Add");
                    if(methodInfo != null)
                    {
                        Type elementType = type.GetGenericArguments().Single();
                        foreach (XmlElement child in xml)
                        {
                            methodInfo.Invoke(collection, new object[] { parseChild(child, elementType) });
                        }

                        return collection;
                    }
                    else
                    {
                        Log.Error("this collection not supported: " + type);
                    }
                }
            }
            return null;
        }
        static object CreateArray(XmlElement xml, Type type)
        {
            int[] dems = new int[type.GetArrayRank()];
            object[] tmp = parseArrayElements(xml, type.GetElementType(), type.GetArrayRank() - 1, dems);

            List<object> tmpObjects = new List<object>(tmp);

            Array tmpArray = Array.CreateInstance(type.GetElementType(), dems);
            for (int   i = 0; i < tmpObjects.Count; i++)
            {
                tmpArray.SetValue(tmpObjects[i], IdToND(i, dems));
            }
            return tmpArray;
        }
        static int[] IdToND(int ID, int[] arrayLengths)
        {
            int[] indices = new int[arrayLengths.Length];
            for (int i = arrayLengths.Length - 1; i >= 0; i--)
            {
                int offset = 1;
                for (int j = 0; j < i; j++)
                {
                    offset *= arrayLengths[j];
                }
                int remainder = ID % offset;
                indices[i] = (ID - remainder) / offset;
                ID = remainder;
            }
            return indices.Reverse().ToArray();
        }
        static object[] parseArrayElements(XmlElement xml, Type type, int deep, int[] dem)
        {
            List<object> tmpObjects = new List<object>();
            if(dem[deep] < xml.ChildNodes.Count)
            {
                dem[deep] = xml.ChildNodes.Count;
            }
            if (deep > 0)
            {
                foreach (XmlElement child in xml)
                {
                    object[] tmp = parseArrayElements(child, type, deep - 1, dem);
                    tmpObjects.AddRange(tmp);
                }
            }
            else
            {
                foreach (XmlElement child in xml)
                {
                    object tmp = parseChild(child, type);
                    if (tmp != null)
                    {
                        tmpObjects.Add(tmp);
                    }
                    else
                    {
                        Log.Error("parse value is null Type: " + type + " Block: " + child.OuterXml);
                    }
                }
            }
            return tmpObjects.ToArray();
        }
        static object CreateValue(string value, Type type)
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
                return func(value);
            }
            else
            {
                Log.Error("parser cant parse this type: " + type + " with this value: " + value);
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

                foreach (XmlAttribute att in xml.Attributes)
                {
                    if (signature.PFs.ContainsKey(att.Name))
                    {
                        Signature.PFInfo pfi = signature.PFs[att.Name];
                        object value = parseAtt(att.Value, pfi.Type);

                        if (value != null)
                        {
                            pfi.SetValue(instance, value);
                        }
                        else
                        {
                            Log.Panic("value cannot be null");
                        }
                    }
                }

                foreach (XmlElement child in xml)
                {
                    if (signature.PFs.ContainsKey(child.Name))
                    {
                        Signature.PFInfo pfi = signature.PFs[child.Name];
                        object value = parseChild(child, pfi.Type);

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
