using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    class SchoolOfficers : IBuildingsInCastle//Slisarenko
    {
        [ProtoMember(1)]
        int[] bischopres = new int[5] { 1, 1, 1, 1, 1 };
        [ProtoMember(2)]
        int[] schoolofficersres = new int[5] { 1, 1, 1, 1, 1 };
        private GameData context;

        private string name = "Школа Офицеров";

        public string Name
        {
            get { return name; }
        }
        public TypeBuildingsInCastle Type()
        {
            return TypeBuildingsInCastle.SchoolOfficers;
        }

        public Unit CreateUnit(int count, FabrikUnit F)
        {
            uint name = Convert.ToUInt32(UnitType.Bishop);
            int cost = Convert.ToInt32(CostUnit.Three);
            Unit p = F.CreateBischop(name, cost);
            p.UnitCount = count;
            return p;
        }

        public int[] ResourceCreateUnit
        {
            get { return bischopres; }
        }

        public int[] ResourceForBuildingBuildings
        {
            get { return schoolofficersres; }
        }
        
        public SchoolOfficers(GameData context)
        {
            this.context = context;
        }
        
    }
}
