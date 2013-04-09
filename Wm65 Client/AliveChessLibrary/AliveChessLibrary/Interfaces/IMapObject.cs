using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChessLibrary.Interfaces
{
    public interface IMapObject : ILocation
    {
        int Id { get; }

        Map Map { get; set; }

        PointTypes Type { get; }
    }
}
