using System.Diagnostics;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessServer.LogicLayer.RequestExecutors.Auxilary;

namespace AliveChessServer.LogicLayer.RequestExecutors
{
    public class ContactKingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public ContactKingExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            ContactKingRequest request = (ContactKingRequest)msg.Command;

            Player p_Organizator = msg.Sender;
            King k_Respondent = p_Organizator.Level.Map.SearchKingByPointId(request.OpponentId);

            Level level = p_Organizator.Level as Level;
            // если противник находится на большой карте)
            if (level.DisputeRoutine.CanStartDialog(k_Respondent))
            {
                // останавливаем королей
                k_Respondent.ClearSteps();
                p_Organizator.King.ClearSteps();

                // начинаем переговоры
                Dialog dispute = level.DisputeRoutine
                    .CreateDispute(p_Organizator.King, k_Respondent, true);

                // меням состояния игроков чтобы никто им не мешал
                k_Respondent.State = KingState.Dispute;
                p_Organizator.King.State = KingState.Dispute;

                k_Respondent.Interaction = dispute;
                p_Organizator.King.Interaction = dispute;

                // добавляем переговоры
                level.DisputeRoutine.Add(p_Organizator.Level, dispute);

                // соперник - живой человек
                if (!k_Respondent.Player.Bot)
                {
                    // отправляем сообщение о нападении королю
                    dispute.YouStep = false;
                    dispute.Organizator = k_Respondent;
                    dispute.Respondent = p_Organizator.King;

                    k_Respondent.Player.Messenger.SendNetworkMessage(new ContactKingResponse(dispute));
                }
                else
                {
                    // искусственный интеллект
                }

                // формируем ответ
                dispute.YouStep = true;
                dispute.Organizator = p_Organizator.King;
                dispute.Respondent = k_Respondent;

                // отправляем ответ организатору переговоров
                if (!k_Respondent.Player.Bot)
                    p_Organizator.Messenger.SendNetworkMessage(new ContactKingResponse(dispute));
                else
                {
                    Dialog surrogate = dispute.GetSurrogate();
                    p_Organizator.Messenger.SendNetworkMessage(new ContactKingResponse(surrogate));
                }
            }
        }
    }
}
