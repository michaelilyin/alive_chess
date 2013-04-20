using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessServer.LogicLayer.AI.MonitorLayer
{
    public class AStarNodeComparer
    {
        /// <summary>
        /// сравнение узлов по стоимости F
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(AStarNode x, AStarNode y)
        {
            if (x.F > y.F)
                return 1;
            else if (x.F < y.F)
                return -1;
            else return 0;
        }
    }
}
