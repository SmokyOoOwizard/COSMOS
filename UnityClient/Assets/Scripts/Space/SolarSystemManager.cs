using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.DataBase;
using System.Xml;


namespace COSMOS.Space
{
    [Manager]
    public static class SolarSystemManager
    {
        public static readonly List<SolarSystemProto> SolarSystems = new List<SolarSystemProto>();
        public static SolarSystem CurrentSystem { get; private set; }

        [InitMethod]
        public static void Init()
        {
            string config = AssetsDatabase.LoadConfig("SolarSystems");
            //if (!string.IsNullOrEmpty(config))
            //{
            //    XmlDocument xdoc = new XmlDocument();
            //    xdoc.LoadXml(config);
            //    XmlElement xroot = xdoc.DocumentElement;
            //    if (xroot.Name == "SolarSystems")
            //    {
            //        foreach (XmlElement item in xroot)
            //        {
            //            if (item.Name == "SolarSystem")
            //            {
            //                SolarSystems.Add(new SolarSystemProto(item));
            //            }
            //        }
            //    }
            //}
        }
    }
}
