using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class DownloadBattlefildResponse : ICommand
    {
        public Command Id
        {
            get { return Command.DownloadBattlefildResponse; }
        }

        [ProtoMember(1)]
        public Battle Battle { get; set; }
    }
}
