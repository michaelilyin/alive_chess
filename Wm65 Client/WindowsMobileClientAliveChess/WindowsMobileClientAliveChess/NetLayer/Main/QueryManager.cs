using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands.BattleCommand;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;

namespace WindowsMobileClientAliveChess.NetLayer.Main
{
    /// <summary>
    /// менеджер запросов
    /// </summary>
    public class QueryManager
    {
        #region Big Map

        // захват шахты
        public static void SendCaptureMine(MapPoint mine)
        {
            CaptureMineRequest request = new CaptureMineRequest();
            request.MineId = mine.Owner.Id;
            ClientApplication.Instance.Transport.Send<CaptureMineRequest>(request);
        }

        // захват замка
        public static void SendCaptureCastle(MapPoint castle)
        {
            CaptureCastleRequest request = new CaptureCastleRequest();
            request.CastleId = castle.Owner.Id;
            ClientApplication.Instance.Transport.Send<CaptureCastleRequest>(request);
        }

        // вход в замок
        public static void SendComeInCastle(MapSector castle)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = castle.Owner.Id;
            ClientApplication.Instance.Transport.Send<ComeInCastleRequest>(request);
        }

        // перемещение короля
        public static void SendMoveKing(int x, int y)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = x;
            request.Y = y;
            ClientApplication.Instance.Transport.Send<MoveKingRequest>(request);
        }

        // сбор ресурса
        //public static void SendCollectResource(Player player, MapObject resource)
        //{
        //    GetResourceRequest request = new GetResourceRequest();
        //    request.ResourceId = resource.Id;
        //    ClientApplication.Instance.Transport.Send<GetResourceRequest>(request);
        //}

        // встреча с другим королем
        public static void SendMeetKing(MapPoint king)
        {
            ContactKingRequest request = new ContactKingRequest();
            request.OpponentId = king.Owner.Id;
            ClientApplication.Instance.Transport.Send<ContactKingRequest>(request);
        }

        // встреча с чужим замком
        public static void SendMeetCastle(MapSector castle)
        {
            ContactCastleRequest request = new ContactCastleRequest();
            request.CastleId = castle.Owner.Id;
            ClientApplication.Instance.Transport.Send<ContactCastleRequest>(request);
        }

        // возврат на карту
        public static void SendBigMapMessage()
        {
            BigMapRequest request = new BigMapRequest();
            ClientApplication.Instance.Transport.Send<BigMapRequest>(request);
        }

        public static void SendGetMapRequest()
        {
            GetMapRequest request = new GetMapRequest();
            ClientApplication.Instance.Transport.Send<GetMapRequest>(request);
        }

        public static void SendGetGameStateRequest()
        {
            GetGameStateRequest getState = new GetGameStateRequest();
            ClientApplication.Instance.Transport.Send<GetGameStateRequest>(getState);
        }

        public static void SendGetObjectsRequestForPlayer()
        {
            GetObjectsRequest request = new GetObjectsRequest();
            request.ForConcreteObserver = false;
            ClientApplication.Instance.Transport.Send<GetObjectsRequest>(request);
        }

        public static void SendGetObjectsRequestForConcreteObserver( 
            IObserver observer, PointTypes observerType)
        {
            GetObjectsRequest request = new GetObjectsRequest();
            request.ObserverId = observer.Id;
            request.ForConcreteObserver = true;
            request.ObserverType = observerType;
            ClientApplication.Instance.Transport.Send<GetObjectsRequest>(request);
        }

        #endregion

        #region Dialog

        // переговоры (битва)
        public static void SendBattleMessage(Dialog dispute, DialogState dType)
        {
            BattleDialogMessage msg = new BattleDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<BattleDialogMessage>(msg);
        }

        // начало войны
        public static void SendWarMessage(Negotiate dispute, DialogState dType)
        {
            WarDialogMessage msg = new WarDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<WarDialogMessage>(msg);
        }

        // переговоры (откуп)
        public static void SendPayOffMessage(Dialog dispute, DialogState dType, 
            int recourceCount)
        {
            PayOffDialogMessage msg = new PayOffDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;
            msg.ResourceCount = recourceCount;

            ClientApplication.Instance.Transport.Send<PayOffDialogMessage>(msg);
        }

        // переговоры (торговля)
        public static void SendTradeMessage(Dialog dispute, DialogState dType)
        {
            MarketDialogMessage msg = new MarketDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;

            ClientApplication.Instance.Transport.Send<MarketDialogMessage>(msg);
        }

        // переговоры (отступление)
        public static void SendCapitulateMessage(Dialog dispute, DialogState dType)
        {
            CapitulateDialogMessage msg = new CapitulateDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;

            ClientApplication.Instance.Transport.Send<CapitulateDialogMessage>(msg);
        }

        // переговоры (захват замка)
        public static void SendCaptureCastleMessage(Dialog dispute, DialogState dType)
        {
            CaptureCastleDialogMessage msg = new CaptureCastleDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
            ClientApplication.Instance.Transport.Send<CaptureCastleDialogMessage>(msg);
        }

        // создание союза
        public static void CreateUnionMessage(Dialog dispute, DialogState dType)
        {
            CreateUnionDialogMessage msg = new CreateUnionDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
            ClientApplication.Instance.Transport.Send<CreateUnionDialogMessage>(msg);
        }

        // заключение мира
        public static void SendPeaceMessage(Negotiate dispute, DialogState dType)
        {
            PeaceDialogMessage msg = new PeaceDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<PeaceDialogMessage>(msg);
        }

        public static void SendJoinEmperiesMessage(Negotiate dispute, DialogState dType)
        {
            JoinEmperiesDialogMessage msg = new JoinEmperiesDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<JoinEmperiesDialogMessage>(msg);
        }

        #endregion

        public static void SendExit()
        {
            ExitFromGameRequest exit = new ExitFromGameRequest();
            ClientApplication.Instance.Transport.Send<ExitFromGameRequest>(exit);
        }

        //Шаг игрока
        public static void SendPlayerMove(Battle battle, byte[] move, bool ok)
        {
            PlayerMoveRequest message = new PlayerMoveRequest();
            message.Move = move;
            message.BattleID = battle.Id;
            message.OpponentID = battle.Respondent.Id;
            message.Ok = ok;
            ClientApplication.Instance.Transport.Send<PlayerMoveRequest>(message);
        }

        // покинуть замок
        public static void SendLeaveCastle()
        {
            LeaveCastleRequest request = new LeaveCastleRequest();
            ClientApplication.Instance.Transport.Send<LeaveCastleRequest>(request);
        }


        #region Empire

        public static void SendCreateUnionMessage(Dialog dispute, DialogState dType)
        {
            CreateUnionDialogMessage msg = new CreateUnionDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
      
            ClientApplication.Instance.Transport.Send<CreateUnionDialogMessage>(msg);
        }

        public static void SendGetUnionOrEmpireInfo()
        {
            GetAlianceInfoRequest msg = new GetAlianceInfoRequest();
            ClientApplication.Instance.Transport.Send<GetAlianceInfoRequest>(msg);
        }

        public static void SendEmbedTaxRate(int rate)
        {
            EmbedTaxRateRequest resp = new EmbedTaxRateRequest();
            resp.Rate = rate;
            ClientApplication.Instance.Transport.Send<EmbedTaxRateRequest>(resp);
        }

        public static void SendExcludeKingFromEmpire(int id)
        {
            ExcludeKingFromEmpireRequest resp = new ExcludeKingFromEmpireRequest();
            resp.KingId = id;
            ClientApplication.Instance.Transport.Send<ExcludeKingFromEmpireRequest>(resp);
        }

        public static void SendExitFromUnionOrEmpire()
        {
            ExitFromAlianceRequest resp = new ExitFromAlianceRequest();
            ClientApplication.Instance.Transport.Send<ExitFromAlianceRequest>(resp);
        }

        public static void SendGetHelpFigure(int count, UnitType type)
        {
            GetHelpFigureRequest resp = new GetHelpFigureRequest();
            resp.FigureCount = count;
            resp.FigureType = type;
            ClientApplication.Instance.Transport.Send<GetHelpFigureRequest>(resp);
        }

        public static void SendGetHelpResource(int count, ResourceTypes type)
        {
            GetHelpResourceRequest resp = new GetHelpResourceRequest();
            resp.ResourceCount = count;
            resp.ResourceType = type;
            ClientApplication.Instance.Transport.Send<GetHelpResourceRequest>(resp);
        }

        public static void SendIncludeKingInEmpire(int id)
        {
            IncludeKingInEmpireRequest resp = new IncludeKingInEmpireRequest();
            resp.KingId = id;
            ClientApplication.Instance.Transport.Send<IncludeKingInEmpireRequest>(resp);
        }

        public static void SendJoinToUnionOrEmpire(int alianceId)
        {
            JoinToAlianceRequest resp = new JoinToAlianceRequest();
            resp.AlianceId = alianceId;
            ClientApplication.Instance.Transport.Send<JoinToAlianceRequest>(resp);
        }

        public static void SendMessageNewsMessage(Player info, NewsType type, string message)
        {
            MessageNewsMessage resp = new MessageNewsMessage();
            resp.News = type;
            resp.Message = message;
            resp.SenderId = info.King.Id;
            ClientApplication.Instance.Transport.Send<MessageNewsMessage>(resp);
        }

        public static void SendStartImpeachment(string message)
        {
            StartImpeachmentRequest resp = new StartImpeachmentRequest();
            resp.Message = message;
            ClientApplication.Instance.Transport.Send<StartImpeachmentRequest>(resp);
        }

        public static void SendStartVote( string message)
        {
            StartVoteRequest resp = new StartVoteRequest();
            resp.Message = message;
            ClientApplication.Instance.Transport.Send<StartVoteRequest>(resp);
        }

        public static void SendFigureHelpMessage(int receiver, List<Unit> units)
        {
            SendFigureHelpMessage resp = new SendFigureHelpMessage();
            resp.ReceiverId = receiver;
            units.ForEach(resp.AddFigure);
            ClientApplication.Instance.Transport.Send<SendFigureHelpMessage>(resp);
        }

        public static void SendResourceHelpMessage(int receiver, List<Resource> res)
        {
            SendResourceHelpMessage resp = new SendResourceHelpMessage();
            resp.ReceiverId = receiver;
            res.ForEach(resp.AddResource);
            ClientApplication.Instance.Transport.Send<SendResourceHelpMessage>(resp);
        }

        public static void SendGetAliancesInfo()
        {
            GetAliancesInfoRequest msg = new GetAliancesInfoRequest();
            ClientApplication.Instance.Transport.Send<GetAliancesInfoRequest>(msg);
        }

        public static void SendVoteFact(bool support)
        {
            VoteBallotMessage msg = new VoteBallotMessage();
            msg.Support = support;
            ClientApplication.Instance.Transport.Send<VoteBallotMessage>(msg);
        }

        public static void SendStartNegotiate(int opponentId)
        {
            StartNegotiateRequest msg = new StartNegotiateRequest();
            msg.OpponentId = opponentId;
            ClientApplication.Instance.Transport.Send<StartNegotiateRequest>(msg);
        }

        #endregion
    }
}
