using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class GameStateExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetGameStateResponse response = (GetGameStateResponse) command;
            
            GameCore.Instance.Player.AddKing(response.King);
            response.King.AttachStartCastle(response.Castle);
            EntitySet<Resource> esr = new EntitySet<Resource>();
            foreach (Resource r in response.Resources)
            {
                esr.Add(r);
            }
            response.King.Resources = esr;

            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            if (mapScene != null)
            {
                mapScene.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<GetGameStateResponse>(mapScene.ShowGetStateResult),
                    response);
            }
        }
    }
}
