using System;
using System.Collections.Generic;
using System.Linq;
using SimpleStart.Core.Entities;

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

        public static bool IsNullOrEmpty<T>(this PagedList<T> list)
        {
            return list?.Data is null || list.Data.Count <= 0;
        }

        public static bool IsNotNullOrEmpty<T>(this List<T> list)
        {
            return !list.IsNullOrEmpty();
        }

        public static bool IsNotNullOrEmpty<T>(this HashSet<T> list)
        {
            return !list.IsNullOrEmpty();
        }

        public static bool IsNotNullOrEmpty<T>(this PagedList<T> list)
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
    }
}