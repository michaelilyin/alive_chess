using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessServer.LogicLayer.AI.MotionLayer;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals
{
    public class KingEvadingGoal : CompositeGoal
    {
        private BotKing player;
        private King enemey;

        public KingEvadingGoal(BotKing king, King enemy)
        {
            this.player = king;
            this.enemey = enemy;
        }
        public override void Activate()
        {
            Status = GoalStatuses.Active;
            player.Steering.TargetAgent2 = enemey;
            player.Steering.SwitchOnBehavior(BehaviorType.Evade);
        }

        public override void AddSubGoal(Goal goal)
        {
            throw new NotImplementedException();
        }

        public override GoalStatuses Process()
        {
            ActivateIfInactive();
            if (player.ObstacleFound)
            {
                player.ObstacleFound = false;
                Status = GoalStatuses.Failed;
            }
            else if (player.Position.Distance(new Vector2D(enemey.X, enemey.Y)) > 10)
                Status = GoalStatuses.Completed;
            return Status;
        }

        public override void Terminate()
        {
           player.Steering.SwitchOnBehavior(BehaviorType.Wander);
        }
    }
}
