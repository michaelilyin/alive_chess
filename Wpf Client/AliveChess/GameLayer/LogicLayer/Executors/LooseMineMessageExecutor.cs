using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.ErrorCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
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
