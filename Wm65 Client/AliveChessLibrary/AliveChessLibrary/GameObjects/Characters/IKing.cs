using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.Statistics;
using GeneralSysLibrary;

namespace AliveChessLibrary.GameObjects.Characters
{
    public interface IKing : IMovable
    {
        void Update();

        void LeaveCastle();

        Castle SearchCastle();

        void ComeInCastle(Castle castle);

        bool InsideCastle(Castle castle);

        void AttachStartCastle(Castle castle);

        string Name { get; set; }

        int Experience { get; set; }

        int MilitaryRank { get; set; }

        IPlayer Player { get; set; }

        IEvaluator Evaluator { get; set; }

        ICommunity Community { get; }
    }
}
