using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Prototype
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
    public class BindProtoAttribute : Attribute
    {
        public string Name { get; protected set; }
        public BindProtoAttribute(string name)
        {
            Name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class ParseMethodAttribute : Attribute
    {

    }
}
