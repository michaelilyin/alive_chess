using System.Diagnostics;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public class MessageWarExecutor : IExecutor
    {
        private LogicEntryPoint _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageWarExecutor(LogicEntryPoint gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        public void Execute(Message msg)
        {
            MessageExecutorTemplate<WarDialogMessage> t =
               new MessageExecutorTemplate<WarDialogMessage>(_logic, _transport);
            t.Proceed(
                delegate()
                {
                    Negotiate d = t.Dispute as Negotiate;

                    // определяем тему переговоров
                    if (t.Command.State == DialogState.Coerce)
                    {
                        d.Theme = NegotiateTheme.War;
                        // начало войны
                    }

                    // если соперник не бот
                    if (!t.Receiver.Bot)
                    {
                        //t.Sender.Player.King.Interaction = null;
                        //t.Receiver.Player.King.Interaction = null;

                        // отправляем ответ
                        t.Receiver.Messenger.SendNetworkMessage(t.Command);
                    }
                    else
                    {
                        // бот
                    }

                }, msg);
        }
    }
}
