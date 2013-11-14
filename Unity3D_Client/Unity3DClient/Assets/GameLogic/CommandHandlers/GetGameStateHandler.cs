using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.CommandHandlers
{
    class GetGameStateHandler : CommandHandler
    {
        public GetGameStateHandler()
            :base(Command.GetGameStateResponse)
        {

        }

        public override void Handle(ICommand command)
        {
            Debug.Log("Update King");
            GetGameStateResponse result = (GetGameStateResponse)command;
            if (result.King != null)
            {
                if (GameCore.Instance.Player.King == null)
                {
                    GameCore.Instance.Player.King = result.King;
                }
                else
                {
                    lock (GameCore.Instance.Player.King)
                    {
                        GameCore.Instance.Player.King.X = result.King.X;
                        GameCore.Instance.Player.King.Y = result.King.Y;
                        GameCore.Instance.Player.King.Experience = result.King.Experience;
                        GameCore.Instance.Player.King.MilitaryRank = result.King.MilitaryRank;
                    }
                }
                if (GameCore.Instance.Player.King.ResourceStore == null)
                    GameCore.Instance.Player.King.ResourceStore = new ResourceStore();
                GameCore.Instance.Player.King.ResourceStore.SetResources(result.Resources);
            }
            Debug.Log("King position " + GameCore.Instance.Player.King.X + " " + GameCore.Instance.Player.King.Y);
        }
    }
}
