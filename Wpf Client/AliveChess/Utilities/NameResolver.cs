using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.Utilities
{
    public class NameResolver
    {
        private static NameResolver _instance;

        private NameResolver()
        {
            
        }

        public static NameResolver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NameResolver();
                return _instance;
            }
        }

        public string GetNameByType(InnerBuildingType type)
        {
            switch (type)
            {
                case InnerBuildingType.Quarters:
                    return "Казармы";
                case InnerBuildingType.TrainingGround:
                    return "Тренировочная площадка";
                case InnerBuildingType.Stabling:
                    return "Конюшня";
                case InnerBuildingType.Workshop:
                    return "Мастерская";
                case InnerBuildingType.Fortress:
                    return "Крепость";
                case InnerBuildingType.Forge:
                    return "Кузница";
                case InnerBuildingType.Hospital:
                    return "Больница";
            }
            return type.ToString();
        }

        public string GetNameByType(ResourceTypes type)
        {
            switch (type)
            {
                case ResourceTypes.Gold:
                    return "Золото";
                case ResourceTypes.Stone:
                    return "Камень";
                case ResourceTypes.Wood:
                    return "Дерево";
                case ResourceTypes.Iron:
                    return "Железо";
                case ResourceTypes.Coal:
                    return "Уголь";
            }
            return type.ToString();
        }

        public string GetNameByType(UnitType type)
        {
            switch (type)
            {
                case UnitType.Pawn:
                    return "Пешка";
                case UnitType.Bishop:
                    return "Слон";
                case UnitType.Knight:
                    return "Конь";
                case UnitType.Rook:
                    return "Ладья";
                case UnitType.Queen:
                    return "Ферзь";
            }
            return type.ToString();
        }
    }
}
