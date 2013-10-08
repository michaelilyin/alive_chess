using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public enum InnerBuildingType
    {
        Quarters, //казармы для создания пешек
        TrainingGround, // тренировочная площадка для создания слонов (офицеров)
        Stabling, // конюшня для создания коней
        Workshop, // мастерская для создания ладьи
        Fortress,  // крепость для создания ферзей
        Forge, // кузница - повышает боевую мощь производимых юнитов
        Hospital // больница - восстанавливает здоровье юнитов, находящихся в городе
    }
}
