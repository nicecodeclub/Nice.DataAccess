using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Net.Common.Convert
{
    /// <summary>
    /// 类型安全转换
    /// </summary>
    public class DataConvert
    {
        public static int ToInt(string cvtStr, int defaultValue = 0)
        {
            int value = 0;
            if (int.TryParse(cvtStr, out value))
                return value;
            return defaultValue;
        }

        public static int ToInt(object cvtObj, int defaultValue = 0)
        {
            int value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (int.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static int? ToIntNullable(object cvtObj, int? defaultValue = null)
        {
            int value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (int.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static long ToLong(string cvtObj, long defaultValue = 0)
        {
            long value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (long.TryParse(cvtObj, out value))
                return value;
            return defaultValue;
        }

        public static long ToLong(object cvtObj, long defaultValue = 0)
        {
            long value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (long.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static float ToFloat(string cvtObj, float defaultValue = 0)
        {
            float value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (float.TryParse(cvtObj, out value))
                return value;
            return defaultValue;
        }

        public static decimal ToDecimal(object cvtObj, decimal defaultValue = 0)
        {
            decimal value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (decimal.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static decimal? ToDecimalNullable(object cvtObj, decimal? defaultValue = null)
        {
            decimal value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (decimal.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }
        public static double ToDouble(object cvtObj, double defaultValue = 0)
        {
            double value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (double.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static double? ToDoubleNullable(object cvtObj, double? defaultValue = null)
        {
            double value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (double.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }
        public static byte ToByte(object cvtObj, byte defaultValue = 0)
        {
            byte value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (byte.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static DateTime ToDateTime(object cvtObj)
        {
            DateTime value = DateTime.MaxValue;
            if (DateTime.TryParse(cvtObj.ToString(), out value))
                return value;
            return DateTime.MaxValue;
        }

        public static DateTime? ToDateTimeNullable(object cvtObj, DateTime? defaultValue = null)
        {
            DateTime value = DateTime.MaxValue;
            if (cvtObj == null)
                return defaultValue;
            if (DateTime.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }

        public static bool ToBoolean(object cvtObj, bool defaultValue=false)
        {
            bool value;
            if (cvtObj == null)
                return defaultValue;
            if (bool.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        } 

        public static bool? ToBoolNullable(object cvtObj, bool? defaultValue = null)
        {
            bool value;
            if (cvtObj == null)
                return defaultValue;
            if (bool.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }


        public static short ToShort(object cvtObj, short defaultValue = 0)
        {
            short value = 0;
            if (cvtObj == null)
                return defaultValue;
            if (short.TryParse(cvtObj.ToString(), out value))
                return value;
            return defaultValue;
        }
    }
}
