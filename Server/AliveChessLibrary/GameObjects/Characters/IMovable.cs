using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Characters
{
    public enum MovableObjectTypes
    {
        King = 0, // король
    }

    /// <summary>
    /// движущийся объект
    /// </summary>
    public interface IMovable : IObserver
    {
        /// <summary>
        /// предыдущая координата
        /// </summary>
        int PrevX { get; set; }

        /// <summary>
        /// предыдущая координата
        /// </summary>
        int PrevY { get; set; }

        /// <summary>
        /// переместить
        /// </summary>
        /// <param name="step"></param>
        void MoveBy(Position step);

        /// <summary>
        /// объект вне игры
        /// </summary>
        void OutOfGame();
    }
}
