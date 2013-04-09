using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class BigMapExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            BigMapRequest request = (BigMapRequest) msg.Command;

            // получаем контекст игрока
            Player player = msg.Sender;
            Level level = player.Level as Level;
            // меням состояние короля игрока
            player.King.State = KingState.BigMap;

            if (player.King.Interaction != null)
            {
                IDispute dispute = player.King.Interaction as IDispute;
                if (dispute.InteractionType == InteractionType.Dialog
                    || dispute.InteractionType == InteractionType.Negotiate)
                {
                    level.DisputeRoutine.TerminateDispute(dispute);
                }
            }
            else
            {
                // отправляем ответ
                player.Messenger.SendNetworkMessage(new BigMapResponse(true));
            }
        }
    }
}
