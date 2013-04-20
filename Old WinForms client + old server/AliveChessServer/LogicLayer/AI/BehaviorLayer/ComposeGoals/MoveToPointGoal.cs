using System;
using System.Drawing;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals
{
    public class MoveToPointGoal : Goal
    {
        private Point point;

        public MoveToPointGoal(Point point)
        {
            this.point = point;
        }
        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override GoalStatuses Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
