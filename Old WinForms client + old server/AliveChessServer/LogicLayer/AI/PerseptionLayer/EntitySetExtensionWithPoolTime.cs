using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessServer.LogicLayer.AI.PerseptionLayer
{
    public static class EntitySetExtensionWithPoolTime
    {
        private static DateTime m_king_time;
        private static DateTime m_castle_time;
        private static DateTime m_mine_time;
        private static DateTime m_resource_time;

        public static DateTime GetUpdateTime<T>(this EntitySet<T> source)
            where T : class
        {
            Type type = typeof(T);
            if (type.Equals(typeof(Castle)))
                return m_castle_time;
            if (type.Equals(typeof(King)) || type.IsSubclassOf(typeof(King)))
                return m_king_time;
            if (type.Equals(typeof(Mine)))
                return m_mine_time;
            if (type.Equals(typeof(Resource)))
                return m_resource_time;
            else throw new AliveChessException("Sequence contains incorrect type");
        }

        public static void SetUpdateTime<T>(this EntitySet<T> source, DateTime dateTime)
           where T : class
        {
            Type type = typeof(T);
            if (type.Equals(typeof(Castle)))
            {
                m_castle_time = dateTime;
            }
            else
            {
                if (type.Equals(typeof(King)) || type.IsSubclassOf(typeof(King)))
                {
                    m_king_time = dateTime;
                }
                else
                {
                    if (type.Equals(typeof(Mine)))
                    {
                        m_mine_time = dateTime;
                    }
                    else
                    {
                        if (type.Equals(typeof(Resource)))
                        {
                            m_resource_time = dateTime;
                        }
                        else throw new AliveChessException("Sequence contains incorrect type");
                    }
                }
            }
        }
    }
}
