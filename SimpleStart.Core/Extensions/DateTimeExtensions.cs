using System;
using System.Globalization;

namespace SimpleStart.Core.Extensions
{
    public static class DateTimeExtensions
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static DateTime SetPart(this DateTime dateTime, int? year, int? month, int? day, int? hour, int? minute, int? second)
        {
            return new DateTime(
                year ?? dateTime.Year,
                month ?? dateTime.Month,
                day ?? dateTime.Day,
                hour ?? dateTime.Hour,
                minute ?? dateTime.Minute,
                second ?? dateTime.Second
            );
        }
        public static DateTime SetYear(this DateTime dateTime, int year)
        {
            return dateTime.SetPart(year, null, null, null, null, null);
        }
        public static DateTime SetMonth(this DateTime dateTime, int month)
        {
            return dateTime.SetPart(null, month, null, null, null, null);
        }
        public static DateTime SetDay(this DateTime dateTime, int day)
        {
            return dateTime.SetPart(null, null, day, null, null, null);
        }
        public static DateTime SetHour(this DateTime dateTime, int hour)
        {
            return dateTime.SetPart(null, null, null, hour, null, null);
        }
        public static DateTime SetMinute(this DateTime dateTime, int minute)
        {
            return dateTime.SetPart(null, null, null, null, minute, null);
        }
        public static DateTime SetSecond(this DateTime dateTime, int second)
        {
            return dateTime.SetPart(null, null, null, null, null, second);
        }
        public static DateTime SetTime(this DateTime dateTime, TimeSpan time)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,time.Hours,time.Minutes,time.Seconds);
        }
        public static string ToLocalString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        }
        public static string ToLocalShortString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToShortDateString();
        }
        public static bool IsBetweenTwoDates(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }
    }
}