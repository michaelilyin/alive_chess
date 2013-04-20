using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.AI.PerseptionLayer
{
    public class GameSetting
    {
        private King _player;

        private King _nearestEnemyKing;
        private Castle _nearestEnemyCastle;
        private Castle _nearestPlayerCastle;
        private Resource _nearestResource;

        private int _unitsCountInsideEnemyCastle;
        private int _unitsCountTogetherWithEnemyKing;
        private int _resourceCountInVisibleArea;
        private int _resourceCountOnHand;

        private double _minDistanceToEnemyCastle;
        private double _minDistanceToEnemyKing;
        private double _minDistanceToPlayerCastle;
        private double _minDistanceToResource;
       
        private PropertyInfo _minResourceDistProp;
        private PropertyInfo _minEnemyKingDistProp;
        private PropertyInfo _minEnemyCastleDistProp;
        private PropertyInfo _minPlayerCastleDistProp;

        public GameSetting(King player)
        {
            this._player = player;
    
            this._minResourceDistProp = typeof (GameSetting).GetProperty("MinDistanceToResource");
            this._minEnemyKingDistProp = typeof(GameSetting).GetProperty("MinDistanceToEnemyKing");
            this._minEnemyCastleDistProp = typeof(GameSetting).GetProperty("MinDistanceToEnemyCastle");
            this._minPlayerCastleDistProp = typeof(GameSetting).GetProperty("MinDistanceToPlayerCastle");
        }
        /*
         * Obtain information about game setting. It's involves calculating
         * minimum distances for kings and castles, revealing weak and strong kings,
         * getting information about resources in visible space
         */
        public void CollectInfo(
            IList<King> kings, IList<Castle> castles,
            IList<Mine> mines, IList<Resource> resources)
        {
            _minDistanceToEnemyKing = -1;
            _minDistanceToPlayerCastle = -1;
            _minDistanceToEnemyCastle = -1;

            this._nearestEnemyKing = GetEnemyKing(kings);
            this._nearestEnemyCastle = GetEnemyCastle(castles);
            this._nearestPlayerCastle = GetPlayerCastle(castles);
            this._nearestResource = GetResource(resources);

            // get information about force of nearest enemy king
            UnitsCountTogetherWithEnemyKing =
               NearestEnemyKing != null ? GetUnitCountTogetherWithKing(NearestEnemyKing) : 0;

            // get information about force of nearest enemy castle
            UnitsCountInsideEnemyCastle = 
                NearestEnemyCastle != null ? GetUnitCountInsideCastle(NearestEnemyCastle) : 0;
           
            ResourceCountOnHand = GetResourceCountOnHand();

            ResourceCountInVisibleArea = GetResourceCountInVisibleArea();
        }

        private King GetEnemyKing(IList<King> targets)
        {
            return GetMinDistance(targets, x => x.Id != _player.Id, 
                _minEnemyKingDistProp);
        }

        private Castle GetEnemyCastle(IList<Castle> targets)
        {
            return GetMinDistance(targets, x => !x.IsBelongTo(_player), 
                _minEnemyCastleDistProp);
        }

        private Castle GetPlayerCastle(IList<Castle> targets)
        {
            return GetMinDistance(targets, x => x.IsBelongTo(_player), 
                _minPlayerCastleDistProp);
        }

        private Resource GetResource(IList<Resource> targets)
        {
            return GetMinDistance(targets, _minResourceDistProp);
        }

        private T GetMinDistance<T>(IList<T> targets, PropertyInfo prop)
            where T : ILocation
        {
            double distance = -1.0;
            T obj = default(T);
            if (targets.Count == 0)
                return default(T);
            T target = targets[0];
            double dist = int.MaxValue;
            obj = target;
            foreach (T item in targets)
            {
                double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                if (tmp < dist)
                {
                    obj = item;
                    distance = tmp;
                }
            }
            prop.SetValue(this, distance, null);
            return obj;
        }

        private T GetMinDistance<T>(IList<T> targets, Func<T, bool> checker, 
            PropertyInfo prop) where T : ILocation
        {
            double distance = -1.0;
            if (targets.Count == 0)
                return default(T);
            T target = default(T);
            double dist = int.MaxValue;
            foreach (T item in targets)
            {
                if (checker(item))
                {
                    double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                    if (tmp < dist)
                    {
                        target = item;
                        distance = tmp;
                    }
                }
            }
            prop.SetValue(this, distance, null);
            return target;
        }

        private int GetUnitCountInsideCastle(Castle castle)
        {
            return 1;
        }

        private int GetUnitCountTogetherWithKing(King enemy)
        {
            return enemy.GetUnitCountFAKE();
        }

        private int GetResourceCountOnHand()
        {
            return 1;
        }

        private int GetResourceCountInVisibleArea()
        {
            return 1;
        }

        public double MinDistanceToEnemyCastle
        {
            get { return _minDistanceToEnemyCastle; }
            set { _minDistanceToEnemyCastle = value; }
        }

        public double MinDistanceToEnemyKing
        {
            get { return _minDistanceToEnemyKing; }
            set { _minDistanceToEnemyKing = value; }
        }

        public double MinDistanceToPlayerCastle
        {
            get { return _minDistanceToPlayerCastle; }
            set { _minDistanceToPlayerCastle = value; }
        }

        public double MinDistanceToResource
        {
            get { return _minDistanceToResource; }
            set { _minDistanceToResource = value; }
        }

        public int UnitsCountInsideEnemyCastle
        {
            get { return _unitsCountInsideEnemyCastle; }
            set { _unitsCountInsideEnemyCastle = value; }
        }

        public int UnitsCountTogetherWithEnemyKing
        {
            get { return _unitsCountTogetherWithEnemyKing; }
            set { _unitsCountTogetherWithEnemyKing = value; }
        }

        public int ResourceCountInVisibleArea
        {
            get { return _resourceCountInVisibleArea; }
            set { _resourceCountInVisibleArea = value; }
        }

        public int ResourceCountOnHand
        {
            get { return _resourceCountOnHand; }
            set { _resourceCountOnHand = value; }
        }

        public King NearestEnemyKing
        {
            get { return _nearestEnemyKing; }
        }

        public Castle NearestEnemyCastle
        {
            get { return _nearestEnemyCastle; }
        }

        public Castle NearestPlayerCastle
        {
            get { return _nearestPlayerCastle; }
        }

        public Resource NearestResource
        {
            get { return _nearestResource; }
        }
    }
}
