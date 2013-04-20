using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessLibrary.GameObjects.Characters
{
    public interface IKing : IMovable
    {
        Castle SearchCastle();

        void ComeInCastle(Castle castle);

        void LeaveCastle();

        void Update();

        string Name { get; set; }

        int Experience { get; set; }

        int MilitaryRank { get; set; }

        IPlayer Player { get; set; }
    }
}
