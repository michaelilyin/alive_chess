/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Logic.Contexts;

import commands.Statistic.GetStatisticResponse;
import commands.Statistic.GetAvailableMapsResponse;
import commands.Error.ErrorMessage;
import commands.Empire.SendFigureHelpMessage;
import commands.Empire.GrandLeaderPrivilegesMessage;
import commands.Empire.MessageNewsMessage;
import commands.Empire.ExcludeKingFromEmpireResponse;
import commands.Empire.EmbedTaxRateResponse;
import commands.Empire.JoinToAlianceResponse;
import commands.Empire.StartVoteResponse;
import commands.Empire.ExcludeFromEmpireMessage;
import commands.Empire.StartImpeachmentResponse;
import commands.Empire.GetHelpFigureResponse;
import commands.Empire.JoinRequestMessage;
import commands.Empire.SendResourceHelpMessage;
import commands.Empire.TakeAwayLeaderPrivilegesMessage;
import commands.Empire.VoteBallotMessage;
import commands.Empire.GetHelpResourceResponse;
import commands.Empire.StartNegotiateResponse;
import commands.Empire.GetAliancesInfoResponse;
import commands.Empire.GetAlianceInfoResponse;
import commands.Empire.ExitFromAlianceResponse;
import commands.Empire.IncludeKingInEmpireResponse;
import commands.Dialog.BattleDialogMessage;
import commands.Dialog.CaptureCastleDialogMessage;
import commands.Dialog.PeaceDialogMessage;
import commands.Dialog.CreateUnionDialogMessage;
import commands.Dialog.MarketDialogMessage;
import commands.Dialog.PayOffDialogMessage;
import commands.Dialog.WarDialogMessage;
import commands.Dialog.DeactivateDialogMessage;
import commands.Dialog.JoinEmperiesDialogMessage;
import commands.Dialog.CapitulateDialogMessage;
import commands.Castle.LeaveCastleResponse;
import commands.Castle.BuildingInCastleResponse;
import commands.Castle.GetRecBuildingsResponse;
import commands.Castle.GetArmyCastleToKingResponse;
import commands.Castle.GetListBuildingsInCastleResponse;
import commands.Castle.BuyFigureResponse;
import commands.BigMap.CaptureMineResponse;
import commands.BigMap.LooseMineMessage;
import commands.BigMap.ComputePathResponse;
import commands.BigMap.CaptureCastleResponse;
import commands.BigMap.UpdateWorldMessage;
import commands.BigMap.GetUnityMapResponse;
import commands.BigMap.GetGameStateResponse;
import commands.BigMap.GetMapResponse;
import commands.BigMap.GetResourceMessage;
import commands.BigMap.GetKingResponse;
import commands.BigMap.ContactKingResponse;
import commands.BigMap.LooseCastleMessage;
import commands.BigMap.TakeResourceMessage;
import commands.BigMap.ComeInCastleResponse;
import commands.BigMap.BigMapResponse;
import commands.BigMap.MoveKingResponse;
import commands.BigMap.GetObjectsResponse;
import commands.BigMap.ContactCastleResponse;
import commands.BigMap.VerifyPathResponse;
import commands.Battle.MoveUnitResponse;
import commands.Battle.DownloadBattlefieldResponse;
import commands.Authorization.ExitFromGameResponse;
import commands.Authorization.AuthorizeResponse;
import commands.Authorization.RegisterResponse;
//import Commands.Authorization.*;
//import Commands.Battle.*;
//import Commands.BigMap.*;
//import Commands.Castle.*;
//import Commands.Dialog.*;
//import Commands.Empire.*;
//import Commands.Error.*;
//import Commands.Statistic.*;
import Logic.Contexts.CommandListeners.*;

/**
 *
 * @author Admin
 * 
 * здесь, возможно как раз и надо будет прописывать реакцию игры на приходящие от сервера команды
 */
