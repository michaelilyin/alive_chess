using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessServer.LogicLayer.AI.MotionLayer;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals
{
    class KingChasingGoal : CompositeGoal
    {
        private King target;
        private BotKing player;

        public KingChasingGoal(BotKing king, King target)
        {
            this.player = king;
            this.target = target;
        }
        public override void Activate()
        {
            Status = GoalStatuses.Active;
            player.Steering.TargetAgent2 = target;
            player.Steering.SwitchOnBehavior(BehaviorType.Pursuit);
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
            else if (player.Position.Distance(new Vector2D(target.X, target.Y)) < 2)
                Status = GoalStatuses.Completed;
            return Status;
        }

        public override void Terminate()
        {
            player.Steering.SwitchOnBehavior(BehaviorType.Wander);
        }
    }
}
