using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Net;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Commands.BattleCommand;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessLibrary.Utility;

namespace AliveChessServer.NetLayer
{
    public class QueryManager
    {
        private ProtoBufferTransport _transport;

        public QueryManager(ProtoBufferTransport transport)
        {
            this._transport = transport;
        }

        #region Registration

        public void SendRegister(ConnectionInfo info, bool isSuccessed)
        {
            RegisterResponse register = new RegisterResponse();
            register.IsSuccessed = isSuccessed;
            _transport.Send<RegisterResponse>(info.Socket, register);
        }

        public void SendAuthorize(PlayerInfo info)
        {
            AuthorizeResponse authorize = new AuthorizeResponse();

            authorize.King = info.Player.King;
            authorize.Castle = info.Player.King.StartCastle;
            authorize.IsSuperUser = info.Player.IsSuperUser;
            authorize.StartResources = info.Player.King.ResourceStore.Resources.ToList();
         
            _transport.Send<AuthorizeResponse>(info.Connection.Socket, authorize);
        }

        public void SendExit(PlayerInfo info)
        {
            ExitFromGameResponse exit = new ExitFromGameResponse();
            _transport.Send<ExitFromGameResponse>(info.Connection.Socket, exit);
        }

        #endregion

        #region Big Map

        public void SendMap(PlayerInfo info, List<MapPoint> points, List<MapSector> sectors, 
            List<Landscape> basePoints)
        {
            GetMapResponse response = new GetMapResponse();
            response.Points = points;
            response.Objects = sectors;
            response.BasePoints = basePoints;
            response.MapId = info.Player.Map.Id;
            response.SizeMapX = info.Player.Map.SizeX;
            response.SizeMapY = info.Player.Map.SizeY;
            _transport.Send<GetMapResponse>(info.Connection.Socket, response);
        }

        public void SendObjects(PlayerInfo info, List<MapPoint> objects, 
            List<MapSector> sectors)
        {
            GetObjectsResponse response = new GetObjectsResponse();
            response.Objects = objects;
            response.Sectors = sectors;
            _transport.Send<GetObjectsResponse>(info.Connection.Socket, response);
        }

        public void SendUpdateWorld(PlayerInfo info, MapPoint mObject)
        {
            UpdateWorldMessage message = new UpdateWorldMessage();
            message.MObject = mObject;
            _transport.Send<UpdateWorldMessage>(info.Connection.Socket, message);
        }

        public void SendMoveKing(PlayerInfo info, List<Position> path)
        {
            MoveKingResponse response = new MoveKingResponse();
            response.Steps = path;
            _transport.Send<MoveKingResponse>(info.Connection.Socket, response);
        }

        public void SendResource(PlayerInfo info, Resource resource, bool fromMine)
        {
            GetResourceMessage response = new GetResourceMessage();
            response.Resource = resource;
            response.FromMine = fromMine;
            _transport.Send<GetResourceMessage>(info.Connection.Socket, response);
        }

        public void TakeResource(PlayerInfo info, Resource resource)
        {
            TakeResourceMessage response = new TakeResourceMessage();
            response.Resource = resource;
            _transport.Send<TakeResourceMessage>(info.Connection.Socket, response);
        }

        public void SendMapReturn(PlayerInfo info)
        {
            BigMapResponse response = new BigMapResponse();
            response.IsAllowed = true;
            _transport.Send<BigMapResponse>(info.Connection.Socket, response);
        }

        public void SendLooseCastle(PlayerInfo info, Castle castle)
        {
            LooseCastleMessage response = new LooseCastleMessage();
            response.CastleId = castle.Id;
            _transport.Send<LooseCastleMessage>(info.Connection.Socket, response);
        }

        public void SendCaptureCastle(PlayerInfo info, Castle castle)
        {
            CaptureCastleResponse response = new CaptureCastleResponse();
            response.Castle = castle;
            _transport.Send<CaptureCastleResponse>(info.Connection.Socket, response);
        }

