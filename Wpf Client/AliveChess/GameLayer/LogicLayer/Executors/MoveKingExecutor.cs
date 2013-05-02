using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;


namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class MoveKingExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            MoveKingResponse response = (MoveKingResponse)command;

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<MoveKingResponse>(mapScene.ShowMoveKingResult),
                response);*/
        }
    }
}
