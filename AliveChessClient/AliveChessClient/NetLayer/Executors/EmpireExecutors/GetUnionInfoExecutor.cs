using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class GetUnionInfoExecutor : IExecutor
    { 
        private Game context;
        private AliveChessDelegate handler;
        private MemberDelegate h;

        public GetUnionInfoExecutor(Game context)
        {
            this.context = context;
            handler = new AliveChessDelegate(context.GameForm.StartAlianceDialog);
           // h = new MemberDelegate(context.GameForm.AlianceControl.ShowMembers);
        }

        public void Execute(ICommand cmd)
        {
            GetAlianceInfoResponse msg = (GetAlianceInfoResponse)cmd;
            context.Members = msg.Members;
            context.UnionId = msg.UnionId;
            context.GameForm.BigMapControl.Invoke(handler);
            //context.GameForm.AlianceControl.Invoke(h, msg.Members);
        }

        public delegate void MemberDelegate(List<GetAlianceInfoResponse.MemberInfo> m);
    }
}
