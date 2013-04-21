using AliveChessClient.GameLayer;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class GetResourceExecutor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;
        private DrawHandler handler;

        public GetResourceExecutor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.BigMapControl.UpdateResource);
            this.handler = new DrawHandler(context.BigMap.GraphicManager.Draw);
        }

        public void Execute(ICommand cmd)
        {
            GetResourceMessage msg = (GetResourceMessage)cmd;
            context.Player.King.StartCastle.ResourceStore.AddResourceToRepository(msg.Resource);

            if (context.GameForm.Created && !context.GameForm.IsDisposed)
            {
                context.GameForm.Invoke(resourceHandler,
                    context.Player.King.StartCastle.ResourceStore.GetResourceCountInRepository(msg.Resource.ResourceType),
                    msg.Resource.ResourceType);

                if (!msg.FromMine)
                {
                    MapPoint res = context.Player.Map[msg.Resource.X, msg.Resource.Y];
                    context.DeleteEntityFromModel(res);
                    context.Player.Map.SetObject(res.ObjectUnderThis);
                    context.Player.Map[res.X, res.Y].Detected = true;
                    context.GameForm.Invoke(handler);
                }
            }
        }

        public delegate void UpdateResourceHandler(object rCount, object rType);
    }
}
