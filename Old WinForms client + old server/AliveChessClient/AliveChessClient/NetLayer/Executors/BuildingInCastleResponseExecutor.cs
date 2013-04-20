using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessClient.NetLayer.Executors
{
    class BuildingInCastleResponseExecutor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public BuildingInCastleResponseExecutor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.CastleControl.list);
        }

        public void Execute(ICommand cmd)
        {
            BuildingInCastleResponse msg = (BuildingInCastleResponse)cmd;
            

            if (context.GameForm.Created)
            {
                int l = msg.Buildings_list.Count;
                for (int i = 0; i < l; i++)
                {
                    context.GameForm.CastleControl.Invoke(resourceHandler, msg.Buildings_list[i]);
                }
                //context.GameForm.CastleControl.Invoke(resourceHandler,t);
                 
            }
        }

        public delegate void UpdateResourceHandler(IInnerBuilding str);
    }

    }

