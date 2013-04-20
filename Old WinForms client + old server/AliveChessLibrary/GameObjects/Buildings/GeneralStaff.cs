using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    class GeneralStaff : IBuildingsInCastle//Slisarenko
    {
        [ProtoMember(1)]
        int[] generalstaff = new int[5] { 1, 1, 1, 1, 1 };
        [ProtoMember(2)]
        int[] queenres = new int[5] { 1, 1, 1, 1, 1 };

        private string name = "Генеральный штаб";
        private GameData context;

        public string Name
        {
            get { return name; }
        }
        public TypeBuildingsInCastle Type()
        {
            return TypeBuildingsInCastle.GeneralStaff;
        }

        public Unit CreateUnit(int count, FabrikUnit F)
        {
            uint name = Convert.ToUInt32(UnitType.Queen);
            int cost = Convert.ToInt32(CostUnit.Seven);
            Unit p = F.CreateQueen(name, cost);
            p.UnitCount = count;
            return p;

        }

        public int[] ResourceForBuildingBuildings
        {
            get { return generalstaff; }
        }

        public GeneralStaff(GameData context)
        {
            this.context = context;
        }
        

        public int[] ResourceCreateUnit
        {
            get { return queenres; }
        }
        

    }
}
