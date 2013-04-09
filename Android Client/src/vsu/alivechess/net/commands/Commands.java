/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package vsu.alivechess.net.commands;

/**
 *
 * @author Al-mal
 */
public class Commands {

    // команды авторизации:   диапазон: 0 - 5 количество: (6)
    public static final int AuthorizeRequest           = 0;
    public static final int AuthorizeResponse          = 1;
    public static final int ExitFromGameRequest        = 2;
    public static final int ExitFromGameResponse       = 3;

    public static final int RegisterRequest            = 4;
    public static final int RegisterResponse           = 5;

    // команды большой карты: диапазон: 6 - 55 количество: (50)
    public static final int BigMapRequest              = 6;
    public static final int BigMapResponse             = 7;
    public static final int CaptureCastleRequest       = 8;
    public static final int CaptureCastleResponse      = 9;
    public static final int CaptureMineRequest         = 10;
    public static final int CaptureMineResponse        = 11;
    public static final int ComeInCastleRequest        = 12;
    public static final int ComeInCastleResponse       = 13;
    public static final int ContactKingRequest         = 14;
    public static final int ContactKingResponse        = 15;
    public static final int TakeResourceMessage        = 16;
    public static final int MoveKingRequest            = 17;
    public static final int MoveKingResponse           = 18;
    public static final int ContactCastleRequest       = 19;
    public static final int ContactCastleResponse      = 20;
    public static final int GetMapRequest              = 21;
    public static final int GetMapResponse             = 22;
    public static final int GetObjectsRequest          = 23;
    public static final int GetObjectsResponse         = 24;
    public static final int LooseCastleMessage         = 25;
    public static final int LooseMineMessage           = 26;
    public static final int UpdateWorldMessage         = 27;
    public static final int GetResourceMessage         = 28;
    public static final int GetUnityMapRequest         = 29;
    public static final int GetUnityMapResponse        = 30;
    public static final int GetGameStateRequest        = 31;
    public static final int GetGameStateResponse       = 32;
    public static final int ComputePathRequest         = 33;
    public static final int ComputePathResponse        = 34;
    public static final int VerifyPathRequest          = 35;
    public static final int VerifyPathResponse         = 36;
    public static final int GetKingRequest             = 37;
    public static final int GetKingResponse            = 38;

// команды системного диалога: диапазон: 56 - 75 количество: (20)
    public static final int BattleDialogMessage        = 56;
    public static final int PayOffDialogMessage        = 57;
//
    public static final int MarketDialogMessage        = 59;
    public static final int CapitulateDialogMessage    = 60;
    public static final int CaptureCastleDialogMessage = 61;
    public static final int DeactivateDialogMessage    = 62;
    public static final int CreateUnionDialogMessage   = 63;
    public static final int WarDialogMessage           = 64;
    public static final int PeaceDialogMessage         = 65;
    public static final int JoinEmperiesDialogMessage  = 66;

    // команды замка: диапазон: 76 - 125(50)
    public static final int LeaveCastleRequest         = 76;
    public static final int LeaveCastleResponse        = 77;
    public static final int GetArmyCastleToKingRequest = 78;
    public static final int GetArmyCastleToKingResponse = 79;
    public static final int GetListBuildingsInCastleRequest  = 80;
    public static final int GetListBuildingsInCastleResponse = 81;
    public static final int GetRecBuildingsRequest     = 82;
    public static final int GetRecBuildingsResponse    = 83;
    public static final int ShowArmyCastleRequest      = 84;
    public static final int ShowArmyKingRequest        = 85;
    public static final int BuildingInCastleRequest    = 86;
    public static final int BuildingInCastleResponse   = 87;
    public static final int BuyFigureRequest           = 88;
    public static final int BuyFigureResponse          = 89;

    // команды шахматного поединка: диапазон 126 - 145(20)
    public static final int BattleMessage              = 127;
    public static final int DownloadBattlefildRequest  = 128;
    public static final int DownloadBattlefildResponse = 129;
    public static final int MoveUnitRequest            = 130;
    public static final int MoveUnitResponse           = 131;

    // команды управлениями союзами и империями: 146 - 205 (50)
    //
    public static final int JoinRequestMessage              = 149;
    public static final int EmbedTaxRateRequest             = 150;
    public static final int EmbedTaxRateResponse            = 151;
    public static final int ExcludeKingFromEmpireRequest    = 152;
    public static final int ExcludeKingFromEmpireResponse   = 153;
    public static final int IncludeKingInEmpireRequest      = 154;
    public static final int IncludeKingInEmpireResponse     = 155;
    public static final int ExitFromAlianceRequest          = 156;
    public static final int ExitFromAlianceResponse         = 157;
    public static final int GetHelpFigureRequest            = 158;
    public static final int GetHelpFigureResponse           = 159;
    public static final int GetHelpResourceRequest          = 160;
    public static final int GetHelpResourceResponse         = 161;
    public static final int GetAlianceInfoRequest           = 162;
    public static final int GetAlianceInfoResponse          = 163;
    public static final int GrandLeaderPrivilegesMessage    = 164;
    public static final int TakeAwayLeaderPrivilegesMessage = 165;
    public static final int JoinToAlianceRequest            = 166;
    public static final int JoinToAlianceResponse           = 167;
    public static final int MessageNewsMessage              = 168;
    public static final int SendFigureHelpMessage           = 169;
    public static final int SendResourceHelpMessage         = 170;
    public static final int StartVoteRequest                = 171;
    public static final int StartVoteResponse		    = 172;
    public static final int StartImpeachmentRequest	    = 173;
    public static final int StartImpeachmentResponse	    = 174;
    public static final int VoteBallotMessage		    = 175;
    public static final int GetAliancesInfoRequest	    = 176;
    public static final int GetAliancesInfoResponse	    = 177;
    public static final int ExcludeFromEmpireMessage	    = 178;
    public static final int StartNegotiateRequest	    = 179;
    public static final int StartNegotiateResponse          = 180;

    // команды торговли:           ?

    // команды статистики: 206 - 225(20)
    public static final int GetStatisticRequest	            = 206;
    public static final int GetStatisticResponse	    = 207;
    public static final int GetAvailableMapsRequest	    = 208;
    public static final int GetAvailableMapsResponse	    = 209;

    public static final int ErrorMessage                    = 666;
}

