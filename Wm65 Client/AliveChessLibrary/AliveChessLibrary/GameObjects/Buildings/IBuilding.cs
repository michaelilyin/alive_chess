using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IBuilding : IObserver
    {
        BuildingTypes BuildingType { get; }
    }
}
