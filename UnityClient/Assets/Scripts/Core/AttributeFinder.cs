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
        public static ReadOnlyDictionary<Type, ReadOnlyCollection<Attribute>> ClassesAttributes { get; private set; }
        public static ReadOnlyDictionary<Type, ReadOnlyDictionary<MemberInfo, ReadOnlyCollection<Attribute>>> MembersAttributes { get; private set; }

        static HashSet<Type> searchAttributes = new HashSet<Type>();
        static object searchAttributesLock = new object();

        static Dictionary<Type, Action<Dictionary<Type, List<Attribute>>, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>>> callbacks = 
            new Dictionary<Type, Action<Dictionary<Type, List<Attribute>>, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>>>();

        public static void AddCallback(Type att, Action<Dictionary<Type, List<Attribute>>, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>> callback)
        {
            lock (searchAttributesLock)
            {
                if (!callbacks.ContainsKey(att))
                {
                    callbacks.Add(att, callback);
                    searchAttributes.Add(att);
                    return;
                }
                callbacks[att] += callback;
            }
        }
        public static void RemoveCallBack(Type att, Action<Dictionary<Type, List<Attribute>>, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>> callback)
        {
            lock (searchAttributesLock)
            {
                if (callbacks.ContainsKey(att))
                {
                    callbacks[att] -= callback;
                    if (callbacks[att] == null)
                    {
                        searchAttributes.Remove(att);
                        callbacks.Remove(att);
                    }
                }
            }
        }

        [InitMethod(0)]
        public static void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            ConcurrentDictionary<Type, List<Attribute>> types = new ConcurrentDictionary<Type, List<Attribute>>();

            Dictionary<Type, Dictionary<Type, List<Attribute>>> aTypes = new Dictionary<Type, Dictionary<Type, List<Attribute>>>();

            ConcurrentDictionary<Type, Dictionary<MemberInfo, List<Attribute>>> members = new ConcurrentDictionary<Type, Dictionary<MemberInfo, List<Attribute>>>();

            Dictionary<Type, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>> aMembers = new Dictionary<Type, Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>>>();

            ConcurrentBag<Type> foundTypes = new ConcurrentBag<Type>();

            HashSet<Type> sa;
            lock (searchAttributesLock)
            {
                sa = new HashSet<Type>(searchAttributes);
            }

            assemblies.AsParallel().Any((assemble) =>
            {
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
                            if (sa.Contains(t))
                            {
                                typesCache.Add(att);
                                if (!foundTypes.Contains(t))
                                {
                                    foundTypes.Add(t);
                                }

                            }
                            else if(t.IsGenericType && sa.Contains(t.GetGenericTypeDefinition()))
                            {
                                typesCache.Add(att);

                                Type tG = t.GetGenericTypeDefinition();
                                if (!foundTypes.Contains(tG))
                                {
                                    foundTypes.Add(tG);
                                }
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
                                if (sa.Contains(t))
                                {
                                    if (!membersCache.ContainsKey(member))
                                    {
                                        membersCache.Add(member, new List<Attribute>());
                                    }
                                    if (!foundTypes.Contains(t))
                                    {
                                        foundTypes.Add(t);
                                    }
                                    membersCache[member].Add(att);

                                    if (!foundTypes.Contains(t))
                                    {
                                        foundTypes.Add(t);
                                    }
                                }
                                else if (t.IsGenericType && sa.Contains(t.GetGenericTypeDefinition()))
                                {
                                    if(!membersCache.ContainsKey(member))
                                    {
                                        membersCache.Add(member, new List<Attribute>());
                                    }
                                    membersCache[member].Add(att);

                                    Type tG = t.GetGenericTypeDefinition();
                                    if (!foundTypes.Contains(tG))
                                    {
                                        foundTypes.Add(tG);
                                    }
                                }
                            }
                        }
                    }

                    if(typesCache.Count > 0)
                    {
                        types.TryAdd(type, new List<Attribute>(typesCache));
                    }
                    if(membersCache.Count > 0)
                    {
                        members.TryAdd(type, membersCache);
                    }
                }
                return false;
            });

            ClassesAttributes = new ReadOnlyDictionary<Type, ReadOnlyCollection<Attribute>>(types.ToDictionary(pair => pair.Key, pair => pair.Value.AsReadOnly()));
            MembersAttributes = new ReadOnlyDictionary<Type, ReadOnlyDictionary<MemberInfo, ReadOnlyCollection<Attribute>>>(
                members.ToDictionary(pair => pair.Key, pair => new ReadOnlyDictionary<MemberInfo, ReadOnlyCollection<Attribute>>(
                      pair.Value.ToDictionary(pair2 => pair2.Key, pair2 => pair2.Value.AsReadOnly()))));

            foreach (var t in types)
            {
                foreach (var a in t.Value)
                {
                    var aT = a.GetType();
                    if (foundTypes.Contains(aT))
                    {
                        if (!aTypes.ContainsKey(aT))
                        {
                            aTypes.Add(aT, new Dictionary<Type, List<Attribute>>());
                        }
                        if (!aTypes[aT].ContainsKey(t.Key))
                        {
                            aTypes[aT].Add(t.Key, new List<Attribute>());
                        }
                        aTypes[aT][t.Key].Add(a);
                    }
                    if(aT.IsGenericType && foundTypes.Contains(aT.GetGenericTypeDefinition()))
                    {
                        var aTG = aT.GetGenericTypeDefinition();
                        if (!aTypes.ContainsKey(aTG))
                        {
                            aTypes.Add(aTG, new Dictionary<Type, List<Attribute>>());
                        }
                        if (!aTypes[aTG].ContainsKey(t.Key))
                        {
                            aTypes[aTG].Add(t.Key, new List<Attribute>());
                        }
                        aTypes[aTG][t.Key].Add(a);
                    }
                }
            }

            //foreach (var t in members)
            //{
            //    foreach (var m in t.Value)
            //    {
            //        foreach (var a in m.Value)
            //        {
            //
            //        }
            //    }
            //}

            foreach (var callback in callbacks)
            {
                Dictionary<Type, List<Attribute>> tmpAt = null;
                if (aTypes.ContainsKey(callback.Key))
                {
                    tmpAt = aTypes[callback.Key];
                }
                Dictionary<Type, Dictionary<MemberInfo, List<Attribute>>> tmpAm = null;
                if (aMembers.ContainsKey(callback.Key))
                {
                    tmpAm = aMembers[callback.Key];
                }

                callback.Value?.Invoke(tmpAt, tmpAm);
            }
        }
    }
}
