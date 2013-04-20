using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IBuildingsInCastle//Slisarenko
    {
        Unit CreateUnit(int count, FabrikUnit F);

        TypeBuildingsInCastle Type();

        string Name { get; }

        int[] ResourceForBuildingBuildings { get; }

        int[] ResourceCreateUnit { get; }
    }
}
