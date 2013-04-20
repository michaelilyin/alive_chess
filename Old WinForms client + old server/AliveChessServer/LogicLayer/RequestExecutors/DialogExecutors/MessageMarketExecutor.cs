using System.Diagnostics;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public class MessageMarketExecutor : IExecutor
    {
        private LogicEntryPoint _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageMarketExecutor(LogicEntryPoint gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        public void Execute(Message msg)
        {
            MessageExecutorTemplate<MarketDialogMessage> t =
               new MessageExecutorTemplate<MarketDialogMessage>(_logic, _transport);
            t.Proceed(
                delegate()
                {
                    Dialog d = t.Dispute as Dialog;

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

                    // определяем тему переговоров
                    if (t.Command.State == DialogState.Offer)
                        d.Theme = DialogTheme.Trade;

                    // прекращаем переговоры
                    if (t.Command.State == DialogState.Agree || t.Command.State == DialogState.Refuse)
                    {
                        t.KingSender.Interaction = null;
                        t.KingReceiver.Interaction = null;

                        t.Level.DisputeRoutine.Remove(t.Sender.Level, t.Dispute);
                    }
                }
                , msg);
        }
    }
}
