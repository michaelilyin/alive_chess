using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessClient.NetLayer.Executors
{
    class GetListBuildingsInCastleExecutor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public GetListBuildingsInCastleExecutor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.CastleControl.list);
        }

        public void Execute(ICommand cmd)
        {
            GetListBuildingsInCastleResponse msg = (GetListBuildingsInCastleResponse)cmd;
 
            if (context.GameForm.Created)
            {
                for (int i = 0; i < msg.List.Count; i++)
                {
                    context.GameForm.CastleControl.Invoke(resourceHandler, msg.List[i]);
                }
            }
        }

        public delegate void UpdateResourceHandler(IInnerBuilding str);

    }
}
