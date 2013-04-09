using System;
using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public interface IAlliance : ICommunity
    {
        Level Level { get; set; }

        int? LevelId { get; set; }

        AllianceStatus Status { get; }

        BallotBox BallotBox { get; }

        IEnumerable<King> NextMember();

        void StartVote(King candidate);

        void FinishVote();

        void DoLogic(GameTime time);

        void AddMember(King king);

        void RemoveMember(King king);

        void PublishNews(NewsType type, string message);

        void PublishNews(NewsType type, string message, 
            Func<King, bool> predicate);

        void PublishNews(Player sender, NewsType type, string message);

        void PublishNews(Player sender, NewsType type, string message, 
            Func<King, bool> predicate);

        TimeSpan TimeSinceVoteStarting { get; set; }

        bool AllowStartVote { get; }

        EntitySet<King> Kings { get; set; }

        bool IsVoteInProgress { get; set; }
    }
}
