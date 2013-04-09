using System;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using BehaviorAILibrary.MotionLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class VerifyPathExecutor : IExecutor
    {
        private readonly PlayerManager _playerManager;
        private readonly AliveChessLogger _logger;
        readonly Func<MapPoint, bool> WayCostVerifier = x => x.WayCost < Constants.ImpassablePoint;

        public VerifyPathExecutor(AliveChessLogger logger, PlayerManager playerManager)
        {
            this._logger = logger;
            this._playerManager = playerManager;
        }

        public void Execute(Message msg)
        {
            VerifyPathRequest request = (VerifyPathRequest) msg.Command;

            //_logger.Log(msg.Sender.Login, "Click", DateTime.Now.ToString());

            if (!msg.Sender.Waiting)
            {
                if (msg.Sender.King.State == KingState.BigMap)
                {
                    msg.Sender.Waiting = true;
                    msg.Sender.Time.SavePreviosTimestamp();

                    if (request.Path != null)
                    {
                        //_logger.Log(msg.Sender.Login, "Start Position",
                        //            msg.Sender.King.X + ":" + msg.Sender.King.Y);

                        int mistakeNumber = 0;
                        FPosition position = null;

                        bool match = PositionsEqual(msg.Sender.King, request.Path[0]);
                        if (match && Motion.VerifyPath(msg.Sender.Map, request.Path,
                            WayCostVerifier, out mistakeNumber, out position))
                        {
                            msg.Sender.King.AddSteps(Motion.Transform(request.Path));
                        }
                        //_logger.Log(msg.Sender.Login,
                        //            "Ban", msg.Sender.King.Id.ToString(),
                        //            msg.Sender.Connection.ToString(),
                        //            msg.Sender.King.X + ":" + msg.Sender.King.Y,
                        //            request.Path[0].X + ":" + request.Path[0].Y);

                        if (!match)
                        {
                            _playerManager.Ban(
                                msg.Sender, "You are starting from impassable area");
                        }
                        else
                        {
                            VerifyPathResponse response = new VerifyPathResponse();
                            {
                                response.X = position.X;
                                response.Y = position.Y;
                            }
                            msg.Sender.Messenger.SendNetworkMessage(response);
                        }
                    }
                }
                else
                {
                    msg.Sender.Messenger.SendNetworkMessage(
                        new ErrorMessage("You're inside castle"));
                }
            }
            else
            {
                //_logger.Log(msg.Sender.Login, "Player is moving or waiting");
                _playerManager.Ban(msg.Sender, "You click too frequent or king is moving now");
            }
        }

        private static bool PositionsEqual(King king, FPosition start)
        {
            return king.X == (int)start.X && king.Y == (int)start.Y;
        }
    }
}
