namespace AliveChessServer.LogicLayer.RequestExecutors
{
    public enum ExecutorTypes
    {
        AuthorizeRequestExecutor           = 0,
        GetMapRequestExecutor              = 1,
        GetObjectsRequestExecutor          = 2,
        MoveKingRequestExecutor            = 3,
        GetResourceRequestExecutor         = 4,
        BigMapRequestExecutor              = 5,
        CaptureCastleRequestExecutor       = 6,
        CaptureMineRequestExecutor         = 7,
        ComeInCastleRequestExecutor        = 8,
        ContactCastleRequestExecutor       = 9,
        ContactKingRequestExecutor         = 10,
        LeaveCastleRequestExecutor         = 11,
        ExitFromGameRequestExecutor        = 12,

        BattleDialogMessageExecutor        = 13,
        CapitulateDialogMessageExecutor    = 14,
        CaptureCastleDialogMessageExecutor = 15,
        PayOffDialogMessageExecutor        = 16,
        CreateUnionDialogMessageExecutor   = 17,
        MarketDialogMessageExecutor        = 18,
        RegisterRequestExecutor            = 19,

        //slisarenko
        GetRecBuildingsRequestExecutor = 20,
        BuildingInCastleRequestExecutor = 21,
        ShowArmyCastleRequestExecutor = 22,
        BuyFigureRequestExecutor = 23,
        GetArmyCastleToKingRequestExecutor = 24,
        GetInnerBuildingsRequestExecutor = 25,
        GetListBuildingsInCastleRequestExecutor = 26,
        ShowArmyKingRequestExecutor = 27,

        DownloadBattlefildRequestExecutor = 38,
        MoveUnitRequestExecutor           = 39,

        GetAlianceInfoRequestExecutor        = 40,
        EmbedTaxRateRequestExecutor          = 41,
        ExcludeKingFromEmpireRequestExecutor = 42,
        GetHelpFigureRequestExecutor         = 43,
        GetHelpResourceRequestExecutor       = 44,
        IncludeKingInEmpireRequestExecutor   = 45,
        JoinToAlianceRequestExecutor         = 46,
        SendFigureHelpMessageExecutor        = 47,
        SendResourceHelpMessageExecutor      = 48,
        StartImpeachmentRequestExecutor      = 49,
        StartVoteRequestExecutor             = 50,
        GetAliancesInfoRequestExecutor       = 51,
        VoteBallotMessageExecutor            = 52,
        ExitFromAlianceRequestExecutor       = 53,
        StartNegotiateRequestExecutor        = 54,

        WarDialogMessageExecutor             = 55,
        PeaceDialogMessageExecutor           = 56,
        JoinEmperiesDialogMessageExecutor    = 57,

        GetStatisticRequestExecutor          = 58,
        GetUnityMapRequestExecutor           = 59,
        GetGameStateRequestExecutor          = 60,
        GetAvailableMapsRequestExecutor      = 61,
        VerifyPathRequestExecutor            = 62,
        GetKingRequestExecutor               = 63,
    }
}
