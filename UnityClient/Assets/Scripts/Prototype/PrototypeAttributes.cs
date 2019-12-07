using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
public class BindProtoAttribute : Attribute
{
    public string Name { get; protected set; }
    public bool SaveInAttribute { get; protected set; }
    public BindProtoAttribute(bool saveInAttribute = false)
    {
        SaveInAttribute = saveInAttribute;
    }
    public BindProtoAttribute(string name, bool saveInAttribute = false)
    {
        Name = name;
        SaveInAttribute = saveInAttribute;
    }
}
[AttributeUsage(AttributeTargets.Method)]
public class ParseMethodAttribute : Attribute
{

}
[AttributeUsage(AttributeTargets.Method)]
public class SaveMethodAttribute : Attribute
{

}
