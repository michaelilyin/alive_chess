using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Resources
{
    /// <summary>
    /// ресурс
    /// </summary>
    public interface IResource : IMapObject
    {
        /// <summary>
        /// количество
        /// </summary>
        int CountResource { get; set; }

        /// <summary>
        /// тип ресурса
        /// </summary>
        ResourceTypes ResourceType { get; set; }
    }
}
