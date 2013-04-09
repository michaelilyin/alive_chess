using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Characters
{
    public enum MovableObjectTypes
    {
        King = 0, // король
    }

    public interface IMovable : IObserver
    {
        int PrevX { get; set; }

        int PrevY { get; set; }

        void MoveBy(Position step);

        void OutOfGame();
    }
}
