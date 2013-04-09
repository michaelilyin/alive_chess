using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class FastBattle : IRoutine
    {
        private Player _player1;
        private Player _player2;

        public FastBattle(Player player1, Player player2)
        {
            this._player1 = player1;
            this._player2 = player2;
        }

        public void Update()
        {
            
        }
    }
}
