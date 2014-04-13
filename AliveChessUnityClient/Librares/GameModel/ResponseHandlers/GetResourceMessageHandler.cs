using AliveChessLibrary.Commands.BigMapCommand;
using GameModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class GetResourceMessageHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetResourceMessage response = (GetResourceMessage)command;
            GameCore.Instance.Network.Messages.AddMessage(String.Format("You are collected the {0} of {1} resource", response.Resource.Quantity, NamesConverter.GetNameByType(response.Resource.ResourceType)));
        }
    }
}
