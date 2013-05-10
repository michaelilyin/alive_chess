using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class CaptureMineRequestExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            CaptureMineResponse response = (CaptureMineResponse) command;
            GameCore.Instance.World.Map.SearchMineById(response.Mine.Id).KingId = response.Mine.KingId;
            GameCore.Instance.BigMapCommandController.BuildingsModified = true;
        }
    }
}