        public void SendLooseMine(PlayerInfo info, Mine mine)
        {
            LooseMineMessage response = new LooseMineMessage();
            response.MineId = mine.Id;
            _transport.Send<LooseMineMessage>(info.Connection.Socket, response);
        }

        public void SendCaptureMine(PlayerInfo info, Mine mine)
        {
            CaptureMineResponse response = new CaptureMineResponse();
            response.Mine = mine;
            _transport.Send<CaptureMineResponse>(info.Connection.Socket, response);
        }

        public void SendComeInCastle(PlayerInfo info, uint castleId)
        {
            ComeInCastleResponse response = new ComeInCastleResponse();
            response.CastleId = castleId;
            _transport.Send<ComeInCastleResponse>(info.Connection.Socket, response);
        }

        public void SendContactCastle(PlayerInfo info, Dispute dispute, Castle castle)
        {
            ContactCastleResponse response = new ContactCastleResponse();
            response.Castle = castle;
            response.Dispute = dispute;
            _transport.Send<ContactCastleResponse>(info.Connection.Socket, response);
        }

        public void SendContactKing(PlayerInfo info, Dispute dispute)
        {
            ContactKingResponse response = new ContactKingResponse();
            response.Discussion = dispute;
            _transport.Send<ContactKingResponse>(info.Connection.Socket, response);
        }

        #endregion

        #region Dialog

        public void SendDeactivateDialog(PlayerInfo info)
        {
            DeactivateDialogMessage response = new DeactivateDialogMessage();
            _transport.Send<DeactivateDialogMessage>(info.Connection.Socket, response);
        }

        public void SendBattleDialogMessage(PlayerInfo info, Dispute dispute)
        {
            BattleDialogMessage message = new BattleDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<BattleDialogMessage>(info.Connection.Socket, message);
        }

        public void SendCapitulateDialogMessage(PlayerInfo info, Dispute dispute)
        {
            CapitulateDialogMessage message = new CapitulateDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<CapitulateDialogMessage>(info.Connection.Socket, message);
        }

        public void SendPayOffDialogMessage(PlayerInfo info, Dispute dispute)
        {
            PayOffDialogMessage message = new PayOffDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<PayOffDialogMessage>(info.Connection.Socket, message);
        }

        public void SendMarketDialogMessage(PlayerInfo info, Dispute dispute)
        {
            MarketDialogMessage message = new MarketDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<MarketDialogMessage>(info.Connection.Socket, message);
        }

        public void SendCaptureCastleDialogMessage(PlayerInfo info, Dispute dispute)
        {
            CapitulateDialogMessage message = new CapitulateDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<CapitulateDialogMessage>(info.Connection.Socket, message);
        }

        public void SendCreateUnionDialogMessage(PlayerInfo info, Dispute dispute)
        {
            CreateUnionDialogMessage message = new CreateUnionDialogMessage();
            message.DisputeId = dispute.Id;
            message.State = dispute.State;
            _transport.Send<CreateUnionDialogMessage>(info.Connection.Socket, message);
        }

        #endregion

        #region Castle

        public void SendLeaveCastle(PlayerInfo info)
        {
            LeaveCastleResponse response = new LeaveCastleResponse();
            _transport.Send<LeaveCastleResponse>(info.Connection.Socket, response);
        }

        /*----------------------------Slisarenko-------------------------------------------*/

        //public void SendGetResBuildings(PlayerInfo context, int count, ResourceTypes type)
        //{
        //    GetRecBuildingsResponse resp = new GetRecBuildingsResponse();
        //    resp.Count = count;
        //    resp.Type = type;
        //    _transport.Send<GetRecBuildingsResponse>(context.Connection.Socket, resp);
        //}

        public void SendGetResBuildings(PlayerInfo context, ResBuild rb)
        {
            GetRecBuildingsResponse resp = new GetRecBuildingsResponse();
            resp.ResBuild = rb;
            _transport.Send<GetRecBuildingsResponse>(context.Connection.Socket, resp);
        }

