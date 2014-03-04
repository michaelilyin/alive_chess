using AliveChessLibrary.Commands;
using Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Network
{
    internal class RequestHandler
    {
        private readonly BackgroundWorker _worker;
        private bool _running = false;
        private readonly CommandPool _commands;
        private readonly Dictionary<Command, CommandHandler> _handlers;

        public RequestHandler(CommandPool commands)
        {
            _commands = commands;
            _worker = new BackgroundWorker();
            _handlers = new Dictionary<Command, CommandHandler>();
            _worker.DoWork += new DoWorkEventHandler(DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecutionStopped);
        }

        public void RegisterHandler(Command command, CommandHandler handler)
        {
            _handlers[command] = handler;
        }

        public void Start()
        {
            _running = true;
            _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            _running = false;
            _worker.Dispose();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Log.Message("Start handle server messages");
            while (_running)
            {
                //Log.Message(String.Format("Worekr: In queue {0} commands", _commands.Count));
                if (_commands.Count > 0)
                {
                    ICommand command = _commands.Dequeue();
                    //Log.Message("I was get the command!");
                    if (command != null)
                    {
                        //Log.Message("Command is not null!");
                        //Log.Message(String.Format("It is {0} command!", command.Id));
                        if (_handlers.ContainsKey(command.Id))
                        {
                            try
                            {
                                _handlers[command.Id].Handle(command);
                            } 
                            catch (Exception ex)
                            {
                                Log.Error(ex);
                            }
                        }
                        else
                        {
                            Log.Error(String.Format("Handler for {0} command is not exists", command.Id));
                        }
                        //Thread.Sleep(5);
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
            Log.Message("Stop handle server messages");
        }
    }
}
