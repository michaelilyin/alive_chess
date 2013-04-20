using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;

namespace AliveChessLibrary.Utility
{
    public delegate GuidIDPair UniqueIdHandler();

    public static class EntitySetExtension
    {
        public static bool Exists<T>(this EntitySet<T> source,
            Func<T, bool> p) where T : class
        {
            for (int i = 0; i < source.Count; i++)
                if (p(source[i])) return true;
            return false;
        }

        public static T FindElement<T>(this EntitySet<T> source,
           Func<T, bool> p) where T : class
        {
            for (int i = 0; i < source.Count; i++)
                if (p(source[i])) return source[i];
            return null;
        }

        public static void RemoveElements<T>(this EntitySet<T> source, Func<T, bool> p)
            where T : class
        {
            foreach (T item in Next<T>(source))
                if (p(item)) source.Remove(item);
        }

        public static void ForEach<T>(this EntitySet<T> source, Action<T> action)
            where T : class
        {
            foreach (T item in Next<T>(source))
                action(item);
        }

        private static IEnumerable<T> Next<T>(EntitySet<T> source)
           where T : class
        {
            for (int i = 0; i < source.Count; i++)
                yield return source[i];
        }

        public static List<T> SelectElements<T>(this EntitySet<T> source, 
            Func<T, bool> predicate) where T : class
        {
            List<T> result = new List<T>();
            for (int i = 0; i < source.Count; i++)
                if (predicate(source[i])) result.Add(source[i]);
            return result;
        }

        public static void DoAction<T>(this EntitySet<T> source, Action<T> action) 
            where T : class
        {
            foreach (var item in source)
                action(item);
        }

        public static void DoAction<T>(this EntitySet<T> source, Func<T, bool> selector,
            Action<T> action) where T : class
        {
            IEnumerable<T> target = source.Where(selector);
            foreach (var item in target)
                action(item);
        }

        //public static List<T> SelectElements<T>(this EntitySet<T> source,
        //    BinaryExpression expr) where T : class
        //{
        //    List<T> result = new List<T>();
        //    for (int i = 0; i < source.Count; i++)
        //    {
        //        Func<T, bool> predicate = expr.Conversion.Compile();
        //        if (predicate(source[i])) result.Add(source[i]);
        //    }
        //    return result;
        //}
    }
}
