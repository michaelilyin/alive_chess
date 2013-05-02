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
    public class GetGameStateExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetGameStateResponse response = (GetGameStateResponse) command;
            
            //GameCore.Instance.Player.AddKing(response.King);
            /*GameCore.Instance.Player.King.Castles = response.King.Castles;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);
            GameCore.Instance.Player.King.X = response.King.;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);*/
            GameCore.Instance.Player.King = response.King;
            EntitySet<Resource> esr = new EntitySet<Resource>();
            foreach (Resource r in response.Resources)
            {
                esr.Add(r);
            }
            GameCore.Instance.Player.King.Resources = esr;

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            if (mapScene != null)
            {
                mapScene.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<GetGameStateResponse>(mapScene.ShowGetStateResult),
                    response);
            }*/
            GameCore.Instance.BigMapCommandController.ReceiveGetGameStateResponse(response);
        }
    }
}
