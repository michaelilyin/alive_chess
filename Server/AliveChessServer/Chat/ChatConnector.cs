using System;
using System.Collections.Generic;
using AliveChessLibrary.Commands;
using AliveChessPluginLibrary;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessLibrary.Interfaces;
using AliveChessServer.NetLayer;

namespace AliveChessServer.Chat
{
    public class ChatConnector : IChatConnector
    {
        private readonly PlayerManager _playerManager;
        private readonly GameWorld _world;
        private readonly ProtoBufferTransport _transport;

        public ChatConnector(PlayerManager playerManager, GameWorld world, 
            ProtoBufferTransport transport)
        {
            this._playerManager = playerManager;
            this._world = world;
            this._transport = transport;
        }

        public IUser RequestUser(int playerId)
        {
            return _playerManager.GetPlayerInfoById(playerId);
        }

        public bool ProvideCredentials(string login, string password)
        {
            throw new NotImplementedException();
        }

        public List<IUser> RequestAllies(IUser user)
        {
            Level level = _world.LevelManager.GetLevelById(user.Stage.Id);
            IAlliance aliance = level.EmpireManager.GetAlianceById(user.Community.Id);
            if(aliance!=null)
            {
                List<IUser> users = new List<IUser>();
                foreach (var item in aliance.NextMember())
                {
                    if (!item.Player.Bot)
                    {
                        Player player = (Player)item.Player;
                        if (!player.Equals(user))
                            users.Add(player);
                    }
                }
                return users;
            }
            return null;
        }

        public void SendMessage(IConnectionInfo info, byte[] array)
        {
            _transport.SocketTransport.Send(array, info.Socket);
        }

        public void SendMessage<T>(IConnectionInfo info, T command) where T : ICommand
        {
            _transport.Send(info.Socket, command);
        }

        public List<ICommunity> RequestCommunities(IStage stage)
        {
            Level level = (Level) stage;
            List<ICommunity> result = new List<ICommunity>();
            foreach (var community in level.EmpireManager.Aliances)
            {
                result.Add(community);
            }

            return result;
        }

        public List<ICommunity> RequestCommunities()
        {
            List<ICommunity> result = new List<ICommunity>();
            foreach (var level in _world.LevelManager.NextLevel())
            {
                foreach (var community in level.EmpireManager.Aliances)
                {
                    result.Add(community);
                }
            }

            return result;
        }
    }
}
