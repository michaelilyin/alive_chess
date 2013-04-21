using System.Threading;
using AliveChessClient.GameLayer;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class CaptureMineExecutor : IExecutor
    {
        private Game context;
        private MinesHandler handler;

        public CaptureMineExecutor(Game context)
        {
            this.context = context;
            this.handler = new MinesHandler(context.GameForm.BigMapControl.UpdateMinesCount);
        }

        public void Execute(ICommand cmd)
        {
            CaptureMineResponse response = (CaptureMineResponse)cmd;

            response.Mine.Map = context.Player.Map;
            context.Player.King.AddMine(response.Mine);
          
            Monitor.Enter(context.BigMap.Observers);
            context.BigMap.Observers.Add(response.Mine.Id, response.Mine);
            Monitor.Exit(context.BigMap.Observers);

            context.GameForm.Invoke(handler, context.Player.King.Mines.Count);

            QueryManager.SendGetObjectsRequestForConcreteObserver(context.Player, 
                response.Mine, PointTypes.Mine);
        }

        public delegate void MinesHandler(int count);
    }
}
