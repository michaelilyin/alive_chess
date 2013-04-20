using System;
using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessServer.LogicLayer.AI.MotionLayer;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals
{
    public class ComeBack2NearestCastleGoal : CompositeGoal
    {
        private BotKing king;
        private Castle castle;
        private List<Position> path;

        public ComeBack2NearestCastleGoal(BotKing king, Castle castle)
        {
            this.king = king;
            this.castle = castle;
        }

        public override void Activate()
        {
            Status = GoalStatuses.Active;
            AStar astar = new AStar(castle.Map, new GameData());
            astar.SetStartEnd(new Point(king.X, king.Y), new Point(castle.X, castle.Y));
            path = astar.FindPath();
            if (path != null)
                king.AddSteps(new Queue<Position>(path));
        }

        public override void AddSubGoal(Goal goal)
        {
            throw new NotImplementedException();
        }

        public override GoalStatuses Process()
        {
            ActivateIfInactive();
            if (king.InsideCastle(castle))
            {
                Status = GoalStatuses.Completed;
            }
            return Status;
        }

        public override void Terminate()
        {
            king.Steering.WanderOff();
        }
    }
}
