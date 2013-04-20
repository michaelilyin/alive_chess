using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessServer.LogicLayer.AI.MotionLayer;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals
{
    public class CollectResourcesGoal : CompositeGoal
    {
        private BotKing _player;
        private ILocation _target;

        public CollectResourcesGoal(BotKing player, ILocation target)
        {
            _player = player;
            _target = target;
        }

        public override void Activate()
        {
            _player.Steering.Target = new Vector2D(_target.X, _target.Y);
            _player.Steering.SwitchOnBehavior(BehaviorType.Seek);
        }

        public override void AddSubGoal(Goal goal)
        {
            throw new NotImplementedException();
        }

        public override GoalStatuses Process()
        {
            ActivateIfInactive();
            if (_player.ObstacleFound)
            {
                _player.ObstacleFound = false;
                Status = GoalStatuses.Failed;
            }
            else if (_player.Position.Equals(_target))
                Status = GoalStatuses.Completed;
            return Status;
        }

        public override void Terminate()
        {
            _player.Steering.SwitchOnBehavior(BehaviorType.Wander);
        }
    }
}
