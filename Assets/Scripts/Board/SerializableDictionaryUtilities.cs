using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Presentation
{
    public static class SerializableDictionaryUtilities
    {
        [Serializable]
        public struct SerializableDictionaryKeyValue<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<SerializableDictionaryKeyValue<TKey, TValue>> keyValues)
        {
            return Enumerable.ToDictionary(
                keyValues,
                keySelector: pair => pair.Key,
                elementSelector: pair => pair.Value);
        }
    }
}