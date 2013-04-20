using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class StartNegotiateExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;
       
        public StartNegotiateExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = _environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            StartNegotiateRequest request = (StartNegotiateRequest)msg.Command;

            Player p_Organizator = msg.Sender;
            Level level = _levelManager.GetLevelById(p_Organizator.LevelId);
            King k_Respondent = p_Organizator.Level.Map.SearchKingById(request.OpponentId);
            IAliance aliance = level.EmpireManager.GetAlianceByMember(k_Respondent);

            // если противник находится на большой карте
            if (level.DisputeRoutine.CanStartDialog(k_Respondent))
            {
                // останавливаем королей
                k_Respondent.ClearSteps();
                p_Organizator.King.ClearSteps();

                Leader leaderRespondent = null;
                Leader leaderOrganizator = p_Organizator.King as Leader;

                if (aliance.Status == AlianceStatus.Empire)
                {
                    leaderRespondent = (aliance as Empire).Leader;

                    // начинаем переговоры
                    Negotiate negotiate = level.DisputeRoutine
                        .CreateNegotiate(leaderOrganizator.King, leaderRespondent.King, true);

                    // меням состояния игроков чтобы никто им не мешал
                    k_Respondent.State = KingState.Negotiate;
                    p_Organizator.King.State = KingState.Negotiate;

                    k_Respondent.Interaction = negotiate;
                    p_Organizator.King.Interaction = negotiate;

                    // добавляем переговоры
                    level.DisputeRoutine.Add(p_Organizator.Level, negotiate);

                    // отправляем сообщение о нападении королю
                    negotiate.YouStep = false;
                    negotiate.Organizator = leaderRespondent.King;
                    negotiate.Respondent = leaderOrganizator.King;

                    // соперник - живой человек
                    if (!k_Respondent.Player.Bot)
                    {
                        // отправляем ответ респонденту переговоров
                        k_Respondent.Player.Messenger.SendNetworkMessage(new StartNegotiateResponse(negotiate));
                    }
                    else
                    {
                        // искусственный интеллект
                    }

                    // формируем ответ
                    negotiate.YouStep = true;
                    negotiate.Respondent = leaderRespondent.King;
                    negotiate.Organizator = leaderOrganizator.King;

                    // отправляем ответ организатору переговоров
                    if (!k_Respondent.Player.Bot)
                        p_Organizator.Messenger.SendNetworkMessage(new StartNegotiateResponse(negotiate));
                    else
                    {
                        // Send surrogate
                    }
                }
            }
        }
    }
}
