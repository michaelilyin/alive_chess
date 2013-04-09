using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    /// <summary>
    /// замок
    /// </summary>
    public interface ICastle : IBuilding
    {
        /// <summary>
        /// король
        /// </summary>
        King King { get; set; }
    
        /// <summary>
        /// игрок
        /// </summary>
        IPlayer Player { get; set; }

        /// <summary>
        /// замок ни кому не принадлежит
        /// </summary>
        bool IsFree { get; }

        /// <summary>
        /// в замке отсутствует король
        /// </summary>
        bool IsEmpty { get; }
    }
}
