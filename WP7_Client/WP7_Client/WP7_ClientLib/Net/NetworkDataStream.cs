using System;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.Commands;

namespace AliveChessLibrary.Net
{
    public class NetworkDataStream
    {
        private byte[] _buffer = null;
        private int _bufferCapacity = 15000;

        private int _bufferSize = 0;

        private int _commandSize = 0;
        private int _commandName = 0;

        private readonly byte[] _sizeArr = new byte[4];
        private readonly byte[] _nameArr = new byte[4];

        private bool _dataIsReady = false;

        private readonly object _syncBuffer = new object();

        public NetworkDataStream()
        {
            Initialize();
            this._buffer = new byte[_bufferCapacity];
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
            ChatUpLimit = 280;
            ErrorDownLimit = 666;
            ErrorUpLimit = 666;
        }

        public void Write(byte[] data)
        {
            if (_bufferCapacity < _bufferSize + data.Length)
                Resize(_bufferSize + data.Length);

            Monitor.Enter(_syncBuffer);
            Array.Copy(data, 0, _buffer, _bufferSize, data.Length);
            _bufferSize += data.Length;
            Monitor.Exit(_syncBuffer);
            _dataIsReady = true;
        }

        public void Clear()
        {
            Monitor.Enter(_syncBuffer);
            Array.Clear(_buffer, 0, _bufferSize);
            Monitor.Exit(_syncBuffer);
            _bufferSize = 0;
            _dataIsReady = false;
        }

        private void Resize(int newSize)
        {
            Monitor.Enter(_syncBuffer);
            Array.Resize<byte>(ref _buffer, newSize);
            Monitor.Exit(_syncBuffer);
            _bufferCapacity = _buffer.Length;
        }

        public BytePackage Read()
        {
            BytePackage bp = null;
            // Start synchronization block
            Monitor.Enter(_syncBuffer);
            // Copy command's index to buffer
            Array.Copy(_buffer, 0, _nameArr, 0, 4);
            // Copy command's size to buffer
            Array.Copy(_buffer, 4, _sizeArr, 0, 4);
            // Convert command's index is given in a form of bytes
            _commandSize = BitConverter.ToInt32(_sizeArr, 0);
            // Convert command's size is given in the form of bytes
            _commandName = BitConverter.ToInt32(_nameArr, 0);
            // Buffer has enough capacity
            if (_bufferSize - 8 >= _commandSize)
            {
                // Initialize new package
                bp = new BytePackage();
                // Recognize command
                bp.CommandType = RecognizeCommand(bp).ToString();

                Debug.Assert(bp.CommandType != string.Empty);
                // Set command's name by index such as MoveKingRequest, MoveKingResponse, etc
                bp.CommandName = ((Command)_commandName).ToString();
                // Set size of command's part which resposible for data is decoded by Protobuf 
                bp.CommandSize = _commandSize;
                // Initialize array
                byte[] bytes = new byte[_commandSize];
                // Copy received data to new array
                Array.Copy(_buffer, 8, bytes, 0, _commandSize);
                // decrease buffer's size
                _bufferSize = _bufferSize - _commandSize - 8;
                // Shift bytes in source byffer
                for (int i = 0; i < _bufferSize; i++)
                    _buffer[i] = _buffer[i + _commandSize + 8];
                // Clear bytes were copied to destination array
                Array.Clear(_buffer, _bufferSize, _commandSize + 8);
                // Set command body (Protobuf)
                bp.CommandBody = bytes;
                // Receiving isn't completed
                if (_bufferSize == 0) _dataIsReady = false;
            }
            Monitor.Exit(_syncBuffer);
            return bp;
        }

        public bool IsReady
        {
            get { return _dataIsReady; }
        }

        private CommandType RecognizeCommand(BytePackage bp)
        {
            if (IsValueInRange(_commandName,
                RegDownLimit, RegUpLimit))
                return CommandType.RegisterCommand;
            if (IsValueInRange(_commandName,
                BigMapDownLimit, BigMapUpLimit))
                return CommandType.BigMapCommand;
            if (IsValueInRange(_commandName,
                DialogDownLimit, DialogUpLimit))
                return CommandType.DialogCommand;
            if (IsValueInRange(_commandName,
                CastleDownLimit, CastleUpLimit))
                return CommandType.CastleCommand;
            if (IsValueInRange(_commandName,
                BattleDownLimit, BattleUpLimit))
                return CommandType.BattleCommand;
            if (IsValueInRange(_commandName,
                EmpireDownLimit, EmpireUpLimit))
                return CommandType.EmpireCommand;
            if (IsValueInRange(_commandName,
                StatisticDownLimit, StatisticUpLimit))
                return CommandType.StatisticCommand;
            if (IsValueInRange(_commandName,
                ErrorDownLimit, ErrorUpLimit))
                return CommandType.ErrorCommand;
            if (IsValueInRange(_commandName,
               ChatDownLimit, ChatUpLimit))
                return CommandType.ChatCommand;

            return CommandType.None;
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
        public int ErrorDownLimit { get; private set; }
        public int ErrorUpLimit { get; private set; }

        private static bool IsValueInRange(int value, int down, int up)
        {
            return value >= down && value <= up;
        }

        public bool IsProtoduf
        {
            get
            {
                return IsValueInRange(_commandName, RegDownLimit, StatisticUpLimit)
                    || IsValueInRange(_commandName, ErrorDownLimit, ErrorUpLimit);
            }
        }

        public bool IsChat
        {
            get { return IsValueInRange(_commandName, ChatDownLimit, ChatUpLimit); }
        }
    }
}
