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
using System.IO;

namespace COSMOS.Prototype
{
    [Manager]
    public static class PrototypeManager
    {
        public readonly static Dictionary<Type, Func<string, object>> ParseUnityTypes = new Dictionary<Type, Func<string, object>>()
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
        public readonly static Dictionary<Type, Func<string, object>> ParseStandardTypes = new Dictionary<Type, Func<string, object>>()
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
            { typeof(string), (s) =>{ return s; } },
        };
        public readonly static Dictionary<Type, Func<string, object>> ParseOtherTypes = new Dictionary<Type, Func<string, object>>();

        public readonly static Dictionary<Type, Func<object, string>> SaveUnityTypes = new Dictionary<Type, Func<object, string>>()
        {
            { typeof(Vector2), (s) =>{ return Saver.SaveVector2((Vector2)s); } },
            { typeof(Vector2?), (s) =>{ return Saver.SaveVector2((Vector2?)s); } },
            { typeof(Vector2Int), (s) =>{ return Saver.SaveVector2Int((Vector2Int)s); } },
            { typeof(Vector2Int?), (s) =>{ return Saver.SaveVector2Int((Vector2Int?)s); } },
            { typeof(Vector3), (s) =>{ return Saver.SaveVector3((Vector3)s); } },
            { typeof(Vector3?), (s) =>{ return Saver.SaveVector3((Vector3?)s); } },
            { typeof(Vector3Int), (s) =>{ return Saver.SaveVector3Int((Vector3Int)s); } },
            { typeof(Vector3Int?), (s) =>{ return Saver.SaveVector3Int((Vector3Int?)s); } },
            { typeof(Vector4), (s) =>{ return Saver.SaveVector4((Vector4)s); } },
            { typeof(Vector4?), (s) =>{ return Saver.SaveVector4((Vector4?)s); } },
            { typeof(Quaternion), (s) =>{ return Saver.SaveQuaternion((Quaternion)s); } },
            { typeof(Quaternion?), (s) =>{ return Saver.SaveQuaternion((Quaternion?)s); } },
            { typeof(Rect), (s) =>{ return Saver.SaveRect((Rect)s); } },
            { typeof(Rect?), (s) =>{ return Saver.SaveRect((Rect?)s); } },
            { typeof(Color), (s) =>{ return Saver.SaveColor((Color)s); } },
            { typeof(Color?), (s) =>{ return Saver.SaveColor((Color?)s); } },
            { typeof(RectOffset), (s) =>{ return Saver.SaveRectOffset(s as RectOffset); } }
        };
        public readonly static Dictionary<Type, Func<object, string>> SaveStandardTypes = new Dictionary<Type, Func<object, string>>()
        {
            { typeof(sbyte), (s) =>{ return Saver.SaveInt((sbyte)s); } },
            { typeof(byte), (s) =>{ return Saver.SaveUInt((byte)s); } },
            { typeof(short), (s) =>{ return Saver.SaveInt((short)s); } },
            { typeof(ushort), (s) =>{ return Saver.SaveUInt((ushort)s); } },
            { typeof(int), (s) =>{ return Saver.SaveInt((int)s); } },
            { typeof(uint), (s) =>{ return Saver.SaveUInt((uint)s); } },
            { typeof(long), (s) =>{ return Saver.SaveInt((long)s); } },
            { typeof(ulong), (s) =>{ return Saver.SaveUInt((ulong)s); } },
            { typeof(char), (s) =>{ return Saver.SaveChar((char)s); } },
            { typeof(float), (s) =>{ return Saver.SaveFloat((float)s); } },
            { typeof(double), (s) =>{ return Saver.SaveDouble((double)s); } },
            { typeof(decimal), (s) =>{ return Saver.SaveDecimal((decimal)s); } },
            { typeof(bool), (s) =>{ return Saver.SaveBool((bool)s); } },
            { typeof(sbyte?), (s) =>{ return Saver.SaveInt((sbyte?)s); } },
            { typeof(byte?), (s) =>{ return Saver.SaveUInt((byte?)s); } },
            { typeof(short?), (s) =>{ return Saver.SaveInt((short?)s); } },
            { typeof(ushort?), (s) =>{ return Saver.SaveUInt((ushort?)s); } },
            { typeof(int?), (s) =>{ return Saver.SaveInt((int?)s); } },
            { typeof(uint?), (s) =>{ return Saver.SaveUInt((uint?)s); } },
            { typeof(long?), (s) =>{ return Saver.SaveInt((long?)s); } },
            { typeof(ulong?), (s) =>{ return Saver.SaveUInt((ulong?)s); } },
            { typeof(char?), (s) =>{ return Saver.SaveChar((char?)s); } },
            { typeof(float?), (s) =>{ return Saver.SaveFloat((float?)s); } },
            { typeof(double?), (s) =>{ return Saver.SaveDouble((double?)s); } },
            { typeof(decimal?), (s) =>{ return Saver.SaveDecimal((decimal?)s); } },
            { typeof(bool?), (s) =>{ return Saver.SaveBool((bool?)s); } },
            { typeof(string), (s) =>{ return s.ToString(); } },
        };
        public readonly static Dictionary<Type, Func<object, string>> SaveOtherTypes = new Dictionary<Type, Func<object, string>>();

