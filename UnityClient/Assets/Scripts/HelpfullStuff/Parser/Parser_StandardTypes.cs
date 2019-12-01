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
        public static sbyte ParseSByte(string raw)
        {
            sbyte i = 0;
            sbyte.TryParse(raw, out i);
            return i;
        }
        public static byte ParseByte(string raw)
        {
            byte i = 0;
            byte.TryParse(raw, out i);
            return i;
        }
        public static short ParseShort(string raw)
        {
            short i = 0;
            short.TryParse(raw, out i);
            return i;
        }
        public static ushort ParseUShort(string raw)
        {
            ushort i = 0;
            ushort.TryParse(raw, out i);
            return i;
        }
        #region int
        public static int ParseInt(string raw)
        {
            int i = 0;
            int.TryParse(raw, out i);
            return i;
        }
        public static Int16 ParseInt16(string raw)
        {
            Int16 i = 0;
            Int16.TryParse(raw, out i);
            return i;
        }
        public static Int32 ParseInt32(string raw)
        {
            Int32 i = 0;
            Int32.TryParse(raw, out i);
            return i;
        }
        public static Int64 ParseInt64(string raw)
        {
            Int64 i = 0;
            Int64.TryParse(raw, out i);
            return i;
        }
        #endregion
        #region uint
        public static uint ParseUInt(string raw)
        {
            uint i = 0;
            uint.TryParse(raw, out i);
            return i;
        }
        public static UInt16 ParseUInt16(string raw)
        {
            UInt16 i = 0;
            UInt16.TryParse(raw, out i);
            return i;
        }
        public static UInt32 ParseUInt32(string raw)
        {
            UInt32 i = 0;
            UInt32.TryParse(raw, out i);
            return i;
        }
        public static UInt64 ParseUInt64(string raw)
        {
            UInt64 i = 0;
            UInt64.TryParse(raw, out i);
            return i;
        }
        #endregion
        public static long ParseLong(string raw)
        {
            long i = 0;
            long.TryParse(raw, out i);
            return i;
        }
        public static ulong ParseULong(string raw)
        {
            ulong i = 0;
            ulong.TryParse(raw, out i);
            return i;
        }
        public static char ParseChar(string raw)
        {
            char i = (char)0;
            char.TryParse(raw, out i);
            return i;
        }
        public static float ParseFloat(string raw)
        {
            float i = 0;
            float.TryParse(raw, out i);
            return i;
        }
        public static double ParseDouble(string raw)
        {
            double i = 0;
            double.TryParse(raw, out i);
            return i;
        }
        public static decimal ParseDecimal(string raw)
        {
            decimal i = 0;
            decimal.TryParse(raw, out i);
            return i;
        }
        public static bool ParseBool(string raw)
        {
            bool i = false;
            bool.TryParse(raw, out i);
            return i;
        }
        // NULLABLE
        #region NULL
        public static sbyte? ParseSByteN(string raw)
        {
            sbyte i = 0;
            if (sbyte.TryParse(raw, out i)) return i;
            return null;
        }
        public static byte? ParseByteN(string raw)
        {
            byte i = 0;
            if (byte.TryParse(raw, out i)) return i;
            return null;
        }
        public static short? ParseShortN(string raw)
        {
            short i = 0;
            if (short.TryParse(raw, out i)) return i;
            return null;
        }
        public static ushort? ParseUShortN(string raw)
        {
            ushort i = 0;
            if (ushort.TryParse(raw, out i)) return i;
            return null;
        }
        #region int
        public static int? ParseIntN(string raw)
        {
            int i = 0;
            if (int.TryParse(raw, out i)) return i;
            return null;
        }
        public static Int16? ParseInt16N(string raw)
        {
            Int16 i = 0;
            if (Int16.TryParse(raw, out i)) return i;
            return null;
        }
        public static Int32? ParseInt32N(string raw)
        {
            Int32 i = 0;
            if (Int32.TryParse(raw, out i)) return i;
            return null;
        }
        public static Int64? ParseInt64N(string raw)
        {
            Int64 i = 0;
            if (Int64.TryParse(raw, out i)) return i;
            return null;
        }
        #endregion
        #region uint
        public static uint? ParseUIntN(string raw)
        {
            uint i = 0;
            if (uint.TryParse(raw, out i)) return i;
            return null;
        }
        public static UInt16? ParseUInt16N(string raw)
        {
            UInt16 i = 0;
            if (UInt16.TryParse(raw, out i)) return i;
            return null;
        }
        public static UInt32? ParseUInt32N(string raw)
        {
            UInt32 i = 0;
            if (UInt32.TryParse(raw, out i)) return i;
            return null;
        }
        public static UInt64? ParseUInt64N(string raw)
        {
            UInt64 i = 0;
            if (UInt64.TryParse(raw, out i)) return i;
            return null;
        }
        #endregion
        public static long? ParseLongN(string raw)
        {
            long i = 0;
            if (long.TryParse(raw, out i)) return i;
            return null;
        }
        public static ulong? ParseULongN(string raw)
        {
            ulong i = 0;
            if (ulong.TryParse(raw, out i)) return i;
            return null;
        }
        public static char? ParseCharN(string raw)
        {
            char i = (char)0;
            if (char.TryParse(raw, out i)) return i;
            return null;
        }
        public static float? ParseFloatN(string raw)
        {
            float i = 0;
            if (float.TryParse(raw, out i)) return i;
            return null;
        }
        public static double? ParseDoubleN(string raw)
        {
            double i = 0;
            if (double.TryParse(raw, out i)) return i;
            return null;
        }
        public static decimal? ParseDecimalN(string raw)
        {
            decimal i = 0;
            if (decimal.TryParse(raw, out i)) return i;
            return null;
        }
        public static bool? ParseBoolN(string raw)
        {
            bool i = false;
            if (bool.TryParse(raw, out i)) return i;
            return null;
        }
        #endregion
        public static Enum ParseEnum(string raw, Type t)
        {
            if (string.IsNullOrEmpty(raw))
            {
                raw = Enum.GetNames(t)[0];
            }

            return (Enum)Enum.Parse(t, raw, true);
        }

    }
}
