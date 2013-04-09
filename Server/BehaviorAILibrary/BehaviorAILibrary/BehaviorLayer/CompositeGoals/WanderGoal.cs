namespace BehaviorAILibrary.BehaviorLayer.ComposeGoals
{
    public class WanderGoal : Goal
    {
        private BotKing king;

        public WanderGoal(BotKing king)
        {
            this.king = king;
        }
        public override void Activate()
        {
            Status = GoalStatuses.Active;
            king.Steering.WanderOn();
        }        

        public override GoalStatuses Process()
        {
            ActivateIfInactive();
            return Status;
        }

        public override void Terminate()
        {
            king.Steering.WanderOff();
        }
    }
}
