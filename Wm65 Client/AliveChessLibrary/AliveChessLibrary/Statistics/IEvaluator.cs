using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Statistics
{
    public interface IEvaluator
    {
        IMemory Memory { get; }

        double RequestPower();

        double RequestWealth();

        double RequestSuccess();

        void AttachOwner(King king);

        double RequestThreat(King opponent);

        double RequestProfit(King opponent);

        double RequestCooperation(King opponent);

        double RequestSentHelpToAllies(King opponent);

        double RequestReceivedHelpFromAllies(King opponent);

        double RequestTradingActivity(King opponent);

        double RequestAverageThreat();

        double RequestAverageProfit();

        double RequestAverageCooperation();

        double RequestSentHelpToAllies();

        double RequestReceivedHelpFromAllies();

        double RequestTradingActivity();
    }
}
