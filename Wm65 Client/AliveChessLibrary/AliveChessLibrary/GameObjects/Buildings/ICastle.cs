using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface ICastle : IBuilding
    {
        King King { get; set; }
    
        IPlayer Player { get; set; }

        bool IsFree { get; }

        bool IsEmpty { get; }
    }
}
