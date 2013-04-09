namespace AliveChessLibrary.Interaction
{
    public enum NewsType
    {
        No                              = 0,
        HelpFigure                      = 1,  // игрок запросил фигуры
        HelpResource                    = 2,  // игрок запросил ресурсы

        PlayerWantJoinToAliance         = 3,  //
        PlayerJoinedToAliance           = 4,  // игрок присоединился к союзу
        PlayerLeaveAliance              = 5,  // игрок вышел из союза
        PlayerExcludedFromEmpire        = 6,  // игрок выгнан из союза

        HelpFigureSended                = 7,  //
        HelpResourceSended              = 8,  //

        VoteStarted                     = 9,  // начало голосования
        VoteEndedResultPublished        = 10, // окончание голосования и объявление результатов

        ImpeachmentStarted              = 11, // начало импичмента
        ImpeachmentEndedResultPublished = 12, // окончание импичмента и объявление результатов

        LeaderEnterInGame               = 13, //
        LeaderExitFromGame              = 14, //

        PlayerEnterInGame               = 15, //
        PlayerExitFromGame              = 16, //

        NewTaxEmbeded                   = 17, // установлен новый налог

        ChangeAlianceStatus             = 18, // союз превратился в империю либо наоборот
    }
}
