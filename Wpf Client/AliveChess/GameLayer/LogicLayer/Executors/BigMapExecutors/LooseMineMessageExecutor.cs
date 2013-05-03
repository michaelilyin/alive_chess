using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    class LooseMineMessageExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            LooseMineMessage message = (LooseMineMessage)command;
            System.Windows.MessageBox.Show("Ваша шахта захвачена!");
        }

        #endregion
    }
}
