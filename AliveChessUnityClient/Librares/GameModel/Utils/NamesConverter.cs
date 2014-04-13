using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.Utils
{
    public static class NamesConverter
    {
        public static string GetNameByType(InnerBuildingType type)
        {
            switch (type)
            {
                case InnerBuildingType.Quarters:
                    return "Casarms";
                case InnerBuildingType.TrainingGround:
                    return "Archers field";
                case InnerBuildingType.Stabling:
                    return "Stable";
                case InnerBuildingType.Workshop:
                    return "Workshop";
                case InnerBuildingType.Fortress:
                    return "Stronghold";
                case InnerBuildingType.Forge:
                    return "Smithy";
                case InnerBuildingType.Hospital:
                    return "Hospital";
            }
            return type.ToString();
        }

        public static string GetNameByType(UnitType type)
        {
            switch (type)
            {
                case UnitType.Pawn:
                    return "Pawn";
                case UnitType.Bishop:
                    return "Bishop";
                case UnitType.Knight:
                    return "Knight";
                case UnitType.Rook:
                    return "Rook";
                case UnitType.Queen:
                    return "Queen";
            }
            return type.ToString();
        }

        public static string GetNameByType(ResourceTypes type)
        {
            switch (type)
            {
                case ResourceTypes.Coal:
                    return "Coal";
                case ResourceTypes.Gold:
                    return "Gold";
                case ResourceTypes.Iron:
                    return "Iron";
                case ResourceTypes.Stone:
                    return "Stone";
                case ResourceTypes.Wood:
                    return "Wood";
            }
            return type.ToString();
        }
    }
}
