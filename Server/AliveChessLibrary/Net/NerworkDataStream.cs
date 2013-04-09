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

        public NerworkDataStream()
        {
            Initialize();
            this.buffer = new byte[bufferCapacity];
        }

        private void Initialize()
        {
            RegDownLimit = 0;
            RegUpLimit = 5;
            BigMapDownLimit = 6;
            BigMapUpLimit = 55;
            DialogDownLimit = 56;
            DialogUpLimit = 75;
            CastleDownLimit = 76;
            CastleUpLimit = 125;
            BattleDownLimit = 126;
            BattleUpLimit = 145;
            EmpireDownLimit = 146;
            EmpireUpLimit = 205;
            StatisticDownLimit = 206;
            StatisticUpLimit = 215;
            ChatDownLimit = 250;
            ChatUpLimit = 250;
        }

        public void Write(byte[] data)
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

        public BytePackage Read()
        {
            BytePackage bp = null;
            // Start synchronization block
            Monitor.Enter(syncBuffer);
            // Copy command's index to buffer
            Array.Copy(buffer, 0, nameArr, 0, 4);
            // Copy command's size to buffer
            Array.Copy(buffer, 4, sizeArr, 0, 4);
            // Convert command's index is given in a form of bytes
            commandSize = BitConverter.ToInt32(sizeArr, 0);
            // Convert command's size is given in the form of bytes
            commandName = BitConverter.ToInt32(nameArr, 0);
            // Buffer has enough capacity
            if (bufferSize - 8 >= commandSize)
            {
                // Initialize new package
                bp = new BytePackage();
                // Recognize command
                RecognizeCommand(bp);
             
                Debug.Assert(bp.CommandType != string.Empty);
                // Set command's name by index such as MoveKingRequest, MoveKingResponse, etc
                bp.CommandName = ((Command)commandName).ToString(); 
                // Set size of command's part which resposible for data is decoded by Protobuf 
                bp.CommandSize = commandSize;
                // Initialize array
                byte[] bytes = new byte[commandSize];
                // Copy received data to new array
                Array.Copy(buffer, 8, bytes, 0, commandSize);
                // decrease buffer's size
                bufferSize = bufferSize - commandSize - 8;
                // Shift bytes in source byffer
                for (int i = 0; i < bufferSize; i++)
                    buffer[i] = buffer[i + commandSize + 8];
                // Clear bytes were copied to destination array
                Array.Clear(buffer, bufferSize, commandSize + 8);
                // Set command body (Protobuf)
                bp.CommandBody = bytes;
                // Receiving isn't completed
                if (bufferSize == 0) dataIsReady = false;
            }
            Monitor.Exit(syncBuffer);
            return bp;
        }

        public bool IsReady
        {
            get { return dataIsReady; }
        }

        private void RecognizeCommand(BytePackage bp)
        {
            // ----------- Test ----------------- //
            if (commandName == 200)
                bp.CommandType = "TEST";
            // ---------------------------------- //

            if (IsValueInRange(commandName, RegDownLimit, RegUpLimit))
                bp.CommandType = CommandType.RegisterCommand.ToString();
            if (IsValueInRange(commandName, BigMapDownLimit, BigMapUpLimit))
                bp.CommandType = CommandType.BigMapCommand.ToString();
            if (IsValueInRange(commandName, DialogDownLimit, DialogUpLimit))
                bp.CommandType = CommandType.DialogCommand.ToString();
            if (IsValueInRange(commandName, CastleDownLimit, CastleUpLimit))
                bp.CommandType = CommandType.CastleCommand.ToString();
            if (IsValueInRange(commandName, BattleDownLimit, BattleUpLimit))
                bp.CommandType = CommandType.BattleCommand.ToString();
            if (IsValueInRange(commandName, EmpireDownLimit, EmpireUpLimit))
                bp.CommandType = CommandType.EmpireCommand.ToString();
        }

        public int RegDownLimit { get; private set; }
        public int RegUpLimit { get; private set; }
        public int BigMapDownLimit { get; private set; }
        public int BigMapUpLimit { get; private set; }
        public int DialogDownLimit { get; private set; }
        public int DialogUpLimit { get; private set; }
        public int CastleDownLimit { get; private set; }
        public int CastleUpLimit { get; private set; }
        public int BattleDownLimit { get; private set; }
        public int BattleUpLimit { get; private set; }
        public int EmpireDownLimit { get; private set; }
        public int EmpireUpLimit { get; private set; }
        public int ChatDownLimit { get; private set; }
        public int ChatUpLimit { get; private set; }
        public int StatisticDownLimit { get; private set; }
        public int StatisticUpLimit { get; private set; }

        private bool IsValueInRange(int value, int down, int up)
        {
            return value >= down && value <= up;
        }

        public bool IsProtoduf
        {
            get { return IsValueInRange(commandName, RegDownLimit, StatisticUpLimit); }
        }

        public bool IsChat
        {
            get { return IsValueInRange(commandName, ChatDownLimit, ChatUpLimit); }
        }
    }
}
