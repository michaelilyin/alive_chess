using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Resources;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class GetHelpResourceExecutor : IExecutor
    {
        private Game context;
        private ResourceHandler h;

        public GetHelpResourceExecutor(Game context)
        {
  
        }

        public void Execute(ICommand cmd)
        {
            GetHelpResourceResponse resp = (GetHelpResourceResponse)cmd;
            foreach (Resource r in resp.Resources)
            {
                context.Player.King.StartCastle.ResourceStore.AddResourceToRepository(r);
                context.GameForm.Invoke(h, context.Player.King.StartCastle.ResourceStore
                    .GetResourceCountInRepository(r.ResourceType), r.ResourceType);
            }

        }

        public delegate void ResourceHandler(int count, ResourceTypes type);
    }
}
