using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.HelpfulStuff
{
    public static partial class Saver
    {
        public static string SaveInt(long raw)
        {
            return raw.ToString();
        }
        public static string SaveInt(long? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveUInt(ulong raw)
        {
            return raw.ToString();
        }
        public static string SaveUInt(ulong? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveChar(char raw)
        {
            return raw.ToString();
        }
        public static string SaveChar(char? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveFloat(float raw)
        {
            return raw.ToString();
        }
        public static string SaveFloat(float? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveDouble(double raw)
        {
            return raw.ToString();
        }
        public static string SaveDouble(double? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveDecimal(decimal raw)
        {
            return raw.ToString();
        }
        public static string SaveDecimal(decimal? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveBool(bool raw)
        {
            return raw.ToString();
        }
        public static string SaveBool(bool? raw)
        {
            if (raw.HasValue)
            {
                return raw.ToString();
            }
            return null;
        }
        public static string SaveEnum(Enum @enum)
        {
            return @enum.ToString();
        }
    }
}
