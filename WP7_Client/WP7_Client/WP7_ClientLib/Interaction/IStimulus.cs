using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Interaction
{
    public interface IStimulus
    {
        King Receiver { get; set; }

        King Sender { get; set; }

        StimulusType Type { get; set; }
    }
}
