﻿using System;
using AliveChessLibrary.GameObjects.Characters;
using BehaviorAILibrary.BehaviorLayer.ComposeGoals;

namespace BehaviorAILibrary.BehaviorLayer.AbstractFactory
{
    public class KingChasingGoalFactory : IGoalFactory
    {
        public Goal Create(CreationContext context)
        {
            BotKing botKing = context.BotKing;
            King enemy = context.PoolingManager.NearestEnemyKing;
            if (enemy != null)
            {
                botKing.Steering.Target = enemy.Position;
                var goal = new KingChasingGoal(botKing, enemy);
                goal.Priority = 2;
                return goal;
            }
            return null;
        }


        public Type TypeOfGoal
        {
            get { return typeof(KingChasingGoal); }
        }
    }
}