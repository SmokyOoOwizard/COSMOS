using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core.HelpfulStuff
{
    public static partial class Saver
    {
        public static string SaveVector2(Vector2 raw)
        {
            return raw.x + ";" + raw.y;
        }
        public static string SaveVector2(Vector2? raw)
        {
            if (raw.HasValue)
            {
                return raw.Value.x + ";" + raw.Value.y;
            }
            return null;
        }
        public static string SaveVector2Int(Vector2Int raw)
        {
            return raw.x + ";" + raw.y;
        }
        public static string SaveVector2Int(Vector2Int? raw)
        {
            if (raw.HasValue)
            {
                return raw.Value.x + ";" + raw.Value.y;
            }
            return null;
        }
        public static string SaveVector3(Vector3 raw)
        {
            return raw.x + ";" + raw.y + ";" + raw.z;
        }
        public static string SaveVector3(Vector3? raw)
        {
            if (raw.HasValue)
            {
                return raw.Value.x + ";" + raw.Value.y + ";" + raw.Value.z;
            }
            return null;
        }
        public static string SaveVector3Int(Vector3Int raw)
        {
            return raw.x + ";" + raw.y + ";" + raw.z;
        }
        public static string SaveVector3Int(Vector3Int? raw)
        {
            if (raw.HasValue) 
            {
                return raw.Value.x + ";" + raw.Value.y + ";" + raw.Value.z;
            }
            return null;
        }
        public static string SaveVector4(Vector4 raw)
        {
            return raw.x + ";" + raw.y + ";" + raw.z + ";" + raw.w;
        }
        public static string SaveVector4(Vector4? raw)
        {
            if (raw.HasValue)
            {
                return raw.Value.x + ";" + raw.Value.y + ";" + raw.Value.z + ";" + raw.Value.w;
            }
            return null;
        }
        public static string SaveQuaternion(Quaternion raw)
        {
#warning NOT COMPLETE
            return "";
        }
        public static string SaveQuaternion(Quaternion? raw)
        {
#warning NOT COMPLETE
            return "";
        }
        public static string SaveRect(Rect raw)
        {
#warning NOT COMPLETE
            return "";
        }
        public static string SaveRect(Rect? raw)
        {
#warning NOT COMPLETE
            return "";
        }
        public static string SaveColor(Color raw)
        {
            string r = ((int)(raw.r * 255)).ToString("X");
            string g = ((int)(raw.g * 255)).ToString("X");
            string b = ((int)(raw.b * 255)).ToString("X");
            string a = ((int)(raw.a * 255)).ToString("X");
            return r + g + b + a;
        }
        public static string SaveColor(Color? raw)
        {
            if (raw.HasValue) 
            {
                string r = ((int)(raw.Value.r * 255)).ToString("X");
                string g = ((int)(raw.Value.g * 255)).ToString("X");
                string b = ((int)(raw.Value.b * 255)).ToString("X");
                string a = ((int)(raw.Value.a * 255)).ToString("X");
                return r + g + b + a;
            } 
            return null;
        }
        public static string SaveRectOffset(RectOffset raw)
        {
#warning NOT COMPLETE
            return "";
        }
    }
}
