using System;
using AliveChessPluginLibrary;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Commands.ChatCommand;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat
{

    public static class Chat
    {
        private static IAliveChessPluginContext context=null;
        //private static List<Channel> channels= new List<Channel>();
        private static Dictionary<int,Channel> channels = new Dictionary<int,Channel>();
        private static List<ICommunity> communities;
        public static void CreateChannels()
        {
            channels.Add(-2,new Channel()); //первым делом канал администраторов
            channels.Add(-1, new Channel()); //затем общий канал
            //if (context!=null)                //!!!!!!!!!!!!TODO!!!!!!!!!!!!!!!!!!
                communities =context.Server.Chat.RequestCommunities();
                for (int i = 0; i < communities.Count; i++)
                    channels.Add(communities[i].Id,new Channel());

        }
        public static void OnClientConnected(int id)
        {
            IUser user = context.Server.Chat.RequestUser(id);
            if (user.Role == 0) //если игрок не админ и не модер
            {
              
                        channels[user.Community.Id].ConnectUser(user,id); //подключить его к каналу его союза
                        channels[-1].ConnectUser(user,id);//подключить его к общему каналу
            }
            else //если модер или админ
              
                        channels[-2].ConnectUser(user,id); //подключить к админскому каналу
                        channels[-1].ConnectUser(user,id); //подключить к общему каналу

        }
        public static void OnClientDisconnected(int id)
        {
            IUser user = context.Server.Chat.RequestUser(id);
            if (user.Role == 0)
            {
                channels[user.Community.Id].DisconnectUser(user,id);
                channels[-1].DisconnectUser(user,id);
            }
            else
                channels[-2].DisconnectUser(user,id);
                channels[-1].DisconnectUser(user, id);

        }
        public static void SendToChannel(int id_sender, int id_getter, string msg)
        {
            IUser user = context.Server.Chat.RequestUser(id_sender);
            switch (id_getter)
            {
                case -2:// сообщение в канал своего союза
                    channels[user.Community.Id].Send(user, id_sender, msg);
                    break;
                case -1: // сообщение в общий канал
                    channels[-1].Send(user, id_sender, msg);
                    break;
                default:
                    MessageReceiveCommand cmd = new MessageReceiveCommand();
                    cmd.Id_Sender=id_sender;
                    cmd.Login_Sender=user.Login;
                    cmd.Message=msg;
                    cmd.MessageType=1;
                    context.Server.Chat.SendMessage<MessageReceiveCommand>(user.Connection, cmd);//TODO!!!
                    break;
            }        
        
        }
        public static IAliveChessPluginContext Context
        {
            get{return context;}
            set { context = value; }
            
        }
        
        

        
    }
}
