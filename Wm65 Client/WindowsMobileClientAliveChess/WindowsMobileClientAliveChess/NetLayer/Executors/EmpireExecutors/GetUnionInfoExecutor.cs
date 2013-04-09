using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class GetUnionInfoExecutor : IExecutor
    { 
        private Game context;
        private AliveChessDelegate handler;
        private MemberDelegate h;

        public GetUnionInfoExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        public delegate void MemberDelegate(List<GetAlianceInfoResponse.MemberInfo> m);
    }
}
