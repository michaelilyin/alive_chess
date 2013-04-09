using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MainAliveChessLibrary;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Commands.ChatCommand;

namespace Chat
{
    public class Channel
    {
        Dictionary<int,IUser> usersonline = new Dictionary<int,IUser>();
        public Channel()
        {
            
        }  
        public bool ConnectUser(IUser user,int id_user)
        {
            if (!usersonline.ContainsKey(id_user))
            {
                usersonline.Add(id_user,user);
                SendUserList();
                return true;
            }
            return false;
        }
        public bool DisconnectUser(IUser user,int id_user)
        {
            if (usersonline.ContainsKey(id_user))
            {
                usersonline.Remove(id_user);
                SendUserList();
                return true;
            }
            return false;
        }
        public bool Send(IUser user_sender, int id_sender, string msg)
        {
            if(user_sender.Role != 0)
                if (msg[0] == '!')
                {
                    string regexp = @"";
                    for (int i = 1; i < msg.Length; i++)
                        regexp += msg[i];
                    Censorship.AddExp(regexp);//добавление админом цензурного шаблона 
                    MessageReceiveCommand cmd = new MessageReceiveCommand();
                    cmd.Id_Sender = id_sender;
                    cmd.Login_Sender=user_sender.Login;
                    cmd.Message = "Шаблон "+msg+" успешно добавлен";
                    cmd.MessageType = 1;
                    Chat.Context.Server.Chat.SendMessage<MessageReceiveCommand>(user_sender.Connection,cmd);
                    return true;
                }
            if (!usersonline.ContainsKey(id_sender))
                            return false;
            if (!Censorship.CheckMessage(msg))
                return false;
            MessageReceiveCommand command=new MessageReceiveCommand();
            command.Id_Sender = id_sender;
            command.Login_Sender=user_sender.Login;
            command.Message = msg;
            command.MessageType = 0;
            foreach (KeyValuePair<int,IUser> user in usersonline)
                Chat.Context.Server.Chat.SendMessage<MessageReceiveCommand>(user.Value.Connection,command); //TODO!!!
            return true;
        }
        private void SendUserList()
        {
            SendContactListCommand command = new SendContactListCommand();
            Dictionary<int,string> contdict = new Dictionary<int,string>();
            foreach (KeyValuePair<int,IUser> user in usersonline)
            {
                command.Ids.Add(user.Key);
                command.Logins.Add(user.Value.Login);
            }
            foreach (KeyValuePair<int, IUser> user in usersonline)
                Chat.Context.Server.Chat.SendMessage<SendContactListCommand>(user.Value.Connection,command);
        }
    }
}
