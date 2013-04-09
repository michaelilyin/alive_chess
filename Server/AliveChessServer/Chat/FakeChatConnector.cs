using System;
using System.Collections.Generic;
using AliveChessLibrary.Commands;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;
using AliveChessPluginLibrary;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.Chat
{
    public class FakeChatConnector : IChatConnector
    {
        private List<Player> _players = new List<Player>();
        List<ICommunity> communities = new List<ICommunity>();
        private AllianceRoutine _alianceRoutine = new AllianceRoutine(null);

        public FakeChatConnector()
        {
            Initialize();
        }

        private void Initialize()
        {
            Level level = new Level {Id = 1};

            Union union1 = new Union {Id = 1};

            Union union2 = new Union {Id = 2};
            Empire empire1 = new Empire(union2) {Id = 3};

            _alianceRoutine.Add(union1);
            _alianceRoutine.Add(empire1);

            Player player1 = new Player {Id = 1, Level = level};
            Player player2 = new Player {Id = 2, Level = level};
            Player player3 = new Player {Id = 3, Level = level};
            Player player4 = new Player {Id = 4, Level = level};

            _players.Add(player1);
            _players.Add(player2);
            _players.Add(player3);
            _players.Add(player4);

            King king1 = new King {Player = player1};
            King king2 = new King {Player = player2};
            King king3 = new King {Player = player3};
            King king4 = new King {Player = player4};

            union1.AddMember(king1);
            union1.AddMember(king2);

            empire1.AddMember(king3);
            empire1.AddMember(king4);

            foreach (IAlliance a in level.EmpireManager.Aliances)
                communities.Add(a);
        }

        public IUser RequestUser(int playerId)
        {
            return _players.Search(x => x.Id == playerId);
        }

        public bool ProvideCredentials(string login, string password)
        {
            return _players.Exists(x => x.Login == login && x.Password == password);
        }

        public List<IUser> RequestAllies(IUser user)
        {
            List<IUser> users = new List<IUser>();
            IAlliance aliance = _alianceRoutine.GetAlianceById(user.Community.Id);
            if (aliance != null)
            {
                foreach (var item in aliance.NextMember())
                {
                    if (!item.Player.Bot)
                    {
                        Player player = (Player)item.Player;
                        if (!player.Equals(user))
                            users.Add(player);
                    }
                }
            }
            return users;
        }

        public void SendMessage(IConnectionInfo info, byte[] array)
        {
            throw new NotImplementedException();
        }

        public void SendMessage<T>(IConnectionInfo info, T command) where T : ICommand
        {
            throw new NotImplementedException();
        }

        public List<ICommunity> RequestCommunities(IStage stage)
        {
            Level level = (Level)stage;
            List<ICommunity> result = new List<ICommunity>();
            foreach (var community in level.EmpireManager.Aliances)
            {
                result.Add(community);
            }

            return result;
        }

        public List<ICommunity> RequestCommunities()
        {
            return communities;
        }
    }
}
