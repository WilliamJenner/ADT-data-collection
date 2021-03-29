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
    }
}
