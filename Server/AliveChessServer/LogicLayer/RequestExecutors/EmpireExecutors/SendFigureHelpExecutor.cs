using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class SendFigureHelpExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public SendFigureHelpExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            SendFigureHelpMessage request = (SendFigureHelpMessage) msg.Command;

            King sender = msg.Sender.King;
            King receiver = sender.Map.SearchKingById(request.ReceiverId);

            if (receiver != null)
            {
                // получаем замок из которого нужно послать фигуры
                Castle castle = sender.GetCastleById(request.FromCastle);
                if (castle != null)
                {
                    throw new NotImplementedException();
                    /*// получаем хранилище фигур указанного замка отправителя
                    FigureStore store = castle.UnitStore;
                    foreach (Unit u in request.Units)
                    {
                        // удаляем фигуры из хранилища отправителя
                        // и добавляем их в хранилище начального замка получателя
                        if (store.RemoveFigure(u.UnitType, u.Quantity))
                            receiver.StartCastle.UnitStore.AddFigureToRepository(u);
                    }
                    if (!receiver.Player.Bot)
                        receiver.Player.Messenger.SendNetworkMessage(new GetHelpFigureResponse(request.Units));*/
                }
            }
        }
    }
}
