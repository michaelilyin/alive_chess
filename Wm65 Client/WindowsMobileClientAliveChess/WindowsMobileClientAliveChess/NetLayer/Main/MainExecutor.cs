using System.Collections.Generic;
using System.Threading;
using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.NetLayer.Executors;
using WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors;
using WindowsMobileClientAliveChess.NetLayer.Executors.DialogExecutors;
using WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors;
using WindowsMobileClientAliveChess.NetLayer.Transport;
using AliveChessLibrary.Commands;

namespace WindowsMobileClientAliveChess.NetLayer.Main
{
    public class MainExecutor
    {
        private CommandPool commandPool;
        private SocketTransport transport;

        private static IDictionary<string, IExecutor> executors;

        public MainExecutor(CommandPool theCommandPool, SocketTransport theTransport, Game game)
        {
            commandPool = theCommandPool;
            transport = theTransport;

            executors = new Dictionary<string, IExecutor>();
            executors.Add("AuthorizeResponseExecutor", new AuthorizeExecutor(game));
            executors.Add("LeaveCastleResponseExecutor", new LeaveCastleExecutor(game));
            executors.Add("UpdateWorldMessageExecutor", new UpdateWorldExecutor(game));
            executors.Add("CaptureMineResponseExecutor", new CaptureMineExecutor(game));
            executors.Add("LooseMineMessageExecutor", new LooseMineExecutor(game));
            executors.Add("MoveKingResponseExecutor", new MoveKingExecutor(game));
            executors.Add("GetResourceMessageExecutor", new GetResourceExecutor(game));
            executors.Add("TakeResourceMessageExecutor", new TakeResourceExecutor(game));
            executors.Add("ComeInCastleResponseExecutor", new ComeInCastleExecutor(game));
            executors.Add("ContactKingResponseExecutor", new DisputeKingExecutor(game));
            executors.Add("MarketDialogMessageExecutor", new TradeMsgExecutor(game));
            executors.Add("BigMapResponseExecutor", new BigMapExecutor(game));
            executors.Add("BattleDialogMessageExecutor", new MessageBattleExecutor(game));
            executors.Add("CapitulateDialogMessageExecutor", new MessageCapitulateExecutor(game));
            executors.Add("PayOffDialogMessageExecutor", new MessagePayOffExecutor(game));
            executors.Add("CaptureCastleDialogMessageExecutor", new MessageCastleExecutor(game));
            executors.Add("ContactCastleResponseExecutor", new DisputeCastleExecutor(game));
            executors.Add("CaptureCastleResponseExecutor", new CaptureCastleExecutor(game));
            executors.Add("LooseCastleMessageExecutor", new LooseCastleExecutor(game));
            executors.Add("GetMapResponseExecutor", new GetMapExecutor(game));
            executors.Add("GetGameStateResponseExecutor", new GetGameStateExecutor(game));
            executors.Add("GetObjectsResponseExecutor", new GetObjectsExecutor(game));
            executors.Add("ExitFromGameResponseExecutor", new ExitFromGameExecutor(game));
            executors.Add("DeactivateDialogMessageExecutor", new DeactivateDialogExecutor(game));

            executors.Add("GetRecBuildingsResponseExecutor", new GetRecBuildingsExequtor(game));
            executors.Add("BuildingInCastleResponseExecutor", new BuildingInCastleResponseExecutor(game));
            executors.Add("BuyFigureResponseExecutor", new HireUnitExecutor(game));
            executors.Add("DownloadBattlefildResponseExecutor", new DownloadBattlefildExecutor(game));
            executors.Add("PlayerMoveResponseExecutor", new PlayerMoveResponseExecutor(game));
            executors.Add("GetArmyCastleToKingResponseExecutor", new ShowArmyKingExecutor(game));


            executors.Add("CreateUnionDialogMessageExecutor", new CreateUnionExecutor(game));
            executors.Add("GetAlianceInfoResponseExecutor", new GetUnionInfoExecutor(game));
            executors.Add("MessageNewsMessageExecutor", new NewsExecutor(game));
            executors.Add("EmbedTaxRateResponseExecutor", new EmbedTaxRateExecutor(game));
            executors.Add("ExcludeKingFromEmpireResponseExecutor", new ExcludeKingFromEmpireExecutor(game));
            executors.Add("ExitFromAlianceResponseExecutor", new ExitFromAlianceExecutor(game));
            executors.Add("GetHelpFigureResponseExecutor", new GetHelpFigureExecutor(game));
            executors.Add("GetHelpResourceResponseExecutor", new GetHelpResourceExecutor(game));
            executors.Add("GrandLeaderPrivilegesMessageExecutor", new GrandLeaderPrivilegesExecutor(game));
            executors.Add("IncludeKingInEmpireResponseExecutor", new IncludeKingInEmpireExecutor(game));
            executors.Add("JoinRequestMessageExecutor", new JoinRequestExecutor(game));
            executors.Add("JoinToAlianceResponseExecutor", new JoinToAlianceExecutor(game));
            executors.Add("StartImpeachmentResponseExecutor", new StartImpeachmentExecutor(game));
            executors.Add("StartVoteResponseExecutor", new StartVoteExecutor(game));
            executors.Add("TakeAwayLeaderPrivilegesMessageExecutor", new TakeAwayLeaderPrivilegesExecutor(game));
            executors.Add("GetAliancesInfoResponseExecutor", new GetAliancesInfoExecutor(game));
            executors.Add("ExcludeFromEmpireMessageExecutor", new ExcludeMessageExecutor(game));
            executors.Add("StartNegotiateResponseExecutor", new StartNegotiateExecutor(game));
            executors.Add("WarDialogMessageExecutor", new WarMessageExecutor(game));
            executors.Add("PeaceDialogMessageExecutor", new PeaceMessageExecutor(game));
            executors.Add("JoinEmperiesDialogMessageExecutor", new JoinEmperiesMessageExecutor(game));
            executors.Add("ErrorMessageExecutor", new ErrorMessageExecutor(game));
        }

        public void Execute()
        {
            while (true)
            {
                if (commandPool.Count > 0)
                {
                    ICommand cmd = commandPool.Dequeue();
                    CallExecutor(((Command)cmd.Id).ToString() + "Executor", cmd);
                }
            }
            Thread.Sleep(10);
        }

        private static void CallExecutor(string executorName, ICommand cmd)
        {
            executors[executorName].Execute(cmd);
        }
    }
}
