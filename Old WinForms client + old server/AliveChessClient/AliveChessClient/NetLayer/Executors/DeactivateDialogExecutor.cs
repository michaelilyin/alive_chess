using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors
{
    public class DeactivateDialogExecutor : IExecutor
    {
        private Game context;
        private DisabledHandler handler;

        public DeactivateDialogExecutor(Game context)
        {
            this.context = context;
            this.handler = new DisabledHandler(context.GameForm.BattleDisputeControl.SetDisabled);
        }

        public void Execute(ICommand cmd)
        {
            DeactivateDialogMessage resp = (DeactivateDialogMessage)cmd;
            context.GameForm.BattleDisputeControl.Invoke(handler);
            QueryManager.SendBigMapMessage(context.Player);
        }

        private delegate void DisabledHandler();
    }
}