        // Выдать список зданий построенных в городе
        public void SendGetListBuildings(PlayerInfo pl, List<InnerBuilding> s)
        {
            BuildingInCastleResponse resp = new BuildingInCastleResponse();
            resp.Buildings_list = s;
            _transport.Send<BuildingInCastleResponse>(pl.Connection.Socket, resp);
        }

        //Выдать армию замка
        public void SendGetArmyCastle(PlayerInfo pl, List<Unit> arm)
        {
            BuyFigureResponse resp = new BuyFigureResponse();
            resp.Units = arm;
            _transport.Send<BuyFigureResponse>(pl.Connection.Socket, resp);

        }

        //Армия замка передана королю!!!
        public void SendGetArmyToKing(PlayerInfo pl, List<Unit> arm)
        {
            GetArmyCastleToKingResponse resp = new GetArmyCastleToKingResponse();
            resp.Arm = arm;
            _transport.Send<GetArmyCastleToKingResponse>(pl.Connection.Socket, resp);
        }

        #endregion

        #region Battle

        public void SendDownloadBattlefild(PlayerInfo context, Battle battle)
        {
            DownloadBattlefildResponse resp = new DownloadBattlefildResponse();
            resp.Battle = battle;
            _transport.Send<DownloadBattlefildResponse>(context.Connection.Socket, resp);
        }

        //ответ на действие игрока
        public void SendPlayerMove(PlayerInfo context, byte[] move, int count)
        {
            PlayerMoveResponse resp = new PlayerMoveResponse();
            resp.Move = move;
            resp.Countunit = count;
            _transport.Send<PlayerMoveResponse>(context.Connection.Socket, resp);
        }

        #endregion

        #region Empire

