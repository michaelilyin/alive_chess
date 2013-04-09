using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Statistics;

namespace AliveChessServer.LogicLayer.AI
{
    public class Memory : IMemory
    {
        public int GetWinNumber(King opponent)
        {
            return opponent.WinNumber;
        }

        public int GetLooseNumber(King opponent)
        {
            return opponent.LooseNumber;
        }

        public int GetCommonBattlesNumber(King opponent)
        {
            return opponent.CommonBattlesNumber;
        }
    }
}
