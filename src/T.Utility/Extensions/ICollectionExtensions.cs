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
    }
}
