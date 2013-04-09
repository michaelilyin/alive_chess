using System.Collections.Generic;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class GetAlianceInfoExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public GetAlianceInfoExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            GetAlianceInfoRequest request = (GetAlianceInfoRequest) msg.Command;
            Level level = _levelManager.GetLevelById(msg.Sender.LevelId);
            IAlliance aliance = level.EmpireManager.GetAlianceByMember(msg.Sender.King);
            msg.Sender.King.State = KingState.GetInfo;
            msg.Sender.Messenger.SendNetworkMessage(
                new GetAlianceInfoResponse(aliance.Id, GetAlianceInfo(aliance)));
        }

        private List<GetAlianceInfoResponse.MemberInfo> GetAlianceInfo(IAlliance aliance)
        {
            var result =
                new List<GetAlianceInfoResponse.MemberInfo>();

            for (int i = 0; i < aliance.Kings.Count; i++)
            {
                var member = new GetAlianceInfoResponse.MemberInfo
                                 {
                    MemberId = aliance.Kings[i].Id,
                    MemberName = aliance.Kings[i].Name
                };
                result.Add(member);
            }
            return result;
        }
    }
}
