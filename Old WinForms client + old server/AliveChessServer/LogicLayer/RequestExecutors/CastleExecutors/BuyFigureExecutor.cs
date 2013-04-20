using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class BuyFigureExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public BuyFigureExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            //BuyFigureRequest request = (BuyFigureRequest)cmd.Command;
            //PlayerInfo plInfo = _playerManager.GetPlayerInfoById(cmd.Sender.Id);
            //King king = cmd.Sender.King;
            //king.CurrentCastle.CreateUnitAndAddInArmy(delegate() { return GuidGenerator.Instance.GeneratePair(); },
            //    request.FigureCount, request.FigureType);
            //List<Unit> arm = plInfo.Player.King.CurrentCastle.ArmyInsideCastle;
            //_queryManager.SendGetArmyCastle(plInfo, arm);

        }
    }
}
