using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel
{
    public class Player
    {
        private King _king;
        public King King
        {
            get
            {
                return _king;
            }
            internal set
            {
                _king = value;
            }
        }
        public bool KingInKastle { get; set; }

        public Dictionary<ResourceTypes, int> ResourcesCache;

        public Player()
        {
            ResourcesCache = new Dictionary<ResourceTypes, int>();
            KingInKastle = false;
        }

        internal void SetResources(List<AliveChessLibrary.GameObjects.Resources.Resource> list)
        {
            _king.ResourceStore.SetResources(list);
            foreach (var res in list)
            {
                ResourcesCache[res.ResourceType] = res.Quantity;
            }
        }
    }
}
