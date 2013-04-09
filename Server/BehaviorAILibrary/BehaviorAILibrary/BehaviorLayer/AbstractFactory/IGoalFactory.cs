using System;

namespace BehaviorAILibrary.BehaviorLayer.AbstractFactory
{
    public interface IGoalFactory
    {
        Goal Create(CreationContext context);

        Type TypeOfGoal { get; }
    }
}
