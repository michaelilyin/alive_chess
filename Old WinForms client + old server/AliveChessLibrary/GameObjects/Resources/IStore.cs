namespace AliveChessLibrary.GameObjects.Resources
{
    public interface IStore
    {
        bool IsFull { get; }

        int MaxCapacity { get; }

        int CurrentCapacity { get; set; }
    }
}
