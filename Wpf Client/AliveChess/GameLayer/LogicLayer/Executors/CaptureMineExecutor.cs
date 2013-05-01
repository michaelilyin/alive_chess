using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class CaptureMineRequestExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            CaptureMineResponse response = (CaptureMineResponse) command;
            GameCore.Instance.World.Map.SearchMineById(response.Mine.Id).KingId = response.Mine.KingId;

            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            if (mapScene != null)
            {
                mapScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action<CaptureMineResponse>(mapScene.ShowCaptureMineResult), response);
            }
        }
    }
}
