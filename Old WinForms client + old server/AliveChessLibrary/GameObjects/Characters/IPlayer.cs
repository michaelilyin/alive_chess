using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;

namespace AliveChessLibrary.GameObjects.Characters
{
    public interface IPlayer
    {
        int Id { get; }
        Map Map { get; set; }
        void AddKing(King king);
        void RemoveKing(King king);
        bool HasKing(int kingId);
        void UpdateVisibleSpace(VisibleSpace sector);
        VisibleSpace VisibleSpace { get; set; }
        bool IsSuperUser { get; set; }
        bool Bot { get; }
        IMessenger Messenger { get; set; }
        ILevel Level { get; set; }
        int LevelId { get; set; }
        bool Ready { get; set; }
    }
}
