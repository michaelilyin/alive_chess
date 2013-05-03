using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace AliveChess.GameLayer.LogicLayer
{
    public static class CustomConverter
    {
        public static List<T> EntitySetToList<T>(EntitySet<T> entitySet) where T : class
        {
            List<T> res = new List<T>();
            if (entitySet != null)
            {
                foreach (T item in entitySet)
                {
                    res.Add(item);
                }
            }
            return res;
        }

        public static EntitySet<T> ListToEntitySet<T>(List<T> list) where T : class
        {
            EntitySet<T> res = new EntitySet<T>();
            if (list != null)
            {
                res.AddRange(list);
            }
            return res;
        }
    }
}
