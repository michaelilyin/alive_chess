using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Statistics;

namespace AliveChessServer.LogicLayer.AI
{
    public class Evaluator : IEvaluator
    {
        private King _king;
        private Memory _memory;

        public Evaluator(Memory memory)
        {
            this._memory = memory;
        }

        public double RequestPower()
        {
            return _king.Power;
        }

        public double RequestWealth()
        {
            return _king.Wealth;
        }

        public double RequestSuccess()
        {
            return _king.Success;
        }

        public void AttachOwner(King owner)
        {
            this._king = owner;
        }

        public double RequestThreat(King opponent)
        {
            // use memory to remind number of meetings and results
            // fake
            return 0;
        }

        public double RequestProfit(King opponent)
        {
            return 0;
        }

        public double RequestCooperation(King opponent)
        {
            return 0;
        }

        public double RequestSentHelpToAllies(King opponent)
        {
            return opponent.SentHelp;
        }

        public double RequestReceivedHelpFromAllies(King opponent)
        {
            return opponent.ReceivedHelp;
        }

        public double RequestTradingActivity(King opponent)
        {
            return opponent.TradingActivityNumber;
        }

        public double RequestAverageThreat()
        {
            return 10;
        }

        public double RequestAverageProfit()
        {
            return 10;
        }

        public double RequestAverageCooperation()
        {
            return 10;
        }

        public double RequestSentHelpToAllies()
        {
            return 10;
        }

        public double RequestReceivedHelpFromAllies()
        {
            return 10;
        }

        public double RequestTradingActivity()
        {
            return 10;
        }

        public IMemory Memory
        {
            get { return _memory; }
        }
    }
}
