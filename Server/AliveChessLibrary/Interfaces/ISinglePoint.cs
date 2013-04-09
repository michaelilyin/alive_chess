using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public interface ISinglePoint : IMapObject
    {
        void AddView(MapPoint point);

        void RemoveView();

        MapPoint ViewOnMap { get; set; }
    }
}
