using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Resources
{
    public interface IResource : IMapObject
    {
        int CountResource { get; set; }

        ResourceTypes ResourceType { get; set; }
    }
}
