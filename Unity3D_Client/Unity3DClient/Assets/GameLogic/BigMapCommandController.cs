using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;

namespace Assets.GameLogic
{
    public class BigMapCommandController
    {
        Timer timerUpdateGameState = new Timer();

        public BigMapCommandController()
        {
            timerUpdateGameState.Elapsed += timerUpdateGameState_Tick;
            timerUpdateGameState.Interval = 500;
        }

        public void StartGameStateUpdate()
        {
            timerUpdateGameState.Start();
        }

        public void StopGameStateUpdate()
        {
            timerUpdateGameState.Stop();
        }

        void timerUpdateGameState_Tick(object sender, EventArgs e)
        {
            SendGetGameStateRequest();
        }

        private void SendGetGameStateRequest()
        {
            GetGameStateRequest request = new GetGameStateRequest();
            GameCore.Instance.Network.Send(request);
            Debug.Log("Get state sended");
        }

        public void SendGetMapRequest()
        {
            GetMapRequest request = new GetMapRequest();
            GameCore.Instance.Network.Send(request);
        }

        public void SendMoveKingRequest(int x, int y)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = x;
            request.Y = y;
            GameCore.Instance.Network.Send(request);
        }

    }
}
