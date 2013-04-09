using System;
using AliveChessPluginLibrary;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.ChatCommand;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat
{
    [AliveChessPluginAttribute(true)]
    public class ChatPlugin : IChatPlugin
    {
        private List<ICommand> commandqueue = new List<ICommand>();
        private IAliveChessPluginContext plugincontext = null;
        private string name;
        Dictionary<Command, IExecutor> dict = new Dictionary<Command, IExecutor>();
        private delegate void IExecutor(ICommand command);
        public void Receive(IBytePackage package, IConnectionInfo from)
        {
            //старый метод    
            return;
        }
        public void Receive(ICommand command, IConnectionInfo from)
        {
            dict[command.Id](command);
        }
        public void JoinLeaveCommandExecutor(ICommand command)
        {
            JoinLeaveCommand cmd = command as JoinLeaveCommand;
            commandqueue.Add(cmd);
        }
        public void MessageCommandExecutor(ICommand command)
        {
            MessageCommand cmd = command as MessageCommand;
            commandqueue.Add(cmd);
        }
        
        
        public void AfterCreate(IAliveChessPluginContext context)
        {
            plugincontext = context;
            name = "Chat";
            dict.Add(Command.JoinLeaveCommand, (new IExecutor(JoinLeaveCommandExecutor)));
            dict.Add(Command.MessageCommand, new IExecutor(MessageCommandExecutor));
           // dict.Add(Command.MessageReceiveCommand, new IExecutor(MessageReceiveCommandExecutor));
           // dict.Add(Command.SendContactListCommand,new IExecutor(SendContactListCommandExecutor));
            Chat.Context=context;
            Chat.CreateChannels(); //инициализация каналов
        }
        public string Name
        {
            get { return name; }
        }
        public void BeforeDestroy()
        {

        }
        public void Execute()
        {
            ICommand command = commandqueue[0];
            commandqueue.RemoveAt(0);
            switch (command.Id)
            {
                case Command.JoinLeaveCommand:
                    if(((JoinLeaveCommand)command).ConOrDisc==0)
                        Chat.OnClientConnected(((JoinLeaveCommand)command).ID_CLIENT);
                    if (((JoinLeaveCommand)command).ConOrDisc == 1)
                        Chat.OnClientDisconnected(((JoinLeaveCommand)command).ID_CLIENT);
                    break;
                case Command.MessageCommand:
                    Chat.SendToChannel(((MessageCommand)command).Id_Sender, ((MessageCommand)command).Id_Getter, ((MessageCommand)command).Message);
                    break;
            }

        }
    }
}
