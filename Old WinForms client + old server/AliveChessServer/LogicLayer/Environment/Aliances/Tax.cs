namespace AliveChessServer.LogicLayer.Environment.Aliances
{
    public struct Tax
    {
        private int _gold;
        private int _wood;

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
