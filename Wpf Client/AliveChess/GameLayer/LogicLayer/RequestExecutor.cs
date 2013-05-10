using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using AliveChess.GameLayer.LogicLayer.Executors;
using AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors;
using AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors;
using AliveChess.NetworkLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Interfaces;

namespace AliveChess.GameLayer.LogicLayer
{
    public class RequestExecutor
    {
        private ILogger _logger;
        private readonly BackgroundWorker _executeWorker;
        private bool _running = false;
        private readonly CommandPool _commands;
        private readonly Dictionary<Command, IExecutor> _executors;

        public RequestExecutor(ILogger logger, CommandPool commands)
        {
            _logger = logger;
            _commands = commands;
            _executeWorker = new BackgroundWorker();
            _executors = new Dictionary<Command, IExecutor>();
            _executeWorker.DoWork += new DoWorkEventHandler(Execute);
            _executeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecutionStopped);

            CreateAuthorizeExecutors();
            CreateBigMapExecutors();
            CreateCastleExecutors();
            CreateErrorExecutors();
        }

        public void Start()
        {
            _running = true;
            _executeWorker.RunWorkerAsync();
        }

        private void Execute(object sender, DoWorkEventArgs e)
        {
            while (_running)
            {
                if (_commands.Count > 0)
                {
                    ICommand command = _commands.Dequeue();
                    _executors[(Command) command.Id].Execute(command);
                    Thread.Sleep(5);
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

        private void CreateAuthorizeExecutors()
        {
            _executors.Add(Command.AuthorizeResponse, new AuthorizeExecutor());
            _executors.Add(Command.GetGameStateResponse, new GetGameStateExecutor());
        }

        private void CreateBigMapExecutors()
        {
            _executors.Add(Command.GetMapResponse, new GetMapExecutor());
            _executors.Add(Command.GetKingResponse, new GetKingExecutor());
            _executors.Add(Command.MoveKingResponse, new MoveKingExecutor());
            _executors.Add(Command.GetObjectsResponse, new GetObjectsExecutor());
            _executors.Add(Command.GetResourceMessage, new GetResourceMessageExecutor());
            _executors.Add(Command.ComeInCastleResponse, new ComeInCastleExecutor());
            _executors.Add(Command.BigMapResponse, new BigMapExecutor());
            _executors.Add(Command.CaptureMineResponse, new CaptureMineRequestExecutor());
            _executors.Add(Command.UpdateWorldMessage, new UpdateWorldMessageExecutor());
            _executors.Add(Command.LooseMineMessage, new LooseMineMessageExecutor());
        }

        private void CreateCastleExecutors()
        {
            _executors.Add(Command.GetBuildingsResponse, new GetBuildingsExecutor());
            _executors.Add(Command.CreateBuildingResponse, new CreateBuildingExecutor());
            _executors.Add(Command.DestroyBuildingResponse, new DestroyBuildingExecutor());
            _executors.Add(Command.LeaveCastleResponse, new LeaveCastleExecutor());
            _executors.Add(Command.GetBuildingQueueResponse, new GetBuildingQueueExecutor());
            _executors.Add(Command.GetCreationRequirementsResponse, new GetCreationRequirementsExecutor());
        }

        private void CreateErrorExecutors()
        {
            _executors.Add(Command.ErrorMessage, new ErrorMessageExecutor());
        }
    }
}
