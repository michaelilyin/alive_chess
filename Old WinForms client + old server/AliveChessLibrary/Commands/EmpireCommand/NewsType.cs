using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    public enum NewsType
    {
        HelpFigure,
        HelpResource,

        PlayerWantJoinToAliance,
        PlayerJoinedToAliance,
        PlayerLeaveAliance,
        PlayerExcludedFromEmpire,

        HelpFigureSended,
        HelpResourceSended,

        VoteStarted,
        VoteEndedResultPublished,

        ImpeachmentStarted,
        ImpeachmentEndedResultPublished,

        LeaderEnterInGame,
        LeaderExitFromGame,

        PlayerEnterInGame,
        PlayerExitFromGame,

        NewTaxEmbeded,

        ChangeAlianceStatus,
    }
}
