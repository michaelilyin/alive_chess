using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class ComeInCastleExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            ComeInCastleResponse response = (ComeInCastleResponse)command;
            Castle castle = GameCore.Instance.World.Map.SearchCastleById(response.CastleId);
            if (castle != null)
            {
                GameCore.Instance.Player.King.ComeInCastle(castle);
                GameCore.Instance.BigMapCommandController.KingInCastle = true;
                GameCore.Instance.CastleCommandController.KingOnMap = false;
            }
        }

        #endregion
    }
}
