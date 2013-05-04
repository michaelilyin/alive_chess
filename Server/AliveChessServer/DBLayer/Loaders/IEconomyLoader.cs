using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessServer.LogicLayer.EconomyEngine;
using AliveChessServer.LogicLayer.Environment;

namespace AliveChessServer.DBLayer.Loaders
{
    public interface IEconomyLoader
    {
        Economy LoadEconomy(LevelTypes levelType);
    }
}
