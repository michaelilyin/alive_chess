using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    class LooseMineMessageExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            LooseMineMessage message = (LooseMineMessage)command;
            Mine mine = GameCore.Instance.World.Map.SearchMineById(message.MineId);
            if (mine != null)
            {
                switch (mine.MineType)
                {
                    case ResourceTypes.Gold:
                        {
                            System.Windows.MessageBox.Show("Ваша золотая шахта захвачена!");
                            break;
                        }
                    case ResourceTypes.Wood:
                        {
                            System.Windows.MessageBox.Show("Ваша лесопилка захвачена!");
                            break;
                        }
                    case ResourceTypes.Stone:
                        {
                            System.Windows.MessageBox.Show("Ваш карьер захвачен!");
                            break;
                        }
                    case ResourceTypes.Iron:
                        {
                            System.Windows.MessageBox.Show("Ваша железная шахта захвачена!");
                            break;
                        }
                    case ResourceTypes.Coal:
                        {
                            System.Windows.MessageBox.Show("Ваша угольная шахта захвачена!");
                            break;
                        }
                }
            }
        }

        #endregion
    }
}
