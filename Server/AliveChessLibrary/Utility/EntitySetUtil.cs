#if !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace AliveChessLibrary.Utility
{
    public delegate GuidIDPair UniqueIdHandler();

    public static class EntitySetUtil
    {
        public static List<T> Filter<T>(this EntitySet<T> source,
            Func<T, bool> predicate) where T : class
        {
            List<T> result = new List<T>();
            for (int i = 0; i < source.Count; i++)
                if (predicate(source[i])) result.Add(source[i]);
            return result;
        }
    }
}

#endif