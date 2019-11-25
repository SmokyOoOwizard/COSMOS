using COSMOS.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class lstring : IXmlSerializable
{
    public string Key { get; private set; }
    public Language Language { get; protected set; }
    public string Value { get; protected set; }

    lstring() { }

    public lstring(string key)
    {
        Key = key;
    }

    public string GetString()
    {
        if(Languages.CurrentLanguage == Language)
        {
            return Value;
        }
        else
        {
            Language = Languages.CurrentLanguage;
            return Value = Language.GetString(Key);
        }
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        Boolean isEmptyElement = reader.IsEmptyElement; // (1)
        reader.ReadStartElement();
        if (!isEmptyElement) // (1)
        {
            Key = reader.ReadContentAsString();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteString(Key);
    }

    public static implicit operator lstring(string key)
    {
        return new lstring(key);
    }
    public static implicit operator string(lstring l)
    {
        return l.GetString();
    }
}
