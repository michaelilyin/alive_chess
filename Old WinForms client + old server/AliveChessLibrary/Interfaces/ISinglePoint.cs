using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public interface ISinglePoint : IMapObject
    {
        void AddView(MapPoint point);

        MapPoint ViewOnMap { get; set; }
    }
}
