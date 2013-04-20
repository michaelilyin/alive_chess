using System;

namespace ModuleBatleAliveChess
{
    class History
    {
        struct HistoryItem
        {
            public ulong key;
            public byte count;
        };

        private HistoryItem[] table;


        public History()
        {
            //Table size must be prime and 2x maximum entries to garantee insert
            table = new HistoryItem[1009];
            Clear();
        }


        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i].key = 0;
                table[i].count = 0;
            }
        }


        /// <summary>
        /// Returns false on third repetion.
        /// </summary>
        /// <param name="key"></param>
        public bool Increase(ulong key)
        {
            int probe = 0;

            while (true)
            {
                int pos = (int)((key + (ulong)(probe * probe)) % (ulong)table.Length);

                if (table[pos].key == 0)
                {
                    table[pos].key = key;
                    table[pos].count = 1;
                    return true;
                }

                if (table[pos].key == key)
                {
                    table[pos].count++;
                    if (table[pos].count == 3)
                        return false;
                    else
                        return true;
                }
                else
                {
                    probe++;
                }
            }
        }


        public void Decrease(ulong key)
        {
            int probe = 0;

            while (true)
            {
                int pos = (int)((key + (ulong)(probe * probe)) % (ulong)table.Length);

                if (table[pos].key == 0)
                {
                    throw new ArgumentException("Decrease of non existent key!");
                }

                if (table[pos].key == key)
                {
                    table[pos].count--;
                    if (table[pos].count == 0) table[pos].key = 0;
                    return;
                }
                else
                {
                    probe++;
                }
            }
        }

    }
}
