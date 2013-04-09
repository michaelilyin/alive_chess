using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public enum DialogState
    {
        NoState = 0, // состояние не определено
        Agree   = 1, // согласие
        Refuse  = 2, // отказ
        Offer   = 3  // предложение
    }
}
