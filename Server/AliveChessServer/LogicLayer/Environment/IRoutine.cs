namespace AliveChessServer.LogicLayer.Environment
{
    public interface IRoutine
    {
        void Update();
    }

    public interface ITimeRoutine
    {
        void Update(GameTime time);
    }
}
