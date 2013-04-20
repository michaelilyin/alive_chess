using System.Collections.Generic;
using AliveChessClient.GameLayer;
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

namespace AliveChessClient.NetLayer.Main
{
    /// <summary>
    /// менеджер запросов
    /// </summary>
    public class QueryManager
    {
        #region Big Map

        // захват шахты
        public static void SendCaptureMine(Player player, MapPoint mine)
        {
            CaptureMineRequest request = new CaptureMineRequest();
            request.MineId = mine.Id;
            ClientApplication.Instance.Transport.Send<CaptureMineRequest>(request);
        }

        // захват замка
        public static void SendCaptureCastle(Player player, MapPoint castle)
        {
            CaptureCastleRequest request = new CaptureCastleRequest();
            request.CastleId = castle.Id;
            ClientApplication.Instance.Transport.Send<CaptureCastleRequest>(request);
        }

        // вход в замок
        public static void SendComeInCastle(Player player, MapSector castle)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = castle.Id;
            ClientApplication.Instance.Transport.Send<ComeInCastleRequest>(request);
        }

        // перемещение короля
        public static void SendMoveKing(Player player, int x, int y)
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
        public static void SendMeetKing(Player player, MapPoint king)
        {
            ContactKingRequest request = new ContactKingRequest();
            request.OpponentId = king.Id;
            ClientApplication.Instance.Transport.Send<ContactKingRequest>(request);
        }

        // встреча с чужим замком
        public static void SendMeetCastle(Player player, MapSector castle)
        {
            ContactCastleRequest request = new ContactCastleRequest();
            request.CastleId = castle.Id;
            ClientApplication.Instance.Transport.Send<ContactCastleRequest>(request);
        }

        // возврат на карту
        public static void SendBigMapMessage(Player player)
        {
            BigMapRequest request = new BigMapRequest();
            ClientApplication.Instance.Transport.Send<BigMapRequest>(request);
        }

        public static void SendGetMapRequest(Player player)
        {
            GetMapRequest request = new GetMapRequest();
            ClientApplication.Instance.Transport.Send<GetMapRequest>(request);
        }

        public static void SendGetObjectsRequestForPlayer(Player player)
        {
            GetObjectsRequest request = new GetObjectsRequest();
            request.ForConcreteObserver = false;
            ClientApplication.Instance.Transport.Send<GetObjectsRequest>(request);
        }

