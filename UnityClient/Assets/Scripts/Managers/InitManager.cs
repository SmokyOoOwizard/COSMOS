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
                                if (m.GetCustomAttribute(typeof(InitMethodAttribute)) != null)
                                {
                                    m.Invoke(null, null);
                                }
                            }
                        }
                    }
                }
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

}
