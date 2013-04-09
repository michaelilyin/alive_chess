using System;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Mathematic.GeometryUtils;
using BehaviorAILibrary.BehaviorLayer.ComposeGoals;

namespace BehaviorAILibrary.BehaviorLayer.AbstractFactory
{
    public class CollectResourcesGoalFactory : IGoalFactory
    {
        public Goal Create(CreationContext context)
        {
            BotKing botKing = context.BotKing;
            Resource resource = context.PoolingManager.NearestResource;
            if (resource != null)
            {
                botKing.Steering.Target = new Vector2D(resource.X, resource.Y);
                var goal = new CollectResourcesGoal(botKing, resource);
                goal.Priority = 6;
                return goal;
            }
            return null;
        }

        public Type TypeOfGoal
        {
            get { return typeof(CollectResourcesGoal); }
        }
    }
}
