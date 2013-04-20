using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class DownloadBattlefieldRequest : ICommand
    {
        [ProtoMember(1)]
        private uint opponent;

        public uint Opponent
        {
            get { return opponent; }
            set { opponent = value; }
        }

        public Command Id
        {
            get { return Command.DownloadBattlefildRequest;}
        }

     }

}