        public static void SendGetObjectsRequestForConcreteObserver(Player player, 
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
        public static void SendBattleMessage(Player player, Dialog dispute, DialogState dType)
        {
            BattleDialogMessage msg = new BattleDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<BattleDialogMessage>(msg);
        }

        // начало войны
        public static void SendWarMessage(Player player, Negotiate dispute, DialogState dType)
        {
            WarDialogMessage msg = new WarDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<WarDialogMessage>(msg);
        }

        // переговоры (откуп)
        public static void SendPayOffMessage(Player player, Dialog dispute, DialogState dType, 
            int recourceCount)
        {
            PayOffDialogMessage msg = new PayOffDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;
            msg.ResourceCount = recourceCount;

            ClientApplication.Instance.Transport.Send<PayOffDialogMessage>(msg);
        }

        // переговоры (торговля)
        public static void SendTradeMessage(Player player, Dialog dispute, DialogState dType)
        {
            MarketDialogMessage msg = new MarketDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;

            ClientApplication.Instance.Transport.Send<MarketDialogMessage>(msg);
        }

        // переговоры (отступление)
        public static void SendCapitulateMessage(Player player, Dialog dispute, DialogState dType)
        {
            CapitulateDialogMessage msg = new CapitulateDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;

            ClientApplication.Instance.Transport.Send<CapitulateDialogMessage>(msg);
        }

        // переговоры (захват замка)
        public static void SendCaptureCastleMessage(Player player, Dialog dispute, DialogState dType)
        {
            CaptureCastleDialogMessage msg = new CaptureCastleDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
            ClientApplication.Instance.Transport.Send<CaptureCastleDialogMessage>(msg);
        }

        // создание союза
        public static void CreateUnionMessage(Player player, Dialog dispute, DialogState dType)
        {
            CreateUnionDialogMessage msg = new CreateUnionDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
            ClientApplication.Instance.Transport.Send<CreateUnionDialogMessage>(msg);
        }

        // заключение мира
        public static void SendPeaceMessage(Player player, Negotiate dispute, DialogState dType)
        {
            PeaceDialogMessage msg = new PeaceDialogMessage();
            msg.State = dType;
            msg.DisputeId = dispute.Id;

            ClientApplication.Instance.Transport.Send<PeaceDialogMessage>(msg);
        }

        public static void SendJoinEmperiesMessage(Player player, Negotiate dispute, DialogState dType)
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
        public static void SendLeaveCastle(Player player)
        {
            LeaveCastleRequest request = new LeaveCastleRequest();
            ClientApplication.Instance.Transport.Send<LeaveCastleRequest>(request);
        }


        #region Empire

        public static void SendCreateUnionMessage(Player player, Dialog dispute, DialogState dType)
        {
            CreateUnionDialogMessage msg = new CreateUnionDialogMessage();
            msg.DisputeId = dispute.Id;
            msg.State = dType;
      
            ClientApplication.Instance.Transport.Send<CreateUnionDialogMessage>(msg);
        }

        public static void SendGetUnionOrEmpireInfo(Player player)
        {
            GetAlianceInfoRequest msg = new GetAlianceInfoRequest();
            ClientApplication.Instance.Transport.Send<GetAlianceInfoRequest>(msg);
        }

        public static void SendEmbedTaxRate(Player info, int rate)
        {
            EmbedTaxRateRequest resp = new EmbedTaxRateRequest();
            resp.Rate = rate;
            ClientApplication.Instance.Transport.Send<EmbedTaxRateRequest>(resp);
        }

        public static void SendExcludeKingFromEmpire(Player info, int id)
        {
            ExcludeKingFromEmpireRequest resp = new ExcludeKingFromEmpireRequest();
            resp.KingId = id;
            ClientApplication.Instance.Transport.Send<ExcludeKingFromEmpireRequest>(resp);
        }

        public static void SendExitFromUnionOrEmpire(Player info)
        {
            ExitFromAlianceRequest resp = new ExitFromAlianceRequest();
            ClientApplication.Instance.Transport.Send<ExitFromAlianceRequest>(resp);
        }

        public static void SendGetHelpFigure(Player info, int count, UnitType type)
        {
            GetHelpFigureRequest resp = new GetHelpFigureRequest();
            resp.FigureCount = count;
            resp.FigureType = type;
            ClientApplication.Instance.Transport.Send<GetHelpFigureRequest>(resp);
        }

        public static void SendGetHelpResource(Player info, int count, ResourceTypes type)
        {
            GetHelpResourceRequest resp = new GetHelpResourceRequest();
            resp.ResourceCount = count;
            resp.ResourceType = type;
            ClientApplication.Instance.Transport.Send<GetHelpResourceRequest>(resp);
        }

        public static void SendIncludeKingInEmpire(Player info, int id)
        {
            IncludeKingInEmpireRequest resp = new IncludeKingInEmpireRequest();
            resp.KingId = id;
            ClientApplication.Instance.Transport.Send<IncludeKingInEmpireRequest>(resp);
        }

        public static void SendJoinToUnionOrEmpire(Player info, int alianceId)
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

        public static void SendStartImpeachment(Player info, string message)
        {
            StartImpeachmentRequest resp = new StartImpeachmentRequest();
            resp.Message = message;
            ClientApplication.Instance.Transport.Send<StartImpeachmentRequest>(resp);
        }

        public static void SendStartVote(Player info, string message)
        {
            StartVoteRequest resp = new StartVoteRequest();
            resp.Message = message;
            ClientApplication.Instance.Transport.Send<StartVoteRequest>(resp);
        }

        public static void SendFigureHelpMessage(Player info, int receiver, List<Unit> units)
        {
            SendFigureHelpMessage resp = new SendFigureHelpMessage();
            resp.ReceiverId = receiver;
            units.ForEach(resp.AddFigure);
            ClientApplication.Instance.Transport.Send<SendFigureHelpMessage>(resp);
        }

        public static void SendResourceHelpMessage(Player info, int receiver, List<Resource> res)
        {
            SendResourceHelpMessage resp = new SendResourceHelpMessage();
            resp.ReceiverId = receiver;
            res.ForEach(resp.AddResource);
            ClientApplication.Instance.Transport.Send<SendResourceHelpMessage>(resp);
        }

        public static void SendGetAliancesInfo(Player player)
        {
            GetAliancesInfoRequest msg = new GetAliancesInfoRequest();
            ClientApplication.Instance.Transport.Send<GetAliancesInfoRequest>(msg);
        }

        public static void SendVoteFact(Player player, bool support)
        {
            VoteBallotMessage msg = new VoteBallotMessage();
            msg.Support = support;
            ClientApplication.Instance.Transport.Send<VoteBallotMessage>(msg);
        }

        public static void SendStartNegotiate(Player player, int opponentId)
        {
            StartNegotiateRequest msg = new StartNegotiateRequest();
            msg.OpponentId = opponentId;
            ClientApplication.Instance.Transport.Send<StartNegotiateRequest>(msg);
        }

        #endregion
    }
}
