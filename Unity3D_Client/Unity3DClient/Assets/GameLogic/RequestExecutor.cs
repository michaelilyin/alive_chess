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
                if (_commands.Count > 0)
                {
                    Debug.Log("Has command in queue!");
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
            _handlers.Add(Command.AuthorizeResponse, new AuthorizeResponseHandler(Command.AuthorizeRequest));
        }
    }
}
