using AliveChessLibrary.Commands;
using Assets.GameLogic.CommandHandlers;
using Assets.GameLogic.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.GameLogic
{
    class RequestExecutor
    {
        private readonly BackgroundWorker _worker;
        private bool _running = false;
        private readonly CommandPool _commands;
        private readonly Dictionary<Command, CommandHandler> _handlers;

        public RequestExecutor(CommandPool commands)
        {
            _commands = commands;
            _worker = new BackgroundWorker();
            _handlers = new Dictionary<Command, CommandHandler>();
            CreateHandlers();
            _worker.DoWork += new DoWorkEventHandler(DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecutionStopped);
        }

        public void Start()
        {
            _running = true;
            _worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            while (_running)
            {
                Debug.Log(String.Format("Worekr: In queue {0} commands", _commands.Count));
                if (_commands.Count > 0)
                {
                    ICommand command = _commands.Dequeue();
                    Debug.Log("I was get the command!");
                    if (command != null)
                    {
                        Debug.Log("Command is not null!");
                        Debug.Log(String.Format("It is {0} command!", command.Id));
                        _handlers[command.Id].Handle(command);
                        Thread.Sleep(5); 
                    }
                }
                else
                {
                    _commands.Wait();
                }
            }
        }

        private void ExecutionStopped(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void CreateHandlers()
        {
            _handlers.Add(Command.AuthorizeResponse, new AuthorizeResponseHandler());
            _handlers.Add(Command.GetMapResponse, new GetMapHandler());
            _handlers.Add(Command.GetGameStateResponse, new GetGameStateHandler());
            _handlers.Add(Command.MoveKingResponse, new MoveKingResponseHandler());
            _handlers.Add(Command.GetResourceMessage, new GetResourceHandler());
            _handlers.Add(Command.CaptureMineResponse, new CaptureMineHandler());
        }
    }
}
