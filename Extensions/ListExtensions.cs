using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static void AddWithCallback<T>(this List<T> collection, T item, Action<T> callback)
        {
            collection.Add(item);
            callback(item);
        }

        public static T AddWithCallback<T, R>(this List<R> collection, R item, Func<R, T> callback)
        {
            collection.Add(item);
            return callback(item);
        }

        public static void AddRangeWithCallback<T>(this List<T> collection, IEnumerable<T> items, Action<IEnumerable<T>> callback)
        {
            collection.AddRange(items);
            callback(items);
        }

        public static T AddRangeWithCallback<T, R>(this List<R> collection, IEnumerable<R> items, Func<IEnumerable<R>, T> callback)
        {
            collection.AddRange(items);
            return callback(items);
        }

        // Source: https://stackoverflow.com/a/11463800
        /// <summary>
        /// Splits a list into an IEnumerable of Lists of nSize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locations"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
