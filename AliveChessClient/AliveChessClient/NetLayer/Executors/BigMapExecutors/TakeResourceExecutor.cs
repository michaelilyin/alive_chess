using AliveChessClient.GameLayer;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class TakeResourceExecutor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;
        private DrawHandler handler;

        public TakeResourceExecutor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.BigMapControl.UpdateResource);
            this.handler = new DrawHandler(context.BigMap.GraphicManager.Draw);
        }

        public void Execute(ICommand cmd)
        {
            TakeResourceMessage msg = (TakeResourceMessage)cmd;
            context.Player.King.StartCastle.ResourceStore.DeleteResourceFromRepository(msg.Resource.ResourceType,
                msg.Resource.CountResource);
            if (context.GameForm.Created && !context.GameForm.IsDisposed)
            {
                context.GameForm.Invoke(resourceHandler,
                       context.Player.King.StartCastle.ResourceStore.GetResourceCountInRepository(msg.Resource.ResourceType),
                       msg.Resource.ResourceType);
            }
        }

        public delegate void UpdateResourceHandler(object rCount, object rType);
    }
}
