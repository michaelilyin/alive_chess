using AliveChessLibrary.GameObjects.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.Utils
{
    public static class KingSync
    {
        public static void Sync(this King dest, King other)
        {
            dest.Heading = other.Heading;
            dest.PrevX = other.PrevX;
            dest.PrevY = other.PrevY;
            //dest.Speed = other.Speed;
            dest.State = other.State;
            dest.Velocity = other.Velocity;
            dest.X = other.X;
            dest.Y = other.Y;
            dest.Experience = other.Experience;
            dest.MilitaryRank = other.MilitaryRank;
            //if (dest.ResourceStore == null)
            //    dest.ResourceStore = new AliveChessLibrary.GameObjects.Resources.ResourceStore();
            //dest.ResourceStore.SetResources(other.ResourceStore.GetResourceListCopy());
        }
    }
}
