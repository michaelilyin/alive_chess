using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Landscapes;
//using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    //[ProtoContract]
    public class Leader : King
    {
       // [ProtoMember(1)]
        private King _king;
        private EntitySet<King> _successors;

        //public Leader()
        //    : base()
        //{
        //}

        public Leader(King king)
            : base()
        {
            this._king = king;
            this._successors = new EntitySet<King>();
        }

        #region Overrides

        public override void AddCastle(Castle castle) { _king.AddCastle(castle); }

        public override void AddMine(Mine mine) { _king.AddMine(mine); }

        public override void AddSteps(Queue<Position> steps) { _king.AddSteps(steps); }

        public override void AddView(MapPoint point) { _king.AddView(point); }

        public override void ClearSteps() { _king.ClearSteps(); }

        public override void ComeInCastle(Castle castle) { _king.ComeInCastle(castle); }

        public override MapPoint CreateView(int x, int y) { return _king.CreateView(x, y); }

        public override bool Equals(uint other) { return _king.Equals(other); }

        public override Castle GetCastle(uint id) { return _king.GetCastle(id); }

        public override Mine GetMine(uint id) { return _king.GetMine(id); }

        public override bool HasCastle(uint id) { return _king.HasCastle(id); }

        public override bool HasMine(uint id) { return _king.HasMine(id); }

        public override void LeaveCastle() { _king.LeaveCastle(); }

        public override void MoveBy(Position step) { _king.MoveBy(step); }

        public override void OutOfGame() { _king.OutOfGame(); }

        public override void RemoveAllCastles() { _king.RemoveAllCastles(); }

        public override void RemoveAllMines() { _king.RemoveAllMines(); }

        public override void RemoveCastle(Castle castle) { _king.RemoveCastle(castle); }

        public override void RemoveCastle(uint id) { _king.RemoveCastle(id); }

        public override void RemoveMine(uint id) { _king.RemoveMine(id); }

        public override void RemoveMine(Mine mine) { _king.RemoveMine(mine); }

        public override Castle SearchCastle() { return _king.SearchCastle(); }

        public override bool Update() { return _king.Update(); }

        public override void UpdateVisibleSpace(VisibleSpace sector){_king.UpdateVisibleSpace(sector); }

        public override uint Id
        {
            get { return _king.Id; }
        }

        public override Guid DbId
        {
            get { return _king.DbId; }
            set { _king.DbId = value; }
        }

        public override int X
        {
            get { return _king.X; }
            set { _king.X = value; }
        }

        public override int Y
        {
            get { return _king.Y; }
            set { _king.Y = value; }
        }

        public override string Name
        {
            get { return _king.Name; }
            set { _king.Name = value; }
        }

        public override int Experience
        {
            get { return _king.Experience; }
            set { _king.Experience = value; }
        }

        public override int MilitaryRank
        {
            get { return _king.MilitaryRank; }
            set { _king.MilitaryRank = value; }
        }

        public override int Distance
        {
            get { return _king.Distance; }
            set { _king.Distance = value; }
        }

        public override GameData GameData
        {
            get { return _king.GameData; }
            set { _king.GameData = value; }
        }

        public override bool IsMove
        {
            get { return _king.IsMove; }
            set { _king.IsMove = value; }
        }

        public override int PrevX
        {
            get { return _king.PrevX; }
            set { _king.PrevX = value; }
        }

        public override int PrevY
        {
            get { return _king.PrevY; }
            set { _king.PrevY = value; }
        }

        public override bool Sleep
        {
            get { return _king.Sleep; }
            set { _king.Sleep = value; }
        }

        public override Castle StartCastle
        {
            get { return _king.StartCastle; }
            set { _king.StartCastle = value; }
        }

        public override Castle CurrentCastle
        {
            get { return _king.CurrentCastle; }
        }

        public override KingState State
        {
            get { return _king.State; }
            set { _king.State = value; }
        }

        public override ResourceStore ResourceStore
        {
            get { return _king.ResourceStore; }
            set { _king.ResourceStore = value; }
        }

        public override MapPoint ViewOnMap
        {
            get { return _king.ViewOnMap; }
            set { _king.ViewOnMap = value; }
        }

        public override Map Map
        {
            get { return _king.Map; }
            set { _king.Map = value; }
        }

        public override Player Player
        {
            get { return _king.Player; }
            set { _king.Player = value; }
        }

        public override VisibleSpace VisibleSpace
        {
            get { return _king.VisibleSpace; }
            set { _king.VisibleSpace = value; }
        }

        public override Guid PlayerId
        {
            get { return _king.PlayerId; }
            set { _king.PlayerId = value; }
        }

        public override Guid MapId
        {
            get { return _king.MapId; }
            set { _king.MapId = value; }
        }

        public override Guid? EmpireId
        {
            get { return _king.EmpireId; }
            set { _king.EmpireId = value; }
        }

        public override Guid? UnionId
        {
            get { return _king.UnionId; }
            set { _king.UnionId = value; }
        }

        public override bool IsLeader { get { return true; } }

        public override EntitySet<Castle> Castles
        {
            get { return _king.Castles; }
            set { _king.Castles.Assign(value); }
        }

        public override EntitySet<Mine> Mines
        {
            get { return _king.Mines; }
            set { _king.Mines.Assign(value); }
        }

        public override EntitySet<Unit> Units
        {
            get { return _king.Units; }
            set { _king.Units.Assign(value); }
        }

        #endregion

        public King King
        {
            get { return _king; }
            set { _king = value; }
        }

        public EntitySet<King> Successors
        {
            get { return this._successors; }
            set { this._successors.Assign(value); }
        }
    }
}
