using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public class MessageCastleExecutor : IExecutor
    {
        private GameLogic _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageCastleExecutor(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        #region IExecutor Members

        public void Execute(Message msg)
        {
            MessageExecutorTemplate<CaptureCastleDialogMessage> t =
                new MessageExecutorTemplate<CaptureCastleDialogMessage>(_logic, _transport);
            t.Proceed(
                delegate
                    {
                    Dialog d = t.Dispute as Dialog;

                    // определяем тему переговоров
                    if (t.Command.State == DialogState.Offer)
                        d.Theme = DialogTheme.CaptureCastle;

                    // если соперник не бот
                    if (!t.Receiver.Bot)
                    {
                        // отправляем ответ
                        t.Receiver.Messenger.SendNetworkMessage(t.Command);
                    }
                    else
                    {
                        // бот
                    }

                    // прекращаем переговоры
                    if (t.Command.State == DialogState.Agree || t.Command.State == DialogState.Refuse)
                    {
                        t.KingSender.Interaction = null;
                        t.KingReceiver.Interaction = null;

                        t.Level.DisputeRoutine.Remove(t.Sender.Level, t.Dispute);

                        //Map map = t.Sender.Player.Map;
                        //King king = t.Sender.Player.King;
                        //uint castleId = map[king.X, king.Y].Id;
                        //Castle castle = map.SearchCastle(castleId);

                        //if (castle.Player != null)
                        //{
                        //    Player p = castle.Player;
                        //    p.King.RemoveCastle(castle);

                        //    if (!t.Receiver.Bot)
                        //        _queryManager.SendLooseCastle(t.Receiver, castle);
                        //}

                        //king.Player.King.AddCastle(castle);
                        ////castle.King = king;
                        //if (!t.Receiver.Bot)
                        //    _queryManager.SendCaptureCastle(t.Sender, castle);
                    }
                }
                , msg);
        }

        #endregion
    }
}
