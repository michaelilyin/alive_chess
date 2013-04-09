using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class DownloadBattlefieldRequest : ICommand
    {
        [ProtoMember(1)]
        public int Opponent { get; set; }

        public Command Id
        {
            get { return Command.DownloadBattlefildRequest;}
        }

     }

}
