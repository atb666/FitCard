using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.DataLayer
{
    internal static class DataUtils
    {
        public static int? GetInt32N(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }
        public static long? GetInt64N(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return null;
            }

            return Convert.ToInt64(value);
        }
        public static DateTime? GetDateTimeN(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }
    }
}
