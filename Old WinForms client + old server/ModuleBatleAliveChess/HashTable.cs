namespace ModuleBatleAliveChess
{
    class HashTable
    {
        GlobalC Gl = new GlobalC();
        private struct HashItem
        {
            public ulong key;
            public int res;
            public byte depth;// глубина
            //public byte  from;
            //public byte  to;
            //public byte  promotion;
            public Move move;
        };

        private HashItem[] table;


        public HashTable()
        {
            Size(32);
        }


        //TODO: Make it return the memory// сделать вывод памяти
        public void Size(uint size)
        {
            table = null;
            if (size == 0)
            {
                table = new HashItem[1];
            }
            else
            {
                table = new HashItem[size * 1024 * 1024 / 16];
            }
            Clear();
        }


        public void Clear()
        {
            for (int pos = 0; pos < table.Length; pos++)
            {
                table[pos].key = 0;
                table[pos].depth = 0;
            }
        }


        public void SetItem(ulong key, int res, byte depth, Move move)
        {
            int pos = (int)(key % (ulong)table.Length);

            if (table[pos].key == key && depth <= table[pos].depth) return;

            table[pos].key = key;
            table[pos].res = res;
            table[pos].depth = depth;
            table[pos].move = move;
        }


        public bool GetItem(ulong key, out int res, out byte depth, out Move move)
        {
            int pos = (int)(key % (ulong)table.Length);
            if (table[pos].key == key)
            {
                res = table[pos].res;
                depth = table[pos].depth;
                move = table[pos].move;
                return true;
            }
            else
            {
                res = Gl.Defeat;
                depth = 0;
                move = new Move();
                move.from = move.to = move.promote = 64;
                return false;
            }
        }

        public bool GetHashResult(ulong key, byte depth, out int res)
        {
            int pos = (int)(key % (ulong)table.Length);
            if (table[pos].key == key && table[pos].depth == depth)
            {
                res = table[pos].res;
                return true;
            }
            else
            {
                res = 0;
                return false;
            }
        }

        public void SetHashResult(ulong key, int res, byte depth)
        {
            int pos = (int)(key % (ulong)table.Length);

            if (table[pos].key != 0 || depth > table[pos].depth)
            {
                table[pos].key = key;
                table[pos].res = res;
                table[pos].depth = depth;
            }
        }

    }
}
