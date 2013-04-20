using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;


namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    class VVU : IBuildingsInCastle//Slisarenko
    {
        [ProtoMember(1)]
        int[] rookres = new int[5] { 1, 1, 1, 1, 1 };

        [ProtoMember(2)]
        int[] vvures = new int[5] { 2, 1, 1, 1, 1 };

        private GameData context;
        private string name = "Высший Военный Университет";

        public string Name
        {
            get { return name; }
        }

        public TypeBuildingsInCastle Type()
        {
            return TypeBuildingsInCastle.VVU;
        }

        public Unit CreateUnit(int count, FabrikUnit F)
        {
            uint name = Convert.ToUInt32(UnitType.Rook);
            int cost = Convert.ToInt32(CostUnit.Five);
            Unit p = F.CreateRook(name, cost);
            p.UnitCount = count;
            return p;
        }

        public int[] ResourceCreateUnit
        {
            get { return rookres; }
        }

        public int[] ResourceForBuildingBuildings
        {
            get { return vvures; }
        }
        
        public VVU (GameData context)
        {
            this.context = context;
        }
        
    }
}
