using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace BehaviorAILibrary.PerseptionLayer
{
    public class PoolingManager
    {
        private King _player;

        private King _enemyKing;
        private Castle _enemyCastle;
        private Castle _playerCastle;
        private Resource _resource;

        public PoolingManager(King player)
        {
            this._player = player;
        }
       
        public void Pool(PoolingStation station)
        {
            Initialize();
            PoolEnemyKings(station.Kings);
            PoolEnemyCastles(station.Castles);
            PoolPlayerCastles(station.Castles);
            PoolResources(station.Resources);

            // get information about force of nearest enemy king
            UnitsCountTogetherWithEnemyKing =
               NearestEnemyKing != null ? GetUnitCountTogetherWithKing(NearestEnemyKing) : 0;

            // get information about force of nearest enemy castle
            UnitsCountInsideEnemyCastle = 
                NearestEnemyCastle != null ? GetUnitCountInsideCastle(NearestEnemyCastle) : 0;
           
            ResourceCountOnHand = GetResourceCountOnHand();

            ResourceCountInVisibleArea = GetResourceCountInVisibleArea();
        }

        private void Initialize()
        {
            MinDistanceToEnemyKing = -1;
            MinDistanceToEnemyCastle = -1;
            MinDistanceToPlayerCastle = -1;
        }

        private void PoolEnemyKings(IList<King> targets)
        {
            King obj = null;
            double distance = int.MaxValue;
            if (targets.Count > 0)
            {
                King target = targets[0];
                obj = target;
                foreach (King item in targets)
                {
                    if (item != _player)
                    {
                        double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                        if (tmp < distance)
                        {
                            obj = item;
                            distance = tmp;
                        }
                    }
                }

                this._enemyKing = obj;
            }
            this.MinDistanceToEnemyKing = distance == int.MaxValue ? -1 : distance;
        }
        private void PoolEnemyCastles(IList<Castle> targets)
        {
            Castle obj = null;
            double distance = int.MaxValue;
            if (targets.Count > 0)
            {
                Castle target = targets[0];
                obj = target;
                foreach (Castle item in targets)
                {
                    if (!item.BelongsTo(_player))
                    {
                        double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                        if (tmp < distance)
                        {
                            obj = item;
                            distance = tmp;
                        }
                    }
                }

                this._enemyCastle = obj;
            }
            this.MinDistanceToEnemyCastle = distance == int.MaxValue ? -1 : distance;
        }
        private void PoolPlayerCastles(IList<Castle> targets)
        {
            Castle obj = null;
            double distance = int.MaxValue;
            if (targets.Count > 0)
            {
                Castle target = targets[0];
                obj = target;
                foreach (Castle item in targets)
                {
                    if (item.BelongsTo(_player))
                    {
                        double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                        if (tmp < distance)
                        {
                            obj = item;
                            distance = tmp;
                        }
                    }
                }

                this._playerCastle = obj;
            }
            this.MinDistanceToPlayerCastle = distance == int.MaxValue ? -1 : distance;
        }
        private void PoolResources(IList<Resource> targets)
        {
            Resource obj = null;
            double distance = int.MaxValue;
            if (targets.Count > 0)
            {
                Resource target = targets[0];
                obj = target;
                foreach (Resource item in targets)
                {
                    double tmp = Math.Sqrt(Math.Pow(_player.X - item.X, 2) + Math.Pow(_player.Y - item.Y, 2));
                    if (tmp < distance)
                    {
                        obj = item;
                        distance = tmp;
                    }
                }

                this._resource = obj;
            }
            this.MinDistanceToResource = distance == int.MaxValue ? -1 : distance;
        }

        private int GetUnitCountInsideCastle(Castle castle)
        {
            return 1;
        }

        private int GetUnitCountTogetherWithKing(King enemy)
        {
            return enemy.Army.GetStrength();
        }

        private int GetResourceCountOnHand()
        {
            return 1;
        }

        private int GetResourceCountInVisibleArea()
        {
            return 1;
        }

        public double MinDistanceToEnemyCastle { get; set; }
        public double MinDistanceToEnemyKing { get; set; }
        public double MinDistanceToPlayerCastle { get; set; }
        public double MinDistanceToResource { get; set; }
        public int UnitsCountInsideEnemyCastle { get; set; }
        public int UnitsCountTogetherWithEnemyKing { get; set; }
        public int ResourceCountInVisibleArea{get; set;}
        public int ResourceCountOnHand { get; set; }
        public King NearestEnemyKing { get { return _enemyKing; } }
        public Castle NearestEnemyCastle { get { return _enemyCastle; } }
        public Castle NearestPlayerCastle { get { return _playerCastle; } }
        public Resource NearestResource { get { return _resource; } }
    }
}
