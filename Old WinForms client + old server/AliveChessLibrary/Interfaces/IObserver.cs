using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public interface IObserver : IMapObject
    {
        int X { get; set; }

        int Y { get; set; }

        int Distance { get; set; }

        VisibleSpace VisibleSpace { get; set; }

        void UpdateVisibleSpace(VisibleSpace sector);
    }
}
