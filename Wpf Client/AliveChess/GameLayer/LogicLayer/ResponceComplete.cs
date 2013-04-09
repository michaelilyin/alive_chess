using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChess.GameLayer.LogicLayer
{
    public static class ResponceComplete
    {
        public static event ResponceCompleteDelegate responceComplete;

        public static void Execute()
        {
            if (responceComplete != null)
            {
                responceComplete();
            }
        }
    }

    public delegate void ResponceCompleteDelegate();
}
