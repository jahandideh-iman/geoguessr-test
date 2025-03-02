using System.Collections.Generic;


namespace GeoGuessr.Game
{
    public static class RandomUtilities
    {
        public static T? RandomElement<T>(this IReadOnlyList<T> list) where T : class
        {
            if(list.Count == 0)
            {
                return null;
            }
            var randomIndex =UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}