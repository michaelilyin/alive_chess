using System;
using System.Diagnostics;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors
{
    public delegate void MessageExecutorHandler();

    public class MessageExecutorTemplate<T> where T : IMessage
    {
        private Level _level;
        private Player _sender;
        private IPlayer _receiver;
        private King _kingReceiver;
        private T _command;
        private IDispute _dispute;

        private GameLogic _logic;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public MessageExecutorTemplate(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._logic = gameLogic;
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

            //Debug.Assert(_environment.DisputeRoutine != null);
        }

        public void Proceed(MessageExecutorHandler deleg, Message msg)
        {
            _command = (T)msg.Command;

            // получаем игрока
            _sender = msg.Sender;

            if (_sender != null)
            {
                // получаем уровень
                _level = _sender.Level as Level;
                // получаем контекст переговоров
                _dispute = _level.DisputeRoutine.GetDisputeById(_command.DisputeId);
                if (_dispute != null)
                {
                    // получаем короля - соперника
                    _kingReceiver = _level.DisputeRoutine.GetOpponent(_dispute, _sender.King);
                    // получаем контекст игрока, управляющего королем - соперником
                    _receiver = _kingReceiver.Player;

                    if (_receiver != null)
                    {
                        // соблюдаем очередность ходов
                        _dispute.YouStep = !_dispute.YouStep;

                        // обнуляем время простоя
                        _dispute.Elapsed = TimeSpan.Zero;

                        deleg.Invoke();
                    }
                    else Debug.Fail("Receiver is null");
                }
                else Debug.Fail("Dispute is null");
            }
            else Debug.Fail("Sender is null");
        }

        public Level Level
        {
            get { return _level; }
        }

        public IDispute Dispute
        {
            get { return _dispute; }
        }

        public Player Sender
        {
            get { return _sender; }
        }

        public IPlayer Receiver
        {
            get { return _receiver; }
        }

        public King KingSender
        {
            get { return _sender.King; }
        }

        public King KingReceiver
        {
            get { return _kingReceiver; }
        }

        public T Command
        {
            get { return _command; }
        }
    }
}
