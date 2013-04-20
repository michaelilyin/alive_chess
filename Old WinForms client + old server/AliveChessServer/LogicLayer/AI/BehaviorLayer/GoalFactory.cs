using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessServer.LogicLayer.AI.BehaviorLayer.ComposeGoals;
using AliveChessServer.LogicLayer.AI.DecisionLayer;
using AliveChessServer.LogicLayer.AI.DecisionLayer.NeuralNetwork;
using AliveChessServer.LogicLayer.AI.PerseptionLayer;

namespace AliveChessServer.LogicLayer.AI.BehaviorLayer
{
    public class GoalFactory
    {
        public Goal CreateGoal(
            BotKing player,
            NeuroTeacher teacher,
            INeuralNetwork network,
            GameSetting setting,
            PriorityQueue memory)
        {
            //NeuroTeacher.OutputBehaviorType behaviorType =
            //    teacher.GetBehaviorType(network.MaxOutputId);
            switch (network.MaxOutputId)
            {
                case 0:
                    {
                        if (!memory.Exists(0))
                        {
                            Castle castle = setting.NearestPlayerCastle;
                            if (castle != null)
                            {
                                var goal = new ComeBack2NearestCastleGoal(player, castle);
                                goal.Priority = 0;
                                return goal;
                            }
                            else return null;
                        }
                        return null;
                    }
                case 1:
                    {
                        if (!memory.Exists(2))
                        {
                            King enemy = setting.NearestEnemyKing;
                            if (enemy != null)
                            {
                                player.Steering.Target = enemy.Position;
                                var goal = new KingChasingGoal(player, enemy);
                                goal.Priority = 2;
                                return goal;
                            }
                            else return null;
                        }
                        return null;
                    }
                case 2:
                    {
                        if (!memory.Exists(4))
                        {
                            King enemy = setting.NearestEnemyKing;
                            if (enemy != null)
                            {
                                player.Steering.Target = enemy.Position;
                                var goal = new KingEvadingGoal(player, enemy);
                                goal.Priority = 4;
                                return goal;
                            }
                            else return null;
                        }
                        return null;
                    }
                case 3:
                    {
                        if (!memory.Exists(6))
                        {
                            Resource resource = setting.NearestResource;
                            if (resource != null)
                            {
                                player.Steering.Target = new Vector2D(resource.X, resource.Y);
                                var goal = new CollectResourcesGoal(player, resource);
                                goal.Priority = 6;
                                return goal;
                            }
                            else return null;
                        }
                        return null;
                    }
                default:
                    return null;
            }
        }

        public bool GetPriority(NeuroTeacher teacher, INeuralNetwork network, PriorityQueue memory)
        {
            // FAKE
            return memory.Exists(network.MaxOutputIDFake);
        }
    }
}
