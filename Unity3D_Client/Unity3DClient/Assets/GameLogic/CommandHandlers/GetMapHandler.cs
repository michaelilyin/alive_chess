using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.CommandHandlers
{
    class GetMapHandler : CommandHandler
    {
        public GetMapHandler()
            : base(Command.GetMapResponse)
        {

        }

        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            Debug.Log("Handle get map");
            GetMapResponse response = (GetMapResponse)command;
            GameCore.Instance.World.Create(response);
        }
    }
}
