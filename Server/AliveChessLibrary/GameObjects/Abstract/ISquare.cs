using System;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public interface ISquare
    {
        uint Id { get; set; }

        Guid DbId { get; set; }
    }
}
