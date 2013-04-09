using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.RequestExecutors.Helpers;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class ContactCastleExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            ContactCastleRequest request = (ContactCastleRequest) msg.Command;

            // получаем контекст игрока
            Player organizator = msg.Sender;
            Level level = organizator.Level as Level;
            // получаем замок игрока
            Castle castle = organizator.Map.SearchCastleById(request.CastleId);
            if (castle != null)
            {
                // замок оказался пуст. Сдаем без боя
                if (castle.Player == null)
                {
                    // формируем ответ
                    if (!organizator.Bot)
                        organizator.Messenger.SendNetworkMessage(new ContactCastleResponse(castle, null));
                    else
                    {
                        //
                    }
                }
                else // замок занят. Придется договариваться либо сражаться...
                {
                    // получаем контекст соперника
                    King respondent = castle.King;

                    // соперник ходит по карте и не занят ничем другим а также на него возможно
                    // напасть
                    if (level.DisputeRoutine.CanStartDialog(respondent))
                    {
                        // начинаем переговоры
                        Dialog dispute = level.DisputeRoutine
                            .CreateDispute(organizator.King, respondent, true);

                        // добавляем переговоры
                        level.DisputeRoutine.Add(organizator.Level, dispute);

                        // меняем состояния игроков чтобы никто им не мешал
                        respondent.State = KingState.Dispute;
                        organizator.King.State = KingState.Dispute;

                        respondent.Interaction = dispute;
                        organizator.King.Interaction = dispute;

                        dispute.YouStep = false;
                        dispute.Organizator = respondent;
                        dispute.Respondent = organizator.King;

                        // соперник - живой человек
                        if (!respondent.Player.Bot)
                        {
                            // отправляем ответ респонденту переговоров
                            respondent.Player.Messenger.SendNetworkMessage(new ContactCastleResponse(null, dispute));
                        }
                        else
                        {
                            // Send message to AI
                        }

                        // формируем ответ
                        dispute.YouStep = true;
                        dispute.Organizator = organizator.King;
                        dispute.Respondent = respondent;

                        // отправляем ответ организатору переговоров
                        if (!respondent.Player.Bot)
                            organizator.Messenger.SendNetworkMessage(new ContactCastleResponse(castle, dispute));
                        else
                        {
                            Dialog surrogate = dispute.GetSurrogate();
                            organizator.Messenger.SendNetworkMessage(new ContactCastleResponse(castle, surrogate));
                        }
                    }
                }
            }
            else
            {
                organizator.Messenger.SendNetworkMessage(new ErrorMessage("Castle not found"));
            }
        }
    }
}
