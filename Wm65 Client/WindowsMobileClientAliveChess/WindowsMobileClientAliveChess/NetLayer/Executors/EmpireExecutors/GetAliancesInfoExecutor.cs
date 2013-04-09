using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class GetAliancesInfoExecutor : IExecutor
    {
        private Game context;
        private AlianceHandler h;

        public GetAliancesInfoExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        public delegate void AlianceHandler(List<GetAliancesInfoResponse.AlianceInfo> a);
    }
}
