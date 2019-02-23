using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey,TValue> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
                return default(TValue);

            return dictionary[key];
        }
    }
    
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return false;

            return !source.Any();
        }
    }
}
