using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.RequestExecutors.Helpers;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
//using ConversationAILibrary;

namespace AliveChessServer.LogicLayer.RequestExecutors
{
    public class ContactKingExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            ContactKingRequest request = (ContactKingRequest)msg.Command;

            Player organizator = msg.Sender;
            King respondent = organizator.Level.Map.SearchKingById(request.KingId);
            if (respondent != null)
            {
                Level level = organizator.Level as Level;
                // если противник находится на большой карте)
                if (level.DisputeRoutine.CanStartDialog(respondent))
                {
                    // останавливаем королей
                    respondent.ClearSteps();
                    organizator.King.ClearSteps();

                    // начинаем переговоры
                    Dialog dispute = level.DisputeRoutine
                        .CreateDispute(organizator.King, respondent, true);

                    // меням состояния игроков чтобы никто им не мешал
                    respondent.State = KingState.Dispute;
                    organizator.King.State = KingState.Dispute;

                    respondent.Interaction = dispute;
                    organizator.King.Interaction = dispute;

                    // добавляем переговоры
                    level.DisputeRoutine.Add(organizator.Level, dispute);

                    // отправляем сообщение о нападении королю
                    dispute.YouStep = false;
                    dispute.Organizator = respondent;
                    dispute.Respondent = organizator.King;

                    // соперник - живой человек
                    if (!respondent.Player.Bot)
                    {
                        respondent.Player.Messenger.SendNetworkMessage(
                            new ContactKingResponse(dispute));
                    }
                    else
                    {
                        //IStimulus stimulus = new Stimulus();
                        //stimulus.Receiver = respondent;
                        //stimulus.Sender = organizator.King;
                        //respondent.Player.Messenger.SendAIMessage(stimulus);
                    }

                    // формируем ответ
                    dispute.YouStep = true;
                    dispute.Organizator = organizator.King;
                    dispute.Respondent = respondent;

                    // отправляем ответ организатору переговоров
                    if (!respondent.Player.Bot)
                        organizator.Messenger.SendNetworkMessage(new ContactKingResponse(dispute));
                    else
                    {
                        Dialog surrogate = dispute.GetSurrogate();
                        organizator.Messenger.SendNetworkMessage(new ContactKingResponse(surrogate));
                    }
                }
            }
            else
            {
                organizator.Messenger.SendNetworkMessage(new ErrorMessage("King not found"));
            }
        }
    }
}
