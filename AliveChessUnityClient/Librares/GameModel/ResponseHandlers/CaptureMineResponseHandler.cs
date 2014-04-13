using AliveChessLibrary.Commands.BigMapCommand;
using GameModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class CaptureMineResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            CaptureMineResponse response = (CaptureMineResponse)command;
            GameCore.Instance.Network.Messages.AddMessage(String.Format("Your are capture the {0} mine", NamesConverter.GetNameByType(response.Mine.MineType)));
        }
    }
}
