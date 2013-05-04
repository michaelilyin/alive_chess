using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IUnitFactory
    {
        Castle Castle { get; set; }
    }
}
