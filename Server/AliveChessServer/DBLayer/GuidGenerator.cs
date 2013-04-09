using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AliveChessLibrary.Utility;

namespace AliveChessServer.DBLayer
{
    public class GuidGenerator
    {
        private int _sequenceIndex;
        private static GuidGenerator _instance;
        private SortedDictionary<Guid, int> _entities;

        private GuidGenerator()
        {
            _sequenceIndex = 1;
            _entities = new SortedDictionary<Guid, int>();
        }

        public Guid Generate()
        {
            return Guid.NewGuid();
        }

        public GuidIDPair GeneratePair()
        {
            GuidIDPair pair = new GuidIDPair();
            pair.Guid = Guid.NewGuid();
            pair.Id = GetInt();
            return pair;
        }

        public int GetInt()
        {
            // convert current time to binary
            byte[] time = BitConverter.GetBytes(DateTime.Now.ToBinary());

            BitVector32 bv = new BitVector32();
            // The maximum server number is 15
            BitVector32.Section numServer = BitVector32.CreateSection(15);
            // The maximum thread number is 15
            BitVector32.Section numThread = BitVector32.CreateSection(15, numServer);

            bv[numServer] = 15;
            bv[numThread] = 15;

            byte[] num = { (byte)bv.Data }; // combination of thread and server number
            byte[] bb = new byte[8]; // binary representation of result

            Array.Copy(num, 0, bb, 0, 1);
            Array.Copy(time, 0, bb, 1, 7);
           
            //return BitConverter.ToUInt32(bb, 0);

            return _sequenceIndex++;
        }

        public int SequenceIndex
        {
            get { return _sequenceIndex; }
        }

        public static GuidGenerator Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = new GuidGenerator();
                return _instance; 
            }
        }
    }
}
