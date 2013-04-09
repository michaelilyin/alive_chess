namespace AliveChessLibrary.Interfaces
{
    public interface ILocalizable
    {
        int SizeX
        {
            get;
            set;
        }

        int SizeY
        {
            get;
            set;
        }

        float GetWayCost(int x, int y);
    }
}
