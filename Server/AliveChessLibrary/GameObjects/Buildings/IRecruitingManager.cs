using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IRecruitingManager
    {
        Castle Castle { get; set; }

        Dictionary<UnitType, CreationRequirements> CreationRequirements { get; set; }
    }
}
