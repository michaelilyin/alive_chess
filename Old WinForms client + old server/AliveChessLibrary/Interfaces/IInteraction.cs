using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;

namespace AliveChessLibrary.Interfaces
{
    public interface IInteraction
    {
        int Id
        { get; set; }

        King Organizator
        { get; set; }

        King Respondent
        { get; set; }

        bool YouStep
        { get; set; }

        TimeSpan Elapsed
        { get; set; }

        InteractionType InteractionType
        { get; }
    }
}
