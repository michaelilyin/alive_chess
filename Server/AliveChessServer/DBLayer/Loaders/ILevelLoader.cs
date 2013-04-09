using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.DBLayer.Loaders
{
    public interface ILevelLoader
    {
        Level LoadLevel(LevelTypes level);

        Player LoadPlayer(Identity identity);

        void SavePlayer(Player player);

        void SaveKing(King king);

        void CommitAllChanges();

        Player FindPlayer(Func<Player, bool> predicate);

        LevelRoutine LevelRoutine { get; set; }
    }
}
