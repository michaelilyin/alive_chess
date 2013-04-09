using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public class MessageBattleExecutor : IExecutor
    {
        private GameLogic _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageBattleExecutor(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        public void Execute(Message msg)
        {
            MessageExecutorTemplate<BattleDialogMessage> t =
                new MessageExecutorTemplate<BattleDialogMessage>(_logic, _transport);
            t.Proceed(
                delegate
                    {
                    Dialog d = t.Dispute as Dialog;

                    //определяем тему переговоров
                    if (t.Command.State == DialogState.Offer)
                        d.Theme = DialogTheme.Battle;

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
                        //_environment.DisputeRoutine.Remove(t.Sender.Level, t.Dispute);
                        //if (t.Command.State == DialogState.Agree)
                        //{
                        //    Battle battle = _environment.BattleRoutine.CreateBattle(t.Sender.Player.King,
                        //        t.Receiver.Player.King, true);

                        //    _environment.BattleRoutine.Add(battle);

                        //    // меням состояния игроков чтобы никто им не мешал
                        //    t.Receiver.Player.King.State = KingState.Dispute;
                        //    t.Sender.Player.King.State = KingState.Dispute;

                        //    _environment.BattleRoutine.War1.DownloadArmy(battle.Organizator.Player.King.Units,
                        //        battle.Respondent.Player.King.Units);

                        //    battle.PlayerArmy = battle.Organizator.Units.ToList();
                        //    battle.OpponentArmy = battle.Respondent.Units.ToList();

                        //    _queryManager.SendDownloadBattlefild(t.Sender, battle);

                        //    if (!t.Receiver.Bot)
                        //    {
                        //        // отправляем сообщение о нападении королю
                        //        battle.YouStep = false;
                        //        battle.Organizator = t.Receiver.Player.King;
                        //        battle.Respondent = t.Sender.Player.King;
                        //        battle.PlayerArmy = battle.Organizator.Units.ToList();
                        //        battle.OpponentArmy = battle.Respondent.Units.ToList();

                        //        _environment.BattleRoutine.War1.DownloadArmy(battle.Organizator.Player.King.Units,
                        //            battle.Respondent.Player.King.Units);

                        //        _queryManager.SendDownloadBattlefild(t.Receiver, battle);
                        //    }

                        //}


                    }
                }, msg);
        }
    }
}
