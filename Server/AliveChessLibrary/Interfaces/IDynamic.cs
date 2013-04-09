using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Interfaces
{
    public delegate void ChangeMapStateHandler<T>(T sender, UpdateWorldEventArgs eventArgs) where T : IMapObject;

    public interface IDynamic<T> where T : IMapObject
    {
        event ChangeMapStateHandler<T> ChangeMapStateEvent;
    }
}
