using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class ResBuild
    {
        [ProtoMember(1)]
        private int _coal = 1; // уголь

        [ProtoMember(2)]
        private int _gold = 1; // золото

        [ProtoMember(3)]
        private int _iron = 1; // железо

        [ProtoMember(4)]
        private int _stone = 1;// камень

        [ProtoMember(5)]
        private int _wood = 1; // дерево


        public int Wood
        {
            get { return _wood; }
            set { _wood = value; }
        }

        public int Coal
        {
            get { return _coal; }
            set { _coal = value; }
        }

        public int Stone
        {
            get { return _stone; }
            set { _stone = value; }
        }

        public int Iron
        {
            get { return _iron; }
            set { _iron = value; }
        }

        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }
    }
}
