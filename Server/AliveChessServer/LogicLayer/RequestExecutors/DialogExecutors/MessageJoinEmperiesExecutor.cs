using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public class MessageJoinEmperiesExecutor : IExecutor
    {
        private GameLogic _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageJoinEmperiesExecutor(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        public void Execute(Message msg)
        {
            MessageExecutorTemplate<JoinEmperiesDialogMessage> t =
               new MessageExecutorTemplate<JoinEmperiesDialogMessage>(_logic, _transport);
            t.Proceed(
                delegate
                    {
                    Negotiate d = t.Dispute as Negotiate;

                    // определяем тему переговоров
                    if (t.Command.State == DialogState.Offer)
                        d.Theme = NegotiateTheme.Join;

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

                        if (t.Command.State == DialogState.Agree)
                        {
                            // объединение империй
                            Empire e1 = t.Level.EmpireManager.GetAlianceByMember(t.KingSender) as Empire;
                            Empire e2 = t.Level.EmpireManager.GetAlianceByMember(t.KingReceiver) as Empire;

                            t.Level.EmpireManager.JoinEmperies(e1, e2);
                        }
                    }
                }, msg);
        }
    }
}
