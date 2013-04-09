using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Statistics;

namespace AliveChess.GameLayer.LogicLayer
{
    public class Player : IPlayer
    {
        private bool _isAuthorized;

        public void AddKing(King king)
        {
            throw new NotImplementedException();
        }

        public void RemoveKing(King king)
        {
            throw new NotImplementedException();
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

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { _isAuthorized = value; }
        }
    }
}