        /// <summary>
        /// установка налога (доступно для лидера)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendEmbedTaxRate(PlayerInfo info, bool successed)
        {
            EmbedTaxRateResponse resp = new EmbedTaxRateResponse();
            resp.Successed = successed;
            _transport.Send<EmbedTaxRateResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// исключение короля из империи (доступно для лидера)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendExcludeKingFromEmpire(PlayerInfo info, bool successed)
        {
            ExcludeKingFromEmpireResponse resp = new ExcludeKingFromEmpireResponse();
            resp.Successed = successed;
            _transport.Send<ExcludeKingFromEmpireResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// выход из союза либо империи
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendExitFromAliance(PlayerInfo info, bool successed)
        {
            ExitFromAlianceResponse resp = new ExitFromAlianceResponse();
            resp.Successed = successed;
            _transport.Send<ExitFromAlianceResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отправка фигур королю
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendGetHelpFigure(PlayerInfo info, List<Unit> units)
        {
            GetHelpFigureResponse resp = new GetHelpFigureResponse();
            resp.Units = units;
            _transport.Send<GetHelpFigureResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отправка ресурсов королю
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendGetHelpResource(PlayerInfo info, List<Resource> resources)
        {
            GetHelpResourceResponse resp = new GetHelpResourceResponse();
            resp.Resources = resources;
            _transport.Send<GetHelpResourceResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// выдача лидерских привилегий
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendGrandLeaderPrivileges(PlayerInfo info)
        {
            GrandLeaderPrivilegesMessage resp = new GrandLeaderPrivilegesMessage();
            _transport.Send<GrandLeaderPrivilegesMessage>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отмена привилеий лидера
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendTakeAwayLeaderPrivilegesMessage(PlayerInfo info)
        {
            TakeAwayLeaderPrivilegesMessage resp = new TakeAwayLeaderPrivilegesMessage();
            _transport.Send<TakeAwayLeaderPrivilegesMessage>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// принятие короля в империю (доступно для лидера)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendIncludeKingInEmpire(PlayerInfo info, bool succesed)
        {
            IncludeKingInEmpireResponse resp = new IncludeKingInEmpireResponse();
            resp.Successed = succesed;
            _transport.Send<IncludeKingInEmpireResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// исключение игрока из империи (отправляется выгнанному из империи игроку)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendExcludeKingFromEmpireMessage(PlayerInfo info)
        {
            ExcludeFromEmpireMessage resp = new ExcludeFromEmpireMessage();
            _transport.Send<ExcludeFromEmpireMessage>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отправка королю сообщения о факте принятия его в союз
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendJoinToAliance(PlayerInfo info, bool succesed)
        {
            JoinToAlianceResponse resp = new JoinToAlianceResponse();
            resp.Successed = succesed;
            _transport.Send<JoinToAlianceResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отправка новости
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendMessageNewsMessage(PlayerInfo sender, PlayerInfo receiver, NewsType type, string message)
        {
            MessageNewsMessage resp = new MessageNewsMessage();
            resp.News = type;
            resp.Message = message;
            if (sender != null)
                resp.SenderId = sender.Player.King.Id;
            _transport.Send<MessageNewsMessage>(receiver.Connection.Socket, resp);
        }

        /// <summary>
        /// начать импичмент лидеру (доступно для империи)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendStartImpeachment(PlayerInfo info, bool successed)
        {
            StartImpeachmentResponse resp = new StartImpeachmentResponse();
            resp.Successed = successed;
            _transport.Send<StartImpeachmentResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// начать голосование
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendStartVote(PlayerInfo info, bool successed)
        {
            StartVoteResponse resp = new StartVoteResponse();
            resp.Successed = successed;
            _transport.Send<StartVoteResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// отправка сообщения лидеру о желании короля вступить в имерию (доступно для империи)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendJoinRequest(PlayerInfo info, King candidate)
        {
            JoinRequestMessage resp = new JoinRequestMessage();
            resp.Candidate = candidate;
            _transport.Send<JoinRequestMessage>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// получение информации о союзе или империи
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendGetAlianceInfo(PlayerInfo info, IAliance aliance)
        {
            GetAlianceInfoResponse resp = new GetAlianceInfoResponse();
            resp.UnionId = aliance.Id;
            aliance.Kings.ForEach(
                x =>
                {
                    GetAlianceInfoResponse.MemberInfo member =
                        new GetAlianceInfoResponse.MemberInfo();
                    member.MemberId = x.Id;
                    member.MemberName = x.Name;
                    resp.Members.Add(member);
                });
            _transport.Send<GetAlianceInfoResponse>(info.Connection.Socket, resp);
        }

        /// <summary>
        /// получение информации о всех союзах или империях на уровне
        /// </summary>
        /// <param name="info"></param>
        /// <param name="successed"></param>
        public void SendGetAliancesInfo(PlayerInfo info, List<IAliance> aliances, bool withLeaders)
        {
            GetAliancesInfoResponse resp = new GetAliancesInfoResponse();
            aliances.ForEach(
                x =>
                {
                    GetAliancesInfoResponse.AlianceInfo a =
                        new GetAliancesInfoResponse.AlianceInfo();

                    a.Id = x.Id;
                    a.Name = "";
                    if (withLeaders && x.Status == AlianceStatus.Empire)
                    {
                        Empire e = x as Empire;

                        GetAlianceInfoResponse.MemberInfo leader =
                            new GetAlianceInfoResponse.MemberInfo();

                        leader.MemberId = e.Leader.Id;
                        leader.MemberName = e.Leader.Name;
                        a.Leader = leader;
                    }
                    resp.Aliances.Add(a);
                });
            _transport.Send<GetAliancesInfoResponse>(info.Connection.Socket, resp);
        }

        public void SendStartNegotiate(PlayerInfo info, Negotiate negotiate)
        {
            StartNetotiateResponse response = new StartNetotiateResponse();
            response.Negotiate = negotiate;
            _transport.Send<StartNetotiateResponse>(info.Connection.Socket, response);
        }

        #endregion
    }
}
