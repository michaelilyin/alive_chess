using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public interface IObserver : IMapObject
    {
        int Distance { get; set; }

        VisibleSpace VisibleSpace { get; set; }
    }
}
