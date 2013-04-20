using System;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.Commands;

namespace AliveChessLibrary.Net
{
    public class NerworkDataStream
    {
        private byte[] buffer = null;
        private int bufferCapacity = 15000;

        private int bufferSize = 0;

        private int commandSize = 0;
        private int commandName = 0;

        private byte[] sizeArr = new byte[4];
        private byte[] nameArr = new byte[4];

        private bool dataIsReady = false;

        private object syncBuffer = new object();

        private const int REG_DOWN_LIMIT     = 0;
        private const int REG_UP_LIMIT       = 5;

        private const int BIGMAP_DOWN_LIMIT  = 6;
        private const int BIGMAP_UP_LIMIT    = 55;

        private const int DIALOG_DOWN_LIMIT  = 56;
        private const int DIALOG_UP_LIMIT    = 75;

        private const int CASTLE_DOWN_LIMIT  = 76;
        private const int CASTLE_UP_LIMIT    = 126;

        private const int BATTLE_DOWN_LIMIT  = 127;
        private const int BATTLE_UP_LIMIT    = 147;

        private const int EMPIRE_DOWN_LIMIT  = 148;
        private const int EMPIRE_UP_LIMIT    = 199;

        public NerworkDataStream()
        {
            this.buffer = new byte[bufferCapacity];
        }

        public void Put(byte[] data)
        {
            if (bufferCapacity < bufferSize + data.Length)
                Resize(bufferSize + data.Length);

            Monitor.Enter(syncBuffer);
            Array.Copy(data, 0, buffer, bufferSize, data.Length);
            bufferSize += data.Length;
            Monitor.Exit(syncBuffer);

            dataIsReady = true;
        }

        private void Resize(int newSize)
        {
            Monitor.Enter(syncBuffer);
            Array.Resize<byte>(ref buffer, newSize);
            Monitor.Exit(syncBuffer);
            bufferCapacity = buffer.Length;
        }

        public BytePackage Get()
        {
            BytePackage bp = null;
            Monitor.Enter(syncBuffer);
            Array.Copy(buffer, 0, nameArr, 0, 4);
            Array.Copy(buffer, 4, sizeArr, 0, 4);
            commandSize = BitConverter.ToInt32(sizeArr, 0);
            commandName = BitConverter.ToInt32(nameArr, 0);
            if (bufferSize - 8 >= commandSize)
            {
                bp = new BytePackage();

                // ----------- Test ----------------- //
                if (commandName == 200)
                    bp.CommandType = "TEST";
                // ---------------------------------- //

                if (commandName >= REG_DOWN_LIMIT && commandName <= REG_UP_LIMIT)
                    bp.CommandType = CommandType.RegisterCommand.ToString();
                if (commandName >= BIGMAP_DOWN_LIMIT && commandName <= BIGMAP_UP_LIMIT)
                    bp.CommandType = CommandType.BigMapCommand.ToString();
                if (commandName >= DIALOG_DOWN_LIMIT && commandName <= DIALOG_UP_LIMIT)
                    bp.CommandType = CommandType.DialogCommand.ToString();
                if (commandName >= CASTLE_DOWN_LIMIT && commandName <= CASTLE_UP_LIMIT)
                    bp.CommandType = CommandType.CastleCommand.ToString();
                if (commandName >= BATTLE_DOWN_LIMIT && commandName <= BATTLE_UP_LIMIT)
                    bp.CommandType = CommandType.BattleCommand.ToString();
                if (commandName >= EMPIRE_DOWN_LIMIT && commandName <= EMPIRE_UP_LIMIT)
                    bp.CommandType = CommandType.EmpireCommand.ToString();

                Debug.Assert(bp.CommandType != string.Empty);

                bp.CommandName = ((Command)commandName).ToString(); 
                bp.CommandSize = commandSize;
                byte[] bytes = new byte[commandSize];
                Array.Copy(buffer, 8, bytes, 0, commandSize);
                bufferSize = bufferSize - commandSize - 8;
                for (int i = 0; i < bufferSize; i++)
                    buffer[i] = buffer[i + commandSize + 8];
                Array.Clear(buffer, bufferSize, commandSize + 8);
                bp.CommandBody = bytes;
                if (bufferSize == 0) dataIsReady = false;
            }
            Monitor.Exit(syncBuffer);
            return bp;
        }

        public bool IsReady
        {
            get { return dataIsReady; }
        }
    }
}
