using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    class Stable : IBuildingsInCastle//Slisarenko
    {
        [ProtoMember(1)]
        int[] knightres = new int[5] { 1, 1, 1, 1, 1 };
        [ProtoMember(2)]
        int[] stableres = new int[5] { 1, 1, 1, 1, 1 };
        private GameData context;
        private string name = "Конюшня";

        public string Name
        {
            get { return name; }
        }

        public TypeBuildingsInCastle Type()
        {
            return TypeBuildingsInCastle.Stable;
        }

        public Unit CreateUnit(int count, FabrikUnit F)
        {
            uint name = Convert.ToUInt32(UnitType.Knight);
            int cost = Convert.ToInt32(CostUnit.Three);
            Unit p = F.CreateKnight(name, cost);
            p.UnitCount = count;
            return p;
        }

        public int[] ResourceForBuildingBuildings
        {
            get { return stableres; }
        }

        public int[] ResourceCreateUnit
        {
            get { return knightres; }
        }

        public Stable (GameData context)
        {
            this.context = context;
        }
        
    }
}
