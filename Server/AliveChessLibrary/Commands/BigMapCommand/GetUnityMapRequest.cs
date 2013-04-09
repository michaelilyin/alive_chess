using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос карты для Unity
    /// </summary>
    [ProtoContract]
    public class GetUnityMapRequest : ICommand
    {
        [ProtoMember(1)]
        private string name;

        public Command Id
        {
            get { return Command.GetUnityMapRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