public class GameContext implements IAuthorizationCommandListener, IBattleCommandListener,
                                    IBigMapCommandListener, ICastleCommandListener,
                                    IChatCommandListener, IDialogCommandListener,
                                    IEmpireCommandListener, IErrorCommandListener,
                                    IStatisticCommandListener
{

    public void AuthorizeResponseReceived(AuthorizeResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ExitFromGameResponseReceived(ExitFromGameResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void RegisterResponseReceived(RegisterResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void DownloadBattlefieldResponseReceived(DownloadBattlefieldResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void MoveUnitResponseReceived(MoveUnitResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void BigMapResponseReceived(BigMapResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void CaptureCastleResponseReceived(CaptureCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void CaptureMineResponseReceived(CaptureMineResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ComeInCastleResponseReceived(ComeInCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ComputePathResponseReceived(ComputePathResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ContactCastleResponseReceived(ContactCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ContactKingResponseReceived(ContactKingResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetGameStateResponseReceived(GetGameStateResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetKingResponseReceived(GetKingResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetMapResponseReceived(GetMapResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetObjectsResponseReceived(GetObjectsResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetResourceMessReceived(GetResourceMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetUnityMapResponseReceived(GetUnityMapResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void LooseCastleMessReceived(LooseCastleMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void LooseMineMessReceived(LooseMineMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void MoveKingResponseReceived(MoveKingResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void TakeResourceMessReceived(TakeResourceMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void UpdateWorldMessReceived(UpdateWorldMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void VerifyPathResponseReceived(VerifyPathResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void BuildingInCastleResponseReceived(BuildingInCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void BuyFigureResponseReceived(BuyFigureResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetArmyCastleToKingResponseReceived(GetArmyCastleToKingResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetListBuildingsInCastleResponseReceived(GetListBuildingsInCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetRecBuildingsResponseReceived(GetRecBuildingsResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void LeaveCastleResponseReceived(LeaveCastleResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void BattleDialogMessReceived(BattleDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void CapitulateDialogMessReceived(CapitulateDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void CaptureCastleDialogMessReceived(CaptureCastleDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void CreateUnionDialogMessReceived(CreateUnionDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void DeactivateDialogMessReceived(DeactivateDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void JoinEmperiesDialogMessReceived(JoinEmperiesDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void MarketDialogMessReceived(MarketDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void PayOffDialogMessReceived(PayOffDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void PeaceDialogMessReceived(PeaceDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void WarDialogMessReceived(WarDialogMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void EmbedTaxRateResponseReceived(EmbedTaxRateResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ExcludeFromEmpireMessReceived(ExcludeFromEmpireMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ExcludeKingFromEmpireResponseReceived(ExcludeKingFromEmpireResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ExitFromAlianceResponseReceived(ExitFromAlianceResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetAlianceInfoResponseReceived(GetAlianceInfoResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetAliancesInfoResponseReceived(GetAliancesInfoResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetHelpFigureResponseReceived(GetHelpFigureResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetHelpResourceResponseReceived(GetHelpResourceResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GrandLeaderPrivilegesMessReceived(GrandLeaderPrivilegesMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void IncludeKingInEmpireResponseReceived(IncludeKingInEmpireResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void JoinRequestMessReceived(JoinRequestMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void JoinToAlianceResponseReceived(JoinToAlianceResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void MessageNewsMessReceived(MessageNewsMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void SendFigureHelpMessReceived(SendFigureHelpMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void SendResourceHelpMessReceived(SendResourceHelpMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void StartImpeachmentResponseReceived(StartImpeachmentResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void StartNegotiateResponseReceived(StartNegotiateResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void StartVoteResponseReceived(StartVoteResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void TakeAwayLeaderPrivilegesMessReceived(TakeAwayLeaderPrivilegesMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void VoteBallotMessReceived(VoteBallotMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void ErrorMessReceived(ErrorMessage c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetAvailableMapsResponseReceived(GetAvailableMapsResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    public void GetStatisticResponseReceived(GetStatisticResponse c) {
        throw new UnsupportedOperationException("Not supported yet.");
    }


}
