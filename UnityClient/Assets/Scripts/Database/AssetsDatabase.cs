﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace COSMOS
{
    [Manager]
    public static class AssetsDatabase
    {
        public class Asset
        {
            public string Type;
            public string Name;
            public string Path;
            public string ID;
            public string[] Tags;
        }
        // ID, asset
        static Dictionary<string, Asset> Assets = new Dictionary<string, Asset>();
        #region LoadAsset
        public static Sprite LoadSprite(string ID)
        {
            if (ID != null && Assets.ContainsKey(ID))
            {
                if (Assets[ID] != null && Assets[ID].Type == "Sprite")
                {
                    Sprite s = Resources.Load<Sprite>(Assets[ID].Path);
                    if (s != null)
                    {
                        return s;
                    }
                    Log.Error("load null sprite " + ID);
                }
                else
                {
                    Log.Error("wrong type " + ID);
                }
            }
            else
            {
                Log.Error("no sprite " + ID);
            }
            return Resources.Load<Sprite>(@"Sprites\Default");

        }
        public static GameObject LoadGameObject(string ID)
        {
            if (ID != null && Assets.ContainsKey(ID))
            {
                if (Assets[ID] != null && Assets[ID].Type == "GameObject")
                {
                    Asset o = Assets[ID];
                    GameObject go = Resources.Load(o.Path, typeof(GameObject)) as GameObject;
                    if (go != null)
                    {
                        return go;
                    }
                    Log.Error("load null gameobject " + ID);
                }
                else
                {
                    Log.Error("wrong type " + ID);
                }
            }
            else
            {
                Log.Error("no gameobject " + ID);
            }
            return null;
        }
        public static Material LoadMaterial(string ID)
        {
            if (ID != null && Assets.ContainsKey(ID))
            {
                if (Assets[ID] != null && Assets[ID].Type == "Material")
                {
                    Asset o = Assets[ID];
                    Material mat = Resources.Load(o.Path, typeof(Material)) as Material;
                    if (mat != null)
                    {
                        return mat;
                    }
                    Log.Error("load null material " + ID);
                }
                else
                {
                    Log.Error("wrong type " + ID);
                }
            }
            else
            {
                Log.Error("no material " + ID);
            }
            return null;
        }
        public static string LoadConfig(string ID)
        {
            if (ID != null && Assets.ContainsKey(ID))
            {
                if (Assets[ID] != null && Assets[ID].Type == "Config")
                {
                    Asset o = Assets[ID];
                    TextAsset proto = Resources.Load(o.Path, typeof(TextAsset)) as TextAsset;
                    if (proto != null)
                    {
                        return proto.text;
                    }
                    Log.Error("load null Config " + ID);
                }
                else
                {
                    Log.Error("wrong type " + ID);
                }
            }
            else
            {
                Log.Error("no config " + ID);
            }
            return null;
        }
        public static string LoadPrototype(string ID)
        {
            ID = @"Prototypes\" + ID;
            if (ID != null && Assets.ContainsKey(ID))
            {
                if (Assets[ID] != null && Assets[ID].Type == "Prototype")
                {
                    Asset o = Assets[ID];
                    TextAsset proto = Resources.Load(o.Path, typeof(TextAsset)) as TextAsset;
                    if (proto != null)
                    {
                        return proto.text;
                    }
                    Log.Error("load null Prototype " + ID);
                }
                else
                {
                    Log.Error("wrong type " + ID);
                }
            }
            else
            {
                Log.Error("no Prototype " + ID);
            }
            return null;
        }
        #endregion
#if !UNITY_EDITOR
        [InitMethod(int.MaxValue)]
#endif
        public static void LoadDatabase()
        {
            TextAsset asset = Resources.Load("AssetsDataBase") as TextAsset;
            if (asset != null)
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(asset.text);
                XmlElement xroot = xdoc.DocumentElement;
                if (xroot != null)
                {
                    foreach (var item in xroot.ChildNodes)
                    {
                        XmlElement go = (XmlElement)item;
                        if (go.Name == "Asset")
                        {
                            XmlElement path = go["Path"];
                            XmlElement tag = go["Tags"];
                            XmlAttribute type = go.Attributes["Type"];
                            XmlAttribute name = go.Attributes["Name"];
                            XmlAttribute id = go.Attributes["ID"];



                            if(path != null && path.InnerText != "" && name != null && type != null && type.InnerText != "" && id != null && id.InnerText != "")
                            {
                                if (!Assets.ContainsKey(id.InnerText))
                                {
                                    Asset a = new Asset();
                                    a.ID = id.InnerText;
                                    a.Type = type.InnerText;
                                    a.Name = name.InnerText;
                                    a.Path = path.InnerText;
                                    if (tag != null)
                                    {
                                        a.Tags = tag.InnerText.Split(',');
                                    }
                                    Assets.Add(id.InnerText, a);
                                }
                            }
                        }
                    }
                }
            }
        }

        #if UNITY_EDITOR
        [InitMethod(int.MaxValue)]
        public static void DebugUpdateSaveLoad()
        {
            UpdateDatabase();
            SaveDatabase();
            LoadDatabase();
        }
        public static void UpdateDatabase()
        {
            UpdateDatabaseGameObjects();
            UpdateDatabaseSprites();
            UpdateDatabaseMaterial();
            UpdateDatabasePrototypes();
            UpdateDatabaseConfigs();
        }
        static void UpdateDatabaseSprites()
        {
            List<string> paths = new List<string>(Directory.GetFiles(@"Assets\Resources", "*.png", SearchOption.AllDirectories));
            paths.AddRange(Directory.GetFiles(@"Assets\Resources", "*.jpg", SearchOption.AllDirectories));
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i] = paths[i].Remove(0, 17).Remove(paths[i].Length-17-4,4);
                string id = paths[i];
                if (!Assets.ContainsKey(id))
                {
                    string[] temp = paths[i].Split('\\');
                    Asset a = new Asset();
                    a.ID = id;
                    a.Type = "Sprite";
                    a.Name = temp[temp.Length - 1];
                    a.Path = paths[i];
                    Assets.Add(id,a);
                }
            }
        }
        static void UpdateDatabaseGameObjects()
        {
            string[] paths = Directory.GetFiles(@"Assets\Resources", "*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = paths[i].Remove(0, 17).Replace(".prefab", "");
                string id = paths[i];
                if (!Assets.ContainsKey(id))
                {
                    string[] temp = paths[i].Split('\\');
                    Asset a = new Asset();
                    a.ID = id;
                    a.Type = "GameObject";
                    a.Name = temp[temp.Length - 1];
                    a.Path = paths[i];
                    Assets.Add(id, a);
                }
            }
        }
        static void UpdateDatabaseMaterial()
        {
            string[] paths = Directory.GetFiles(@"Assets\Resources", "*.mat", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = paths[i].Remove(0, 17).Replace(".mat", "");
                string id = paths[i];
                
                if (!Assets.ContainsKey(id))
                {
                    string[] temp = paths[i].Split('\\');
                    Asset a = new Asset();
                    a.ID = id;
                    a.Type = "Material";
                    a.Name = temp[temp.Length - 1];
                    a.Path = paths[i];
                    Assets.Add(id, a);
                }
            }
        }
        static void UpdateDatabasePrototypes()
        {
            List<string> paths = new List<string>(Directory.GetFiles(@"Assets\Resources\Prototypes", "*.xml", SearchOption.AllDirectories));
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i] = paths[i].Remove(0, 17).Replace(".xml", "");
                string id = paths[i];
                if (!Assets.ContainsKey(id))
                {
                    string[] temp = paths[i].Split('\\');
                    Asset a = new Asset();
                    a.ID = id;
                    a.Type = "Prototype";
                    a.Name = temp[temp.Length - 1];
                    a.Path = paths[i];
                    Assets.Add(id, a);
                }
            }
        }
        static void UpdateDatabaseConfigs()
        {
            List<string> paths = new List<string>(Directory.GetFiles(@"Assets\Resources\Configs", "*.xml", SearchOption.AllDirectories));
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i] = paths[i].Remove(0, 17).Replace(".xml", "");
                string id = paths[i];
                if (!Assets.ContainsKey(id))
                {
                    string[] temp = paths[i].Split('\\');
                    Asset a = new Asset();
                    a.ID = id;
                    a.Type = "Config";
                    a.Name = temp[temp.Length - 1];
                    a.Path = paths[i];
                    Assets.Add(id, a);
                }
            }
        }
        public static void SaveDatabase()
        {
            if (!Directory.Exists(@"Assets\Resources"))
            {
                Directory.CreateDirectory(@"Assets\Resources");
            }
            if (File.Exists(@"Assets\Resources\AssetsDatabase.txt"))
            {
                File.Delete(@"Assets\Resources\AssetsDatabase.txt");
            }
            XmlDocument xdoc = new XmlDocument();
            XmlElement xroot = xdoc.CreateElement("AssetsDatabase");
            foreach (var item in Assets)
            {
                if (item.Value != null)
                {
                    XmlElement AssetElement = xdoc.CreateElement("Asset");

                    XmlElement PathElement = xdoc.CreateElement("Path");
                    PathElement.InnerText = (item.Value).Path;

                    XmlAttribute IDAttribute = xdoc.CreateAttribute("ID");
                    IDAttribute.InnerText = item.Key;

                    XmlAttribute NameAttribute = xdoc.CreateAttribute("Name");
                    NameAttribute.InnerText = (item.Value).Name;

                    XmlAttribute TypeAttribute = xdoc.CreateAttribute("Type");
                    TypeAttribute.InnerText = (item.Value).Type;

                    AssetElement.AppendChild(PathElement);

                    AssetElement.Attributes.Append(IDAttribute);
                    AssetElement.Attributes.Append(NameAttribute);
                    AssetElement.Attributes.Append(TypeAttribute);


                    if ((item.Value).Tags != null && (item.Value).Tags.Length > 0)
                    {
                        XmlElement TagElement = xdoc.CreateElement("Tags");
                        TagElement.InnerText = string.Join(",", (item.Value).Tags.ToArray());
                        AssetElement.AppendChild(TagElement);
                    }

                    xroot.AppendChild(AssetElement);
                }
            }
            xdoc.AppendChild(xroot);
            xdoc.Save(@"Assets\Resources\AssetsDatabase.txt");
        }
        #endif
    }
}
