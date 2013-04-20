using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment
{
    public class AlianceRoutine : IRoutine
    {
        private List<IAliance> _aliances;
        private PlayerManager _playerManager;
        private List<UnitedAliance> _unitedAliances;
        private List<War> _wars;

        private object _aliancesSync = new object();
   
        public AlianceRoutine()
        {
            _aliances = new List<IAliance>();
        }

        /// <summary>
        /// жизненный цикл союзов и империй
        /// </summary>
        /// <param name="time"></param>
        public void DoLogic(GameTime time)
        {
            lock (_aliancesSync)
            {
                foreach (IAliance aliance in Next())
                {
                    aliance.DoLogic(time);
                }
            }
        }

        /// <summary>
        /// добавление альянса
        /// </summary>
        /// <param name="aliance"></param>
        public void Add(IAliance aliance)
        {
            lock (_aliancesSync)
                _aliances.Add(aliance);
            if (aliance.Status == AlianceStatus.Empire)
            {
                Empire e = aliance as Empire;
                e.DowngradeEvent +=
                    new Empire.EmpireHandler(DowngradeEmpireToUnion);
                e.VoteFinishEvent += new Empire.EmpireHandler(OnVoteFinished);
                e.ImpeachmentFinishedEvent += new Empire.EmpireHandler(OnImpeachmentFinished);
                e.TaxEvent += new Empire.TaxHandler(OnTakeResource);
            }
            if (aliance.Status == AlianceStatus.Union)
            {
                Union u = aliance as Union;
                u.VoteFinishEvent += new Union.UnionHandler(OnVoteFinished);
            }
        }

        /// <summary>
        /// удаление альянса
        /// </summary>
        /// <param name="union"></param>
        public void Remove(IAliance aliance)
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
                int index = _aliances.FindIndex(
                    x =>
                    {
                        return x.Id == union.Id;
                    });

                Empire empire = new Empire(union);
                this._aliances[index] = empire;

                this._aliances[index].PublishNews(NewsType.ChangeAlianceStatus, "Union was upgraded to empire");

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
                int index = _aliances.FindIndex(
                    x =>
                    {
                        return x.Id == empire.Id;
                    });
                this._aliances[index] = empire.Union;
                this._aliances[index].PublishNews(NewsType.ChangeAlianceStatus, "Empire was downgraded to union");

                empire.Level.RemoveEmpire(empire);
                empire.Level.AddUnion(empire.Union);
            }
        }

        public void JoinEmperies(params Empire[] emperies)
        {
            UnitedAliance unitedAliance = new UnitedAliance();
            for (int i = 0; i < emperies.Length; i++)
                unitedAliance.AddEmperie(emperies[i]);
        }

        public void StartWar(params Empire[] emperies)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IAliance> Next()
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

                empire.VoteFinishEvent += new Empire.EmpireHandler(OnVoteFinished);
                empire.ImpeachmentFinishedEvent += new Empire.EmpireHandler(OnImpeachmentFinished);
                empire.DowngradeEvent += new Empire.EmpireHandler(DowngradeEmpireToUnion);
                empire.TaxEvent += new Empire.TaxHandler(OnTakeResource);

                empire.TaxRate = 3;
                Store store = new Store();
                empire.Store = store;

                ResourceStore resourceStore = new ResourceStore();
                FigureStore figureStore = new FigureStore();

                store.AddFigureStore(figureStore);
                store.AddResourceStore(resourceStore);

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
        private Leader GrandPrivileges(Player player, IAliance aliance)
        {
            //King leader = player.Player.King;
            Leader leader = _playerManager.UpgratePlayerToLeader(player);

            player.Messenger.SendNetworkMessage(new GrandLeaderPrivilegesMessage());
            aliance.PublishNews(player, NewsType.VoteEndedResultPublished,
                String.Format("Player {0} is new leader!", leader.Name));

            return leader;
        }

        /// <summary>
        /// отъем лидерских привилегий
        /// </summary>
        /// <param name="player"></param>
        /// <param name="aliance"></param>
        private void TakeAwayLeaderPrivileges(Player player, IAliance aliance)
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
        public IAliance GetAlianceByMember(King player)
        {
            lock (_aliancesSync)
            {
                return _aliances.Find(x =>
                    {
                        return x.Id == player.UnionId || x.Id == player.EmpireId;
                    });
            }
        }

        /// <summary>
        /// получение альянса по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAliance GetAlianceById(int id)
        {
            lock (_aliancesSync)
            {
                return _aliances.Find(x => { return x.Id == id; });
            }
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

        public List<IAliance> Aliances
        {
            get { return _aliances; }
        }
    }
}
