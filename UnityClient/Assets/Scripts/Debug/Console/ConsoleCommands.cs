using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Debug.Console
{ 
    [Manager]
    public static class ConsoleCommands
    {
        public static Dictionary<string, ConsoleCommandInfo> Commands;

        [InitMethod()]
        public static void Init()
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in item.GetTypes())
                {
                    foreach (var m in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        ConsoleCommand command = m.GetCustomAttribute<ConsoleCommand>();
                        if (command != null)
                        {
                            Commands.Add(command.Name, new ConsoleCommandInfo());
                        }
                    }
                }
            }
        }
    }
}
