using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class CaptureMineExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
     
        public CaptureMineExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            CaptureMineRequest request = (CaptureMineRequest) msg.Command;

            Map map = msg.Sender.Map;
            Mine mine = map.SearchMineById(request.MineId);

            if (mine != null)
            {
                if ((mine.Player == null) || (mine.King.Id != msg.Sender.King.Id && !mine.King.Sleep))
                {
                    if (mine.Player != null && mine.Player.Id != msg.Sender.Id)
                    {
                        mine.King.Player.Messenger.SendNetworkMessage(new LooseMineMessage(mine.Id));
                        mine.King.RemoveMine(mine);
                    }

                    msg.Sender.King.AddMine(mine);
                    msg.Sender.Messenger.SendNetworkMessage(new CaptureMineResponse(mine));

                    mine.Activate();
                }
            }
            else
            {
                msg.Sender.Messenger.SendNetworkMessage(new ErrorMessage("Mine not found"));
            }
        }
    }
}
