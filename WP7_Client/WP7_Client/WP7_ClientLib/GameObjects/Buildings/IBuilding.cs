using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Buildings
{
    /// <summary>
    /// строение на большой карте
    /// </summary>
    public interface IBuilding : IObserver
    {
        /// <summary>
        /// тип строения
        /// </summary>
        BuildingTypes BuildingType { get; }
    }
}
