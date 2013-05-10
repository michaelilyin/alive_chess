using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class GetCreationRequirementsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetCreationRequirementsResponse response = (GetCreationRequirementsResponse)command;
            lock (GameCore.Instance.Player.King.CurrentCastle.BuildingManager.CreationRequirements)
            {
                if (response.BuildingRequirements != null)
                    GameCore.Instance.Player.King.CurrentCastle.BuildingManager.CreationRequirements = response.BuildingRequirements;
                else
                    GameCore.Instance.Player.King.CurrentCastle.BuildingManager.CreationRequirements.Clear();
            }
            lock (GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.CreationRequirements)
            {
                if (response.RecruitingRequirements != null)
                    GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.CreationRequirements = response.RecruitingRequirements;
                else
                    GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.CreationRequirements.Clear();
            }
        }

        #endregion
    }
}
