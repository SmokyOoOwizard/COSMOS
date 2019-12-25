using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Debug.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommand : Attribute
    {
        public readonly string Name;
        public ConsoleCommand(string name)
        {
            Name = name;
        }
    }
}
