using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStart.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for collections and enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region IsNullOrEmpty

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <returns>True if the collection is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Determines whether the list is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to check</param>
        /// <returns>True if the list is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty<T>(this List<T>? list)
        {
            return list == null || list.Count <= 0;
        }

        /// <summary>
        /// Determines whether the set is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set</typeparam>
        /// <param name="set">The set to check</param>
        /// <returns>True if the set is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty<T>(this HashSet<T>? set)
        {
            return set == null || set.Count <= 0;
        }

        /// <summary>
        /// Determines whether the dictionary is null or contains no elements.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dictionary">The dictionary to check</param>
        /// <returns>True if the dictionary is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary)
        {
            return dictionary == null || dictionary.Count <= 0;
        }

        /// <summary>
        /// Determines whether the dictionary is null or contains no elements.
        /// </summary>
        /// <param name="dictionary">The dictionary to check</param>
        /// <returns>True if the dictionary is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty(this Dictionary<string, object>? dictionary)
        {
            return dictionary == null || dictionary.Count <= 0;
        }

        #endregion

        #region IsNotNullOrEmpty

        /// <summary>
        /// Determines whether the collection is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <returns>True if the collection is not null and not empty; otherwise, false</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return !source.IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the list is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to check</param>
        /// <returns>True if the list is not null and not empty; otherwise, false</returns>
        public static bool IsNotNullOrEmpty<T>(this List<T> list)
        {
            return !list.IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the set is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set</typeparam>
        /// <param name="set">The set to check</param>
        /// <returns>True if the set is not null and not empty; otherwise, false</returns>
        public static bool IsNotNullOrEmpty<T>(this HashSet<T> set)
        {
            return !set.IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the dictionary is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dictionary">The dictionary to check</param>
        /// <returns>True if the dictionary is not null and not empty; otherwise, false</returns>
        public static bool IsNotNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return !dictionary.IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the dictionary is not null and contains at least one element.
        /// </summary>
        /// <param name="dictionary">The dictionary to check</param>
        /// <returns>True if the dictionary is not null and not empty; otherwise, false</returns>
        public static bool IsNotNullOrEmpty(this Dictionary<string, object> dictionary)
        {
            return !dictionary.IsNullOrEmpty();
        }

        #endregion

        #region AddIf Methods

        /// <summary>
        /// Adds the item to the list if the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The value to add</param>
        /// <param name="condition">The condition to check</param>
        public static void AddIf<T>(this List<T> list, T value, Func<T, bool> condition)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (condition(value))
                list.Add(value);
        }

        /// <summary>
        /// Adds the item to the list if it is not null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The value to add</param>
        public static void AddIfNotNull<T>(this List<T?> list, T? value) where T : class
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (value != null)
                list.Add(value);
        }

        /// <summary>
        /// Adds the item to the list if it is not null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The value to add</param>
        public static void AddIfNotEmpty<T>(this List<T> list, T value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (value != null)
                list.Add(value);
        }

        /// <summary>
        /// Adds the string to the list if it is not null, empty, or whitespace.
        /// </summary>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The string to add</param>
        public static void AddIfNotEmpty(this List<string> list, string value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (!string.IsNullOrWhiteSpace(value))
                list.Add(value);
        }

        /// <summary>
        /// Adds the value to the list if it is greater than zero.
        /// </summary>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The value to add</param>
        public static void AddIfNotZero(this List<double> list, double value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (value > 0)
                list.Add(value);
        }

        /// <summary>
        /// Adds the value to the list if it is greater than zero.
        /// </summary>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The value to add</param>
        public static void AddIfNotZero(this List<int> list, int value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (value > 0)
                list.Add(value);
        }
        
        /// <summary>
        /// Adds the value to the list if it has a value (for nullable value types).
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to add to</param>
        /// <param name="value">The nullable value to add</param>
        public static void AddIfHasValue<T>(this List<T> list, T? value) where T : struct
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
                
            if (value.HasValue)
                list.Add(value.Value);
        }

        #endregion

        #region Enum Helpers

        /// <summary>
        /// Gets a list of all values defined in the specified enum type.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A list containing all enum values</returns>
        public static List<T> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Gets a list of string representations of all values defined in the specified enum type.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A list containing string representations of all enum values</returns>
        public static List<string> GetEnumStringList<T>() where T : Enum
        {
            List<T> enumList = GetEnumList<T>();
            return enumList.ConvertAll(x => x.ToString());
        }

        /// <summary>
        /// Gets a dictionary of all values defined in the specified enum type with their string representations.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A dictionary containing enum values and their string representations</returns>
        public static Dictionary<T, string> GetEnumDictionary<T>() where T : Enum
        {
            return GetEnumList<T>().ToDictionary(k => k, v => v.ToString());
        }

        /// <summary>
        /// Gets a dictionary of all values defined in the specified enum type with their int representations.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A dictionary containing enum values and their int representations</returns>
        public static Dictionary<T, int> GetEnumValueDictionary<T>() where T : Enum
        {
            return GetEnumList<T>().ToDictionary(k => k, v => Convert.ToInt32(v));
        }

        #endregion

        #region Batching and Partitioning

        /// <summary>
        /// Splits a collection into batches of a specified size.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to split</param>
        /// <param name="batchSize">The size of each batch</param>
        /// <returns>A collection of batches</returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than 0.");

            using var enumerator = source.GetEnumerator();
            List<T> batch = new List<T>(batchSize);

            while (enumerator.MoveNext())
            {
                batch.Add(enumerator.Current);
                if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Count > 0)
                yield return batch;
        }

        /// <summary>
        /// Partitions a collection into a specified number of approximately equal parts.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to partition</param>
        /// <param name="partitions">The number of partitions to create</param>
        /// <returns>A collection of partitions</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int partitions)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (partitions <= 0)
                throw new ArgumentOutOfRangeException(nameof(partitions), "Number of partitions must be greater than 0.");

            // First, convert to a list to get the count and enable indexing
            var list = source.ToList();
            var count = list.Count;

            if (count == 0)
                yield break;

            // Calculate size of each partition
            var size = (int)Math.Ceiling((double)count / partitions);

            for (int i = 0; i < partitions; i++)
            {
                var skip = i * size;
                if (skip >= count)
                    break;

                yield return list.Skip(skip).Take(Math.Min(size, count - skip));
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns distinct elements from a sequence using a custom equality comparer for a specific property.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the collection</typeparam>
        /// <typeparam name="TKey">The type of the property to compare</typeparam>
        /// <param name="source">The collection to get distinct elements from</param>
        /// <param name="keySelector">A function to extract the property to compare</param>
        /// <returns>A collection of distinct elements</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.GroupBy(keySelector).Select(g => g.First());
        }

        /// <summary>
        /// Returns a collection with elements that are not null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to filter</param>
        /// <returns>A collection without null elements</returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T>? source) where T : class
        {
            if (source == null)
                return Enumerable.Empty<T>();

            return source.Where(x => x != null);
        }

        /// <summary>
        /// Returns a collection with elements that have a value (for nullable value types).
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to filter</param>
        /// <returns>A collection with elements that have a value</returns>
        public static IEnumerable<T> WhereHasValue<T>(this IEnumerable<T?>? source) where T : struct
        {
            if (source == null)
                return Enumerable.Empty<T>();

            return source.Where(x => x.HasValue).Select(x => x!.Value);
        }

        /// <summary>
        /// Returns the default value if the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <param name="defaultValue">The default value to return</param>
        /// <returns>The collection or the default value</returns>
        public static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> defaultValue)
        {
            return source.IsNullOrEmpty() ? defaultValue : source;
        }

        /// <summary>
        /// Returns the first element of a collection, or the default value if the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to get the first element from</param>
        /// <returns>The first element or the default value</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T>? source, T defaultValue)
        {
            if (source == null)
                return defaultValue;

            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : defaultValue;
        }

        /// <summary>
        /// Returns a value indicating whether a collection contains any of the specified values.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <param name="values">The values to check for</param>
        /// <returns>True if the collection contains any of the specified values; otherwise, false</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values)
        {
            if (source == null || values == null || !values.Any())
                return false;

            return values.Any(value => source.Contains(value));
        }

        /// <summary>
        /// Returns a value indicating whether a collection contains all of the specified values.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <param name="values">The values to check for</param>
        /// <returns>True if the collection contains all of the specified values; otherwise, false</returns>
        public static bool ContainsAll<T>(this IEnumerable<T>? source, params T[]? values)
        {
            if (source == null)
                return false;
            if (values == null || !values.Any())
                return true;

            return values.All(source.Contains);
        }

        #endregion
    }
}