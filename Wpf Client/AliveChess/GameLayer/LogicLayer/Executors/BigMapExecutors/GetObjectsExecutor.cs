using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class GetObjectsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetObjectsResponse response = (GetObjectsResponse)command;

            lock (GameCore.Instance.World.Map.Resources)
            {
                GameCore.Instance.World.Map.Resources = CustomConverter.ListToEntitySet(response.Resources);
            }
            lock (GameCore.Instance.World.Map.Kings)
            {
                GameCore.Instance.World.Map.Kings = CustomConverter.ListToEntitySet(response.Kings);
                GameCore.Instance.World.Map.Kings.Add(GameCore.Instance.Player.King);
            }

            if (response.Castles != null)
            {
                lock (GameCore.Instance.World.Map.Castles)
                {
                    foreach (var castle in response.Castles)
                    {
                        var oldCastle = GameCore.Instance.World.Map.SearchCastleById(castle.Id);
                        if (oldCastle != null)
                        {
                            oldCastle.KingId = castle.KingId;
                        }
                        else
                        {
                            castle.BuildingManager = new ProductionManager<InnerBuildingType>();
                            castle.RecruitingManager = new ProductionManager<UnitType>();
                            GameCore.Instance.World.Map.AddCastle(castle);
                        }
                    }
                }
            }

            if (response.Mines != null)
            {
                lock (GameCore.Instance.World.Map.Mines)
                {
                    foreach (var mine in response.Mines)
                    {
                        var oldMine = GameCore.Instance.World.Map.SearchMineById(mine.Id);
                        if (oldMine != null)
                        {
                            oldMine.KingId = mine.KingId;
                            oldMine.X = mine.X;
                            oldMine.Y = mine.Y;
                            oldMine.Width = mine.Width;
                            oldMine.Height = mine.Height;
                            oldMine.WayCost = mine.WayCost;
                            oldMine.GainingResource = mine.GainingResource;
                            oldMine.Capacity = mine.Capacity;
                        }
                        else
                        {
                            GameCore.Instance.World.Map.AddMine(mine);
                        }
                    }
                }
            }
            GameCore.Instance.BigMapCommandController.DynamicObjectsModified = true;
            GameCore.Instance.BigMapCommandController.BuildingsModified = true;
        }

        #endregion
    }
}
