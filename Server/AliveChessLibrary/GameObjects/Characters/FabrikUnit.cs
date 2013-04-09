using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Characters
{
    public class FabrikUnit//Slisarenko
    {
        private GameData context;

        public FabrikUnit(GameData con)
        {
            this.context = con;
        }

        public Unit CreatePawn(uint name, int cost)
        {
            Unit unpawn = new Unit();
            unpawn.Id = name;
            unpawn.UnitCount = cost;
            return unpawn;
        }
        public Unit CreateBischop(uint name, int cost)
        {
            Unit unpawn = new Unit();
            unpawn.Id = name;
            unpawn.UnitCount = cost;
            return unpawn;
        }
        public Unit CreateRook(uint name, int cost)
        {
            Unit unpawn = new Unit();
            unpawn.Id = name;
            unpawn.UnitCount = cost;
            return unpawn;
        }
        public Unit CreateKnight(uint name, int cost)
        {
            Unit unpawn = new Unit();
            unpawn.Id = name;
            unpawn.UnitCount = cost;
            return unpawn;
        }
        public Unit CreateQueen(uint name, int cost)
        {
            Unit unpawn = new Unit();
            unpawn.Id = name;
            unpawn.UnitCount = cost;
            return unpawn;
        }

    }

}
