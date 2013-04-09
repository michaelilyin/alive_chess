using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class UpdateWorldMessageExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