        static Dictionary<string, Type> SignaturesName = new Dictionary<string, Type>();
        static Delogger.Logger Log = new Delogger.Logger();

        static Dictionary<Type, Signature> Signatures = new Dictionary<Type, Signature>();
        [InitAsyncMethod(int.MinValue)]
        static Task Init()
        {
            Log.Tags.Add("Parse");
            Log.Tags.Add("Save");
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
                            var saveMethod = methods.FirstOrDefault((x) => {
                                if (x.GetCustomAttribute<SaveMethodAttribute>() != null)
                                {
                                    if (x.IsStatic)
                                    {
                                        if (x.ReturnType == typeof(string))
                                        {
                                            var parameters = x.GetParameters();
                                            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object))
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                Log.Error("paramets should be \"object\" Type: " + type + " method: " + x.Name);
                                            }
                                        }
                                        else
                                        {
                                            Log.Error("return type should be is \"string\" Type: " + type + " method: " + x.Name);
                                        }
                                    }
                                    else
                                    {
                                        Log.Error("save method should be static Type: " + type + " method: " + x.Name);
                                    }
                                }
                                return false;
                            });
                            if(parseMethod != null)
                            {
                                ParseOtherTypes.Add(type, (x) => { return parseMethod.Invoke(null, new object[] { x }); });
                            }
                            if(saveMethod != null)
                            {
                                SaveOtherTypes.Add(type, (x) => { return (string)saveMethod.Invoke(null, new object[] { x }); });
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
        public static T Parse<T>(string xml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            return (T)parseChild(xdoc.DocumentElement, null);
        }
        public static Task<T> ParseAsync<T>(XmlElement xml)
        {
            return Task.Factory.StartNew<T>(() =>
            {
                return (T)parseChild(xml, null);
            });
        }
        public static Task<T> ParseAsync<T>(string xml)
        {
            return Task.Factory.StartNew<T>(() =>
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xml);
                return (T)parseChild(xdoc.DocumentElement, null);
            });
        }

        public static string SavePrototype<T>(T prototype)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                SavePrototypeXml<T>(prototype).WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }
        public static Task<string> SavePrototypeAsync<T>(T prototype)
        {
            return Task.Factory.StartNew<string>(() =>
            {
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    SavePrototypeXml<T>(prototype).WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            });
        }
        public static XmlDocument SavePrototypeXml<T>(T prototype )
        {
            if (Signatures.ContainsKey(prototype.GetType()))
            {
                Signature signature = Signatures[prototype.GetType()];
                XmlDocument xdoc = new XmlDocument();
                XmlElement xroot = (XmlElement)saveChild(xdoc, (prototype, null));
                xdoc.AppendChild(xroot);
                Log.Error(xroot.OuterXml);
                return xdoc;
            }
            else
            {
                throw new Exception("type: " + prototype.GetType() + " its not a prototype");
            }
        }
        public static Task<XmlDocument> SavePrototypeXmlAsync<T>(T prototype)
        {
            return Task.Factory.StartNew<XmlDocument>(() =>
            {
                if (Signatures.ContainsKey(prototype.GetType()))
                {
                    Signature signature = Signatures[prototype.GetType()];
                    XmlDocument xdoc = new XmlDocument();
                    XmlElement xroot = (XmlElement)saveChild(xdoc, (prototype, null));
                    xdoc.AppendChild(xroot);
                    Log.Error(xroot.OuterXml);
                    return xdoc;
                }
                else
                {
                    throw new Exception("type: " + prototype.GetType() + " its not a prototype");
                }
            });
        }
        #region Save
        static XmlNode saveChild(XmlDocument xdoc, (object obj, object[] param) context)
        {
            if (context.obj != null)
            {
                if (context.obj.GetType().IsArray)
                {
                    return saveArray(xdoc, context);
                }
                else if (context.obj.GetType().IsGenericType && context.obj.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    return saveKeyValuePair(xdoc, context);
                }
                else if (context.obj.GetType().GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>) || x.GetGenericTypeDefinition() == typeof(ICollection<>))))
                {
                    return saveCollection(xdoc, context);
                }
                else if (Signatures.ContainsKey(context.obj.GetType()))
                {
                    return savePrototype(xdoc, context);
                }
                return saveValue(xdoc, context);
            }
            return null;
        }
        static XmlNode saveCollection(XmlDocument xdoc, (object obj, object[] param) context)
        {
            string elementName = context.obj.GetType().Name;
            if (context.param != null)
            {
                Signature.PFInfo pfi = (Signature.PFInfo)context.param.FirstOrDefault((x) => x is Signature.PFInfo);
                if (pfi != null)
                {
                    elementName = pfi.Name;
                }
                else
                {
                    string name = (string)context.param.FirstOrDefault(x => x is string);
                    if (!string.IsNullOrEmpty(name))
                    {
                        elementName = name;
                    }
                }
            }
            XmlElement xml = xdoc.CreateElement(elementName);
            foreach (object item in (dynamic)context.obj)
            {
                XmlElement tmp = (XmlElement)saveChild(xdoc, (item, new object[] { "Element" }));
                if(tmp != null)
                {
                    xml.AppendChild(tmp);
                }
            }
            return xml;
        }
        static XmlNode saveKeyValuePair(XmlDocument xdoc, (object obj, object[] param) context)
        { 
            string elementName = "KeyValuePair";
            if (context.param != null)
            {
                Signature.PFInfo pfi = (Signature.PFInfo)context.param.FirstOrDefault((x) => x is Signature.PFInfo);
                if (pfi != null)
                {
                    elementName = pfi.Name;
                }                
            }
            XmlElement xml = xdoc.CreateElement(elementName);
            dynamic tmpPair = context.obj;
            XmlElement k = (XmlElement)saveChild(xdoc, (tmpPair.Key, new object[] { "Key" }));
            XmlElement v = (XmlElement)saveChild(xdoc, (tmpPair.Value, new object[] { "Value" }));
            xml.AppendChild(k);
            xml.AppendChild(v);
            return xml;
        }
        static XmlNode saveArray(XmlDocument xdoc, (object obj, object[] param) context)
        {
            string elementName = context.obj.GetType().Name;
            if (context.param != null)
            {
                Signature.PFInfo pfi = (Signature.PFInfo)context.param.FirstOrDefault((x) => x is Signature.PFInfo);
                if (pfi != null)
                {
                    elementName = pfi.Name;
                }
                else
                {
                    string name = (string)context.param.FirstOrDefault(x => x is string);
                    if (!string.IsNullOrEmpty(name))
                    {
                        elementName = name;
                    }
                }
            }
            XmlElement xml = xdoc.CreateElement(elementName);
            Array array = (Array)context.obj;
            foreach (var item in array)
            {
                XmlElement tmp = (XmlElement)saveChild(xdoc, (item, new object[] { "Element" }));
                if(tmp != null)
                {
                    xml.AppendChild(tmp);
                }
            }
            return xml;
        }
        static XmlNode saveValue(XmlDocument xdoc, (object obj, object[] param) context)
        {
            Func<object, string> func = null;
            Type type = context.obj.GetType();
            if (SaveStandardTypes.ContainsKey(type))
            {
                func = SaveStandardTypes[type];
            }
            else if (SaveUnityTypes.ContainsKey(type))
            {
                func = SaveUnityTypes[type];
            }
            else if (SaveOtherTypes.ContainsKey(type))
            {
                func = SaveOtherTypes[type];
            }

            if (func != null)
            {
                string returnedValue = func(context.obj);
                if (context.param != null)
                {
                    Signature.PFInfo pfi = (Signature.PFInfo)context.param.FirstOrDefault((x) => x is Signature.PFInfo);
                    if (pfi != null)
                    {
                        if (pfi.Attributes.Any((x) => x is BindProtoAttribute && ((BindProtoAttribute)x).SaveInAttribute))
                        {
                            XmlAttribute att = xdoc.CreateAttribute(pfi.Name);
                            att.Value = returnedValue;
                            return att;
                        }
                        else
                        {
                            XmlElement xmle = xdoc.CreateElement(pfi.Name);
                            xmle.InnerText = returnedValue;
                            return xmle;
                        }
                    }
                    else
                    {
                        string name = (string)context.param.FirstOrDefault(x => x is string);
                        if (!string.IsNullOrEmpty(name))
                        {
                            XmlElement xmle = xdoc.CreateElement(name);
                            xmle.InnerText = returnedValue;
                            return xmle;
                        }
                    }
                }
                XmlElement xml = xdoc.CreateElement(type.Name);
                xml.InnerText = returnedValue; 
                return xml;
            }
            else
            {
                Log.Error("parser cant save this type: " + type + " with this value: " + context.obj);
                return null;
            }
        }
        static XmlNode savePrototype(XmlDocument xdoc, (object obj, object[] param) context)
        {
            if (Signatures.ContainsKey(context.obj.GetType()))
            {
                Signature signature = Signatures[context.obj.GetType()];
                XmlElement xroot = xdoc.CreateElement(signature.Name);

                List<Task> tasks = new List<Task>();
                foreach (var pfi in signature.PFs)
                {
                    tasks.Add(Task.Factory.StartNew(() => { return saveChild(xdoc, (pfi.Value.GetValue(context.obj), new object[] { pfi.Value })); }));
                }
                Task.WaitAll(tasks.ToArray());
                foreach (Task<XmlNode> task in tasks)
                {
                    XmlNode tmp = task.Result;
                    if (tmp != null)
                    {
                        if(tmp is XmlAttribute)
                        {
                            xroot.Attributes.Append((XmlAttribute)tmp);
                        }
                        else
                        {
                            xroot.AppendChild(tmp);
                        }
                    }
                }
                return xroot;
            }
            else
            {
                Log.Error("type: " + context.obj.GetType() + " its not a prototype");
                return null;
            }
        }
        #endregion
        #region Parse
        static object parseChild(XmlElement xml, Type type)
        {
            if (type != null && type.IsArray)
            {
                return CreateArray(xml, type);
            }
            else if(type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                Type[] tmp = type.GetGenericArguments();
                return parseKeyValuePair(xml, type, tmp[0], tmp[1]);
            }
            else if (type != null && type.GetInterfaces().Any(x => x.IsGenericType && 
                (x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>) || x.GetGenericTypeDefinition() == typeof(ICollection<>))))
            {
                return CreateCollection(xml, type);
            }
            else if (SignaturesName.ContainsKey(xml.Name) || type != null && Signatures.ContainsKey(type))
            {
                return CreatePrototype(xml, type);
            }
            else if(type != null)
            {
                return CreateValue(xml.InnerText, type);
            }
            return null;
        }
        static object parseAtt(string value, Type type)
        {
            return CreateValue(value, type);
        }
        static object CreateCollection(XmlElement xml, Type type)
        {
            bool isDictinary = type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (type.GetInterfaces().Any(x => x.IsGenericType && 
            (x.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) || x.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>) 
            || x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>))))
            {
                try
                {
                    Type elementType = type.GetInterfaces().FirstOrDefault((x) => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)).GetGenericArguments()[0];
                    object collection = null;
                    if (isDictinary)
                    {
                        collection = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(type.GetInterfaces().FirstOrDefault((x) => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)).GetGenericArguments()));
                    }
                    else
                    {
                        collection = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    }
                    var map = collection.GetType().GetInterfaceMap(typeof(ICollection<>));
                    MethodInfo methodInfo = map.TargetMethods.FirstOrDefault(x => x.Name.Contains("Add"));
                    if (elementType.IsInterface)
                    {
                        elementType = null;
                    }
                    foreach (XmlElement child in xml)
                    {
                        methodInfo.Invoke(collection, new object[] { parseChild(child, elementType) });
                    }
                    return Activator.CreateInstance(type, collection); ;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            else
            {
                var map = type.GetInterfaceMap(typeof(ICollection<>));
                #warning maybe need save methods in signature
                MethodInfo methodInfo = map.TargetMethods.FirstOrDefault(x => x.Name.Contains("Add"));
                if(methodInfo != null)
                {
                    try
                    {
                        object collection = Activator.CreateInstance(type);
                        Type elementType = type.GetInterfaces().FirstOrDefault((x) => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)).GetGenericArguments()[0];
                        if (elementType.IsInterface)
                        {
                            elementType = null;
                        }
                        foreach (XmlElement child in xml)
                        {
                            methodInfo.Invoke(collection, new object[] { parseChild(child, elementType) });
                        }
                        return collection;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
                else
                {
                    Log.Error("this collection not supported: " + type);
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
        static object parseKeyValuePair(XmlElement xml, Type type, Type keyType, Type valueType)
        {
            object key = parseChild(xml.ChildNodes[0] as XmlElement, keyType);
            object value = parseChild(xml.ChildNodes[1] as XmlElement, valueType);
            object pair = Activator.CreateInstance(type, new object[] { key, value });
            return pair;
        }
        static object CreateValue(string value, Type type)
        {
            Func<string, object> func = null;

            if (ParseStandardTypes.ContainsKey(type))
            {
                func = ParseStandardTypes[type];
            }
            else if (ParseUnityTypes.ContainsKey(type))
            {
                func = ParseUnityTypes[type];
            }
            else if (ParseOtherTypes.ContainsKey(type))
            {
                func = ParseOtherTypes[type];
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
        #endregion

    }
}
