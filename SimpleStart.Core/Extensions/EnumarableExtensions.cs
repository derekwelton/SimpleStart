using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStart.Core.Extensions
{
    public static class EnumarableExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list is null || list.Count <= 0;
        }
        public static bool IsNullOrEmpty<T>(this HashSet<T> list)
        {
            return list is null || list.Count <= 0;
        }
        public static bool IsNullOrEmpty(this Dictionary<string,object> list)
        {
            return list is null || list.Count <= 0;
        }
        public static bool IsNotNullOrEmpty<T>(this List<T> list)
        {
            return !list.IsNullOrEmpty();
        }
        public static bool IsNotNullOrEmpty<T>(this HashSet<T> list)
        {
            return !list.IsNullOrEmpty();
        }
        public static bool IsNotNullOrEmpty(this Dictionary<string,object> list)
        {
            return !list.IsNullOrEmpty();
        }
        public static List<T> GetEnumList<T>() where T : System.Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        public static List<string> GetEnumStringList<T>() where T : System.Enum
        {
            List<T> enumList = GetEnumList<T>();
            return enumList.ConvertAll(x => x.ToString());
        }


        public static void AddIfNotEmpty(this List<string> list, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) list.Add(value);
        }
        public static void AddIfNotZero(this List<double> list, double value)
        {
            if (value > 0) list.Add(value);
        }
        public static void AddIfNotZero(this List<int> list, int value)
        {
            if (value > 0) list.Add(value);
        }
        public static void AddIfNotEmpty<T>(this List<T> list, T value)
        {
             if(value != null) list.Add(value);
        }
    }
}