namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public struct Tax
    {
        private int _gold;
        private int _wood;
        private int _iron;
        private int _stone;
        private int _coal;

        public int Iron
        {
            get { return _iron; }
            set { _iron = value; }
        }

        public int Stone
        {
            get { return _stone; }
            set { _stone = value; }
        }

        public int Coal
        {
            get { return _coal; }
            set { _coal = value; }
        }

        public int Wood
        {
            get { return _wood; }
            set { _wood = value; }
        }

        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }
    }
}
