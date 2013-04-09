using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос карты для Unity
    /// </summary>
    [ProtoContract]
    public class GetUnityMapRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetUnityMapRequest; }
        }

        [ProtoMember(1)]
        public string Name { get; set; }
    }
}
