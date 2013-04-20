using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;


namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class Voenkomat : IBuildingsInCastle//Slisarenko
    {
        [ProtoMember(1)]
        int[] pawnres = new int[5] { 1, 1, 1, 1, 1 };
        [ProtoMember(2)]
        int[] voenkomatres = new int[5] { 1, 1, 1, 1, 1 };
        private GameData context;
        private string name = "Военкомат";
        //private TypeBuildingsInCastle type = TypeBuildingsInCastle.Voencomat;

        public string Name
        {
            get { return name; }
        }

        public TypeBuildingsInCastle Type()
        {
            return TypeBuildingsInCastle.Voencomat;
        }

        public Unit CreateUnit(int count, FabrikUnit F)
        {
            uint name = Convert.ToUInt32(UnitType.Pawn);
            int cost = Convert.ToInt32(CostUnit.One);
            Unit p = F.CreatePawn(name, cost);
            p.UnitCount = count;
            return p;
        }

        public int[] ResourceCreateUnit
        {
            get { return pawnres; }
        }

        public int[] ResourceForBuildingBuildings
        {
            get { return voenkomatres; }
        }

        public Voenkomat(GameData context)
        {
            this.context = context;
        }
        
        
    }

}
