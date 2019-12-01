using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Managers
{
    public class InitManager : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
            LogManager.Init();
            InitAllManagers();
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void InitAllManagers()
        {
            List<KeyValuePair<int,(Type,MethodInfo)>> methods = new List<KeyValuePair<int, (Type, MethodInfo)>>();
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in item.GetTypes())
                {
                    if (t.GetCustomAttribute(typeof(ManagerAttribute), false) != null)
                    {
                        if (t.IsAbstract && t.IsSealed)
                        {
                            foreach (var m in t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                            {
                                InitMethodAttribute initMethod = m.GetCustomAttribute(typeof(InitMethodAttribute)) as InitMethodAttribute;
                                if (initMethod != null)
                                {
                                    methods.Add(new KeyValuePair<int, (Type, MethodInfo)>(initMethod.Priority, (t, m)));
                                }
                            }
                        }
                    }
                }
            }
            methods.Sort((x, y) => { return y.Key.CompareTo(x.Key); });
            try
            {
                foreach (var method in methods)
                {
                    Log.Info("Init: " + method.Value.Item1.FullName + " Method: " + method.Value.Item2.Name, "Init");
                    method.Value.Item2.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Init");
            }
        }
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ManagerAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class InitMethodAttribute : Attribute
{
    public int Priority;
    public InitMethodAttribute(int priority = 0)
    {
        Priority = priority;
    }
}
