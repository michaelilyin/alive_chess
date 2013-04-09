namespace AliveChessLibrary.Commands
{
    public enum Command
    {
        // --------------------- Сектор команд кодируемых протобуфером (0 - 210)---------------------- //

        // команды авторизации:   диапазон: 0 - 5 количество: (6)
        AuthorizeRequest           = 0,
        AuthorizeResponse          = 1,
        ExitFromGameRequest        = 2,
        ExitFromGameResponse       = 3,
        RegisterRequest            = 4,
        RegisterResponse           = 5,

        // команды большой карты: диапазон: 6 - 55 количество: (50)
        BigMapRequest              = 6,
        BigMapResponse             = 7,
        CaptureCastleRequest       = 8,
        CaptureCastleResponse      = 9,
        CaptureMineRequest         = 10,
        CaptureMineResponse        = 11,
        ComeInCastleRequest        = 12,
        ComeInCastleResponse       = 13,
        ContactKingRequest         = 14,
        ContactKingResponse        = 15,
        TakeResourceMessage        = 16,
        MoveKingRequest            = 17,
        MoveKingResponse           = 18,
        ContactCastleRequest       = 19,
        ContactCastleResponse      = 20,
        GetMapRequest              = 21,
        GetMapResponse             = 22,
        GetObjectsRequest          = 23,
        GetObjectsResponse         = 24,
        LooseCastleMessage         = 25,
        LooseMineMessage           = 26,
        UpdateWorldMessage         = 27,
        GetResourceMessage         = 28,
        GetUnityMapRequest         = 29,
        GetUnityMapResponse        = 30,
        GetGameStateRequest        = 31,
        GetGameStateResponse       = 32,
        ComputePathRequest         = 33,
        ComputePathResponse        = 34,
        VerifyPathRequest          = 35,
        VerifyPathResponse         = 36,
        GetKingRequest             = 37,
        GetKingResponse            = 38,

        // команды системного диалога: диапазон: 56 - 75 количество: (20)
        BattleDialogMessage        = 56,
        PayOffDialogMessage        = 57,
        //
        MarketDialogMessage        = 59,
        CapitulateDialogMessage    = 60,
        CaptureCastleDialogMessage = 61,
        DeactivateDialogMessage    = 62,
        CreateUnionDialogMessage   = 63,
        WarDialogMessage           = 64,
        PeaceDialogMessage         = 65,
        JoinEmperiesDialogMessage  = 66,

        // команды замка: диапазон: 76 - 125(50)
        LeaveCastleRequest         = 76,
        LeaveCastleResponse        = 77,
        GetArmyCastleToKingRequest = 78,
        GetArmyCastleToKingResponse = 79,
        GetListBuildingsInCastleRequest  = 80,
        GetListBuildingsInCastleResponse = 81,
        GetRecBuildingsRequest     = 82,
        GetRecBuildingsResponse    = 83,
        ShowArmyCastleRequest      = 84,
        ShowArmyCastleResponse     = 90,
        ShowArmyKingRequest        = 85,
        ShowArmyKingResponse       = 91,
        BuildingInCastleRequest    = 86,
        BuildingInCastleResponse   = 87,
        BuyFigureRequest           = 88,
        BuyFigureResponse          = 89,

        // команды шахматного поединка: диапазон 126 - 145(20)
        BattleMessage              = 127,
        DownloadBattlefildRequest  = 128,
        DownloadBattlefildResponse = 129,
        MoveUnitRequest            = 130,
        MoveUnitResponse           = 131,
        
        // команды управлениями союзами и империями: 146 - 205 (50)
        //
        JoinRequestMessage              = 149,
        EmbedTaxRateRequest             = 150,
        EmbedTaxRateResponse            = 151,
        ExcludeKingFromEmpireRequest    = 152,
        ExcludeKingFromEmpireResponse   = 153,
        IncludeKingInEmpireRequest      = 154,
        IncludeKingInEmpireResponse     = 155,
        ExitFromAlianceRequest          = 156,
        ExitFromAlianceResponse         = 157,
        GetHelpFigureRequest            = 158,
        GetHelpFigureResponse           = 159,
        GetHelpResourceRequest          = 160,
        GetHelpResourceResponse         = 161,
        GetAlianceInfoRequest           = 162,
        GetAlianceInfoResponse          = 163,
        GrandLeaderPrivilegesMessage    = 164,
        TakeAwayLeaderPrivilegesMessage = 165,
        JoinToAlianceRequest            = 166,
        JoinToAlianceResponse           = 167,
        MessageNewsMessage              = 168,
        SendFigureHelpMessage           = 169,
        SendResourceHelpMessage         = 170,
        StartVoteRequest                = 171,
        StartVoteResponse               = 172,
        StartImpeachmentRequest         = 173,
        StartImpeachmentResponse        = 174,
        VoteBallotMessage               = 175,
        GetAliancesInfoRequest          = 176,
        GetAliancesInfoResponse         = 177,
        ExcludeFromEmpireMessage        = 178,
        StartNegotiateRequest           = 179,
        StartNegotiateResponse          = 180,

        // команды торговли:           ?

        // команды статистики: 206 - 225(20)
        GetStatisticRequest             = 206,
        GetStatisticResponse            = 207,
        GetAvailableMapsRequest         = 208,
        GetAvailableMapsResponse        = 209,

        // --------------------- Сектор команд чата (250 - 280)----------------------------------------- //

        // команды чата
        JoinLeaveCommand                = 250,
        MessageCommand                  = 251,
        MessageReceiveCommand           = 252,
        SendContactListCommand          = 253,

        ErrorMessage                    = 666
    }
}
