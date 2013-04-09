using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessLibrary.Interfaces
{
    public interface IActivator
    {
        void ActivateMine(Mine target);
        void ActivateCastle(Castle target);
    }
}
