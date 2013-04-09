using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using WP7_Client.LogicLayer.Executors;
using WP7_Client.NetLayer;
using AliveChessLibrary.Commands;
using WP7_Client.PresentationLayer.ConcreteScreens;

namespace WP7_Client.LogicLayer
{
    public class RequestExecutor
    {
        private readonly BackgroundWorker _executeWorker;
        private readonly CommandPool _commands;
        private readonly Dictionary<Command, IExecutor> _executors;

        public Dictionary<Command, IExecutor> Executors
        {
            get { return _executors; }
        }

        public RequestExecutor(CommandPool commands)
        {
            _commands = commands;
            _executeWorker = new BackgroundWorker();
            _executors = new Dictionary<Command, IExecutor>();
            _executeWorker.DoWork += Execute;

            CreateInitialExecutors();
        }

        public void Start()
        {
            if (!_executeWorker.IsBusy) _executeWorker.RunWorkerAsync();
        }

        private void Execute(object sender, DoWorkEventArgs e)
        {
            while (((App)Application.Current).Transport.Connected())
            {
                if (_commands.Count > 0)
                {
                    var command = _commands.Dequeue();
                    _executors[command.Id].Execute(command);
                    Thread.Sleep(5);
                }
                Thread.Sleep(5);
            }
        }

        public void CreateInitialExecutors()
        {
            _executors.Add(Command.AuthorizeResponse, new AuthorizeExecutor());
            _executors.Add(Command.RegisterResponse, new RegisterExecutor());
            _executors.Add(Command.GetMapResponse, new GetMapExecutor());
            _executors.Add(Command.ErrorMessage, new ErrorMessageExecutor());
        }

        public void CreateBigMapExecutors(BigMapScreen screen)
        {
            _executors.Add(Command.MoveKingResponse, new MoveKingExecutor(screen));
            _executors.Add(Command.GetGameStateResponse, new GameStateExecutor(screen));
            _executors.Add(Command.GetObjectsResponse, new GetObjectsExecutor(screen));
            _executors.Add(Command.GetResourceMessage, new ResourceMessageExecutor(screen));
        }
    }
}
