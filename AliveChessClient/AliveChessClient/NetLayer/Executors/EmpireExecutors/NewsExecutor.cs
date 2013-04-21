using System;
using System.Windows.Forms;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class NewsExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;
        private KingList h1;
       // private KingList h2;

        public NewsExecutor(Game context)
        {
            this.context = context;
            handler = new AliveChessDelegateWithText(context.GameForm.BigMapControl.SetText);

            h1 = delegate(int id)
            {
                CheckedListBox.ObjectCollection l = 
                    context.GameForm.AlianceControl.KingList.Items;
                for (int i = 0; i < l.Count; i++)
                {
                    uint index = Convert.ToUInt32(l[i]);
                    if (index == id)
                        context.GameForm.AlianceControl.KingList.SetItemChecked(i, true);
                }
            };

            //h2 = delegate(uint id)
            //{
            //    CheckedListBox.ObjectCollection l =
            //        context.GameForm.LeaderControl.KingList.Items;
            //    for (int i = 0; i < l.Count; i++)
            //    {
            //        uint index = Convert.ToUInt32(l[i]);
            //        if (index == id)
            //            context.GameForm.LeaderControl.KingList.SetItemChecked(i, true);
            //    }
            //};
        }

        public void Execute(ICommand cmd)
        {
            MessageNewsMessage msg = (MessageNewsMessage)cmd;
            context.GameForm.Invoke(handler, msg.Message);
            if (msg.News == NewsType.HelpFigure || msg.News == NewsType.HelpResource)
                context.GameForm.Invoke(h1, msg.SenderId);
            //if(msg.News == NewsType.PlayerWantJoinToAliance)
               // context.GameForm.Invoke(h2, msg.SenderId);
        }

        private delegate void KingList(int id);
    }
}
