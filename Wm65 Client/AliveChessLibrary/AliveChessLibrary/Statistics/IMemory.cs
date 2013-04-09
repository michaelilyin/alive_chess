using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Statistics
{
    public interface IMemory
    {
        int GetWinNumber(King opponent);

        int GetLooseNumber(King opponent);

        int GetCommonBattlesNumber(King opponent);
    }
}
