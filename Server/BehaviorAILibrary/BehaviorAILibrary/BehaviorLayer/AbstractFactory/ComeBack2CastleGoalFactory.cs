using System;
using AliveChessLibrary.GameObjects.Buildings;
using BehaviorAILibrary.BehaviorLayer.ComposeGoals;

namespace BehaviorAILibrary.BehaviorLayer.AbstractFactory
{
    public class ComeBack2CastleGoalFactory : IGoalFactory
    {
        public Goal Create(CreationContext context)
        {
            BotKing botKing = context.BotKing;
            Castle castle = context.PoolingManager.NearestPlayerCastle;
            if (castle != null)
            {
                var goal = new ComeBack2NearestCastleGoal(botKing, castle);
                goal.Priority = 0;
                return goal;
            }
            return null;
        }

        public Type TypeOfGoal
        {
            get { return typeof(ComeBack2NearestCastleGoal); }
        }
    }
}
