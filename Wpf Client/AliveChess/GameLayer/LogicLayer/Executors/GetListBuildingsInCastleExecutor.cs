using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    class GetListBuildingsInCastleExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetListBuildingsInCastleResponse response = (GetListBuildingsInCastleResponse)command;

            CastleScene castleScene = (CastleScene)GameCore.Instance.WindowContext.Find("SceneCastle", false);
            
            castleScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetListBuildingsInCastleResponse>(castleScene.ShowGetListBuildingsInCastleResult),
                response);
        }

        #endregion
    }
}
