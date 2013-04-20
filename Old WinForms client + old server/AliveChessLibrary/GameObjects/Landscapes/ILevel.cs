using System;
using AliveChessLibrary.Interaction;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    public interface ILevel
    {
        int Id { get; set; }
        Map Map { get; set; }
        void AddDispute(IDispute d);
        void RemoveDispute(IDispute d);
        void AddBattle(Battle b);
        void RemoveBattle(Battle b);
    }
}
