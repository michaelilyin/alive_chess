using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class AllianceRoutine : ITimeRoutine
    {
        private List<IAlliance> _aliances;
        private PlayerManager _playerManager;
        private List<UnitedAlliance> _unitedAliances;
        private List<War> _wars;
        private TimeManager _timeManager;
     
        private readonly object _aliancesSync = new object();

        public AllianceRoutine(TimeManager timeManager)
        {
            _timeManager = timeManager;
            _aliances = new List<IAlliance>();
        }

        /// <summary>
        /// жизненный цикл союзов и империй
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            if (time.Elapsed > TimeSpan.FromMilliseconds(10))
            {
                lock (_aliancesSync)
                {
                    foreach (IAlliance aliance in Next())
                        aliance.DoLogic(time);
                }

                time.SavePreviousTimestamp();
            }
        }

        /// <summary>
        /// добавление альянса
        /// </summary>
        /// <param name="aliance"></param>
        public void Add(Empire aliance)
        {
            lock (_aliancesSync)
                _aliances.Add(aliance);
            aliance.DowngradeEvent +=
                DowngradeEmpireToUnion;
            aliance.VoteFinishEvent += OnVoteFinished;
            aliance.ImpeachmentFinishedEvent += OnImpeachmentFinished;
            aliance.TaxEvent += OnTakeResource;
        }

        public void Add(Union aliance)
        {
            lock (_aliancesSync)
                _aliances.Add(aliance);
            aliance.VoteFinishEvent += OnVoteFinished;
        }

        /// <summary>
        /// удаление альянса
        /// </summary>
        /// <param name="union"></param>
        public void Remove(IAlliance aliance)
        {
            lock (_aliancesSync)
                _aliances.Remove(aliance);
        }

        /// <summary>
        /// трансформация союза в империю
        /// </summary>
        /// <param name="union"></param>
        public Empire UpgradeUnionToEmpire(Union union)
        {
            lock (_aliancesSync)
            {
                int index = _aliances.FindIndex(x => x.Id == union.Id);
                Empire empire = new Empire(union);
                this._aliances[index] = empire;
                this._aliances[index].PublishNews(NewsType.ChangeAlianceStatus, 
                    "Union was upgraded to empire");

                union.Level.RemoveUnion(union);
                union.Level.AddEmpire(empire);

                return empire;
            }
        }

        /// <summary>
        /// трансформация империи в союз
        /// </summary>
        /// <param name="empire"></param>
        public void DowngradeEmpireToUnion(Empire empire)
        {
            lock (_aliancesSync)
            {
                int index = _aliances.FindIndex(x => x.Id == empire.Id);
                this._aliances[index] = empire.Union;
                this._aliances[index].PublishNews(NewsType.ChangeAlianceStatus, 
                    "Empire was downgraded to union");

                empire.Level.RemoveEmpire(empire);
                empire.Level.AddUnion(empire.Union);
            }
        }

        public void JoinEmperies(params Empire[] emperies)
        {
            UnitedAlliance unitedAliance = new UnitedAlliance();
            for (int i = 0; i < emperies.Length; i++)
                unitedAliance.AddEmperie(emperies[i]);
        }

        public void StartWar(params Empire[] emperies)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IAlliance> Next()
        {
            lock (_aliances)
            {
                for (int i = 0; i < _aliances.Count; i++)
                    yield return _aliances[i];
            }
        }

        /// <summary>
        /// обработчик события на завершение голосования для союза
        /// </summary>
        /// <param name="union"></param>
        public void OnVoteFinished(Union union)
        {
            union.BallotBox.Calculate();
            if (union.BallotBox.IsCandidateVictory)
            {
                Empire empire = UpgradeUnionToEmpire(union);

                empire.VoteFinishEvent += OnVoteFinished;
                empire.ImpeachmentFinishedEvent += OnImpeachmentFinished;
                empire.DowngradeEvent += DowngradeEmpireToUnion;
                empire.TaxEvent += OnTakeResource;

                empire.TaxRate = 3;;

                Player player = _playerManager.GetPlayerInfoByKingId(union.BallotBox.Candidate.Id);
                empire.Leader = GrandPrivileges(player, union);
                empire.DestroyBallotBox();
            }
        }

        /// <summary>
        /// обработчик события на завершение голосования для империи
        /// </summary>
        /// <param name="empire"></param>
        public void OnVoteFinished(Empire empire)
        {
            empire.BallotBox.Calculate();
            // получаем игрока - кандидата
            Player player = _playerManager.GetPlayerInfoByKingId(empire.BallotBox.Candidate.Id);
            if (empire.BallotBox.IsCandidateVictory)
                empire.Leader = GrandPrivileges(player, empire);
            else
                // выборы не состоялись
                empire.PublishNews(player, NewsType.VoteEndedResultPublished,
                    String.Format("The leader is not changed", player.King.Name));
            empire.DestroyBallotBox();
        }

        /// <summary>
        /// обработчик события на завершение импичмента для империи
        /// </summary>
        /// <param name="empire"></param>
        public void OnImpeachmentFinished(Empire empire)
        {
            empire.BallotBox.Calculate();
            // получаем игрока - кандидата
            Player player = player = _playerManager.GetPlayerInfoByKingId(empire.BallotBox.Candidate.Id); ;
            // лидеру объявили импичмент большинством голосов
            if (empire.BallotBox.IsCandidateVictory)
            {
                TakeAwayLeaderPrivileges(player, empire);
                empire.WithoutLeader = true;
                empire.CurrentTimeWithoutLeader = TimeSpan.Zero;
            }
            else
                // импичмент не состоялся
                empire.PublishNews(player, NewsType.ImpeachmentEndedResultPublished,
                    String.Format("The leader is not changed", player.King.Name));
            empire.DestroyBallotBox();
        }

        /// <summary>
        /// выдача лидерских привилегий
        /// </summary>
        /// <param name="player"></param>
        /// <param name="aliance"></param>
        private Leader GrandPrivileges(Player player, IAlliance aliance)
        {
            //King leader = player.Player.King;
            Leader leader = _playerManager.UpgratePlayerToLeader(player);

            player.Messenger.SendNetworkMessage(new GrandLeaderPrivilegesMessage());
            aliance.PublishNews(player, NewsType.VoteEndedResultPublished,
                String.Format("Player {0} is new leader!", leader.Name));

            return leader;
        }

        /// <summary>
        /// забрать лидерские привилегии
        /// </summary>
        /// <param name="player"></param>
        /// <param name="aliance"></param>
        private void TakeAwayLeaderPrivileges(Player player, IAlliance aliance)
        {
            Debug.Assert(player != null);
            King leader = player.King;
            _playerManager.DowngradePlayerToKing(player);

            player.Messenger.SendNetworkMessage(new TakeAwayLeaderPrivilegesMessage());
            aliance.PublishNews(player, NewsType.ImpeachmentEndedResultPublished,
                String.Format("Player {0} is overthrew!", leader.Name));
        }
        /// <summary>
        /// получение альянса по его участнику
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public IAlliance GetAlianceByMember(King player)
        {
            lock (_aliancesSync)
                return _aliances.Find(x => x.Id == player.UnionId || x.Id == player.EmpireId);
        }

        /// <summary>
        /// получение альянса по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAlliance GetAlianceById(int id)
        {
            lock (_aliancesSync)
                return _aliances.Find(x => x.Id == id);
        }

        /// <summary>
        /// отправить сообщение о взимании налога
        /// </summary>
        /// <param name="king"></param>
        /// <param name="resource"></param>
        public void OnTakeResource(King king, Resource resource)
        {
            IPlayer player = king.Player;
            if (!player.Bot)
                player.Messenger.SendNetworkMessage(new TakeResourceMessage(resource));
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }

        public List<IAlliance> Aliances { get { return _aliances; } }
    }
}
