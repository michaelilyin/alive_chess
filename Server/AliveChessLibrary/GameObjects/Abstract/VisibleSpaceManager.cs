using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public class VisibleSpaceManager
    {
        private GameData _context;

        public VisibleSpaceManager(GameData context)
        {
            this._context = context;
        }

        /// <summary>
        /// получение области видимости для игрока
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public VisibleSpace GetVisibleSpace(King player, bool isSuperUser)
        {
            VisibleSpace vs = new VisibleSpace(player.Map);

            if (!isSuperUser)
            {
                vs.Add(GetVisibleSpace(player));
            }
            else // получение всей карты
            {
                for (int i = 0; i <= player.Map.SizeX; i++)
                {
                    for (int j = 0; j <= player.Map.SizeY; j++)
                    {
                        if (player.Map.Locate(i, j))
                        {
                            vs.Add(player.Map[i, j]);
                        }
                    }
                }
            }

            return vs;
        }

        public VisibleSpace GetVisibleSpace(IObserver observer)
        {
            VisibleSpace vs = new VisibleSpace(observer.Map);

            Map map = observer.Map;
            int startX = observer.X - observer.Distance;
            int startY = observer.Y - observer.Distance;

            int SizeX = startX + 2 * observer.Distance;
            int SizeY = startY + 2 * observer.Distance;

            for (int i = startX; i <= SizeX; i++)
            {
                for (int j = startY; j <= SizeY; j++)
                {
                    if (map.Locate(i, j))
                    {
                        vs.Add(map[i, j]);
                    }
                }
            }

            return vs;
        }

        public List<MapPoint> GetObjectsInVisibleSpace(VisibleSpace sector, 
            bool isSuperUser)
        {
            List<MapPoint> objects = new List<MapPoint>();
            List<MapPoint> _sector = sector.Sector;
            foreach (MapPoint mo in _sector)
            {
                if (mo.MapPointType == PointTypes.King || mo.MapPointType == PointTypes.Resource)
                    objects.Add(mo);
            }
            return objects;
        }

        public List<MapSector> GetSectorsInVisibleSpace(VisibleSpace sector, 
            bool isSuperUser)
        {
            List<MapSector> objects = new List<MapSector>();
            List<MapPoint> _sector = sector.Sector;
            foreach (MapPoint mo in _sector)
            {
                if ((mo.MapPointType == PointTypes.Castle || mo.MapPointType == PointTypes.Mine) 
                    && !objects.Contains(mo.MapSector))
                    objects.Add(mo.MapSector);
            }
            return objects;
        }
    }
}
