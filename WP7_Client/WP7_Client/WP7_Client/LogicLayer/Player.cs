using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Statistics;

namespace WP7_Client.LogicLayer
{
    public class Player : IPlayer
    {
        public Player()
        {
            Kings = new List<King>();
        }

        public List<King> Kings { get; private set; }
        public void AddKing(King king)
        {
            if (!Kings.Contains(king))
                Kings.Add(king);
        }

        public void RemoveKing(King king)
        {
            Kings.Remove(king);
        }

        public void AddVisibleSector(IVisibleSpace space)
        {
            throw new NotImplementedException();
        }

        public void RemoveVisibleSector(IVisibleSpace space)
        {
            throw new NotImplementedException();
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public Map Map
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Bot
        {
            get { throw new NotImplementedException(); }
        }

        public bool Ready
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int LevelId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ILevel Level
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSuperUser
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IMessenger Messenger
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Statistic Statistics
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IVisibleSpace VisibleSpace
        {
            get { throw new NotImplementedException(); }
        }

        public ICommunity Community
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsAuthorized { get; set; }
    }
}
