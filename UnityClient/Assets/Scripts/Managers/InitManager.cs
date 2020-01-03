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
            List<KeyValuePair<int,(Type,MethodInfo, bool)>> methods = new List<KeyValuePair<int, (Type, MethodInfo, bool)>>();
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in item.GetTypes())
                {
                    if (t.GetCustomAttribute(typeof(ManagerAttribute), false) != null)
                    {
                        foreach (var m in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            InitMethodAttribute initMethod = m.GetCustomAttribute<InitMethodAttribute>();
                            if (initMethod != null)
                            {
                                methods.Add(new KeyValuePair<int, (Type, MethodInfo, bool)>(initMethod.Priority, (t, m, false)));
                            }
                            else
                            {
                                InitAsyncMethodAttribute initAsyncMethod = m.GetCustomAttribute<InitAsyncMethodAttribute>();
                                if (initAsyncMethod != null)
                                {
                                    methods.Add(new KeyValuePair<int, (Type, MethodInfo, bool)>(initAsyncMethod.Priority, (t, m, true)));
                                }
                            }
                        }
                    }
                }
            }
            methods.Sort((x, y) => { return y.Key.CompareTo(x.Key); });
            List<Task> tasks = new List<Task>();
            try
            {
                foreach (var method in methods)
                {
                    if (method.Value.Item3)
                    {
                        Log.Info("Init async: " + method.Value.Item1.FullName + " Method: " + method.Value.Item2.Name, "Init", "Async");
                        tasks.Add(method.Value.Item2.Invoke(null, null) as Task);
                    }
                    else
                    {
                        Log.Info("Init: " + method.Value.Item1.FullName + " Method: " + method.Value.Item2.Name, "Init");
                        method.Value.Item2.Invoke(null, null);
                    }
                }
                Task.WaitAll(tasks.ToArray());
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
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class InitAsyncMethodAttribute : Attribute
{
    public int Priority;
    public InitAsyncMethodAttribute(int priority = 0)
    {
        Priority = priority;
    }
}
