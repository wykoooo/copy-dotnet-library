using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public class SpecialConverter
    {
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static long ConvertDateTimeInt(System.DateTime time, DateTime baseTime)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseTime);
            intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        public static System.DateTime ConvertIntDateTime(long d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        public static System.DateTime ConvertIntDateTime(long d, DateTime baseTime)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(baseTime);
            time = startTime.AddSeconds(d);
            return time;
        }

        public static string ConvertStringToStandardDateTimeString(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ConvertStringToStandardDateTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static long UNIX_TIMESTAMP(DateTime dateTime)
        {
            return (dateTime.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
        }
    }
}
