using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class GetCreationRequirementsHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetCreationRequirementsResponse response = (GetCreationRequirementsResponse)command;
            lock (GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager.CreationRequirements)
            {
                if (response.BuildingRequirements != null)
                    GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager.CreationRequirements = response.BuildingRequirements;
                else
                    GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager.CreationRequirements.Clear();
            }
            lock (GameCore.Instance.World.Player.King.CurrentCastle.RecruitingManager.CreationRequirements)
            {
                if (response.RecruitingRequirements != null)
                    GameCore.Instance.World.Player.King.CurrentCastle.RecruitingManager.CreationRequirements = response.RecruitingRequirements;
                else
                    GameCore.Instance.World.Player.King.CurrentCastle.RecruitingManager.CreationRequirements.Clear();
            }
        }
    }
}
