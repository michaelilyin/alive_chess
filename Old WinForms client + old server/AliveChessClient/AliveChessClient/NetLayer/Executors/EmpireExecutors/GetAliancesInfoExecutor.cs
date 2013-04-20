using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class GetAliancesInfoExecutor : IExecutor
    {
        private Game context;
        private AlianceHandler h;

        public GetAliancesInfoExecutor(Game context)
        {
            this.context = context;
            this.h = new AlianceHandler(context.BigMap.ShowAliances);
        }

        public void Execute(ICommand cmd)
        {
            GetAliancesInfoResponse resp = (GetAliancesInfoResponse)cmd;
            context.Aliances = resp.Aliances;
            context.BigMap.Invoke(h, resp.Aliances);
        }

        public delegate void AlianceHandler(List<GetAliancesInfoResponse.AlianceInfo> a);
    }
}
