using System;
using System.Collections.Generic;
using System.Linq;

namespace T.Utility.Extensions
{
    /// <summary>
    /// ICollection extension methods for common scenarios.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        ///  Indicates whether the specified collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Indicates whether the specified collection is neither null nor empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection != null && collection.Any();
        }

        /// <summary>
        /// Returns distinct elements from a sequence according to a specified key selector function.
        /// https://www.modb.pro/db/104719
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }
}
