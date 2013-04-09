using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public class FabricBuildings//Slisarenko
    {
        public IBuildingsInCastle BuildVenkomat(GameData gd)
        {
            Voenkomat voenk = new Voenkomat(gd);
            return voenk;
        }

        public IBuildingsInCastle BuildSchollOfficer(GameData gd)
        {
            SchoolOfficers voenk = new SchoolOfficers(gd);
            return voenk;
        }

        public IBuildingsInCastle BuildGeneralStaff(GameData gd)
        {
            GeneralStaff voenk = new GeneralStaff(gd);
            return voenk;
        }

        public IBuildingsInCastle BuildVVU( GameData gd)
        {
            VVU voenk = new VVU(gd);
            return voenk;
        }

        public IBuildingsInCastle BuildStable(GameData gd)
        {
            Stable voenk = new Stable(gd);
            return voenk;
        }
    }
}
