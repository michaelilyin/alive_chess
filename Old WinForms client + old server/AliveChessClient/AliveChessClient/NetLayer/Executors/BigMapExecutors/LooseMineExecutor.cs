using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class LooseMineExecutor : IExecutor
    {
        private Game context;
        private MinesHandler handler;

        public LooseMineExecutor(Game context)
        {
            this.context = context;
            this.handler = new MinesHandler(context.GameForm.BigMapControl.UpdateMinesCount);
        }

        public void Execute(ICommand cmd)
        {
            LooseMineMessage resp = cmd as LooseMineMessage;
            context.Player.King.RemoveMine(resp.MineId);

            context.BigMap.Observers.Remove(resp.MineId);

            context.GameForm.Invoke(handler, context.Player.King.Mines.Count);

           // context.Player.UpdateVisibleSpace();
        }

        public delegate void MinesHandler(int count);
    }
}
