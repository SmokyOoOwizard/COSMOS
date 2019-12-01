using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.HelpfullStuff
{
    public static partial class Parser
    {
        public static Vector2 ParseVector2(string raw)
        {
            if (raw != null)
            {
                Vector2 r = new Vector2();
                string[] axis = raw.Split(';');
                float x = 0;
                float.TryParse(axis[0], out x);
                r.x = x;
                if (axis.Length > 1)
                {
                    float y = 0;
                    float.TryParse(axis[1], out y);
                    r.y = y;
                }
                return r;
            }
            return new Vector2();
        }
        public static Vector2? ParseVector2N(string raw)
        {
            if (raw != null)
            {
                Vector2 r = new Vector2();
                string[] axis = raw.Split(';');
                float x = 0;
                if(!float.TryParse(axis[0], out x))
                {
                    return null;
                }
                r.x = x;
                if (axis.Length > 1)
                {
                    float y = 0;
                    if(!float.TryParse(axis[1], out y))
                    {
                        return null;
                    }
                    r.y = y;
                }
                return r;
            }
            return null;
        }
        public static Vector2Int ParseVector2Int(string raw)
        {
            if (raw != null)
            {
                Vector2Int r = new Vector2Int();
                string[] axis = raw.Split(';');
                int x = 0;
                int.TryParse(axis[0], out x);
                r.x = x;
                if (axis.Length > 1)
                {
                    int y = 0;
                    int.TryParse(axis[1], out y);
                    r.y = y;
                }
                return r;
            }
            return new Vector2Int();
        }
        public static Vector3 ParseVector3(string raw)
        {
            if (raw != null)
            {
                Vector3 r = new Vector3();
                string[] axis = raw.Split(';');
                float x = 0;
                float.TryParse(axis[0], out x);
                r.x = x;
                if (axis.Length > 1)
                {
                    float y = 0;
                    float.TryParse(axis[1], out y);
                    r.y = y;
                }
                if (axis.Length > 2)
                {
                    float z = 0;
                    float.TryParse(axis[2], out z);
                    r.z = z;
                }
                return r;
            }
            return new Vector3();
        }
        public static Vector3Int ParseVector3Int(string raw)
        {
            if (raw != null)
            {
                Vector3Int r = new Vector3Int();
                string[] axis = raw.Split(';');
                int x = 0;
                int.TryParse(axis[0], out x);
                r.x = x;
                if (axis.Length > 1)
                {
                    int y = 0;
                    int.TryParse(axis[1], out y);
                    r.y = y;
                }
                if (axis.Length > 2)
                {
                    int z = 0;
                    int.TryParse(axis[2], out z);
                    r.z = z;
                }
                return r;
            }
            return new Vector3Int();
        }
        public static Vector4 ParseVector4(string raw)
        {
            if (raw != null)
            {
                Vector4 r = new Vector4();
                string[] axis = raw.Split(';');
                float x = 0;
                float.TryParse(axis[0], out x);
                r.x = x;
                if (axis.Length > 1)
                {
                    float y = 0;
                    float.TryParse(axis[1], out y);
                    r.y = y;
                }
                if (axis.Length > 2)
                {
                    float z = 0;
                    float.TryParse(axis[2], out z);
                    r.z = z;
                }
                if (axis.Length > 3)
                {
                    float w = 0;
                    float.TryParse(axis[3], out w);
                    r.w = w;
                }
                return r;
            }
            return new Vector4();
        }
        public static Quaternion ParseQuaternion(string raw)
        {
#warning NOT COMPLETE
            return Quaternion.identity;
        }
        public static Rect ParseRect(string raw)
        {
#warning NOT COMPLETE
            return new Rect();
        }
        public static RectOffset ParseRectOffset(string raw)
        {
#warning NOT COMPLETE
            return new RectOffset();
        }
        public static Color ParseColor(string raw)
        {
            string rawR = raw.Substring(0, 2);
            float R = ((float)int.Parse(rawR, System.Globalization.NumberStyles.HexNumber)) / 255f;

            string rawG = raw.Substring(2, 2);
            float G = ((float)int.Parse(rawG, System.Globalization.NumberStyles.HexNumber)) / 255f;

            string rawB = raw.Substring(4, 2);
            float B = ((float)int.Parse(rawB, System.Globalization.NumberStyles.HexNumber)) / 255f;

            float A = 1;
            if (raw.Length > 6)
            {
                string rawA = raw.Substring(6, 2);
                A = ((float)int.Parse(rawA, System.Globalization.NumberStyles.HexNumber)) / 255f;
            }
            return new Color(R, G, B, A);
        }
    }
}
