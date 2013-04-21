using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessClient.NetLayer.Executors
{
    class GetRecBuildingsExequtor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public GetRecBuildingsExequtor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.CastleControl.date);
        }

        public void Execute(ICommand cmd)
        {
             GetRecBuildingsResponse msg = (GetRecBuildingsResponse)cmd;

             if (context.GameForm.Created)
             {
                 context.GameForm.CastleControl.Invoke(resourceHandler, msg.ResBuild);

             }
        }

        public delegate void UpdateResourceHandler(ResBuild res);
        //public delegate void UpdateResourceHandler(ResourceTypes t, int count);
    }
}
