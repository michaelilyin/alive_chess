using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public interface IMultyPoint : IMapObject
    {
        void AddView(MapSector sector);

        MapSector ViewOnMap { get; set; }
    }
}
