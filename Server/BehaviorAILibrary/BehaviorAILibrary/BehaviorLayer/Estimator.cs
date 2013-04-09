using System;
using System.Collections.Generic;
using BehaviorAILibrary.BehaviorLayer.AbstractFactory;
using BehaviorAILibrary.DecisionLayer;
using BehaviorAILibrary.DecisionLayer.NeuralNetwork;

namespace BehaviorAILibrary.BehaviorLayer
{
    public class Estimator
    {
        private PriorityQueue memory;
        private IDictionary<int, IGoalFactory> _factories;

        public Estimator(NeuroTeacher teacher, PriorityQueue memory)
        {
            this.memory = memory;
           
            InitializeFactories(teacher);
        }

        private void InitializeFactories(NeuroTeacher teacher)
        {
            _factories = new Dictionary<int, IGoalFactory>();

            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.Patrol), null);
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.Wander), null);
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.CaptureCastle), null);
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.CreateUnits), null);

            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.ChaseEnemyKing), new KingChasingGoalFactory());
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.CollectResources), new CollectResourcesGoalFactory());
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.ComeBackCastle), new ComeBack2CastleGoalFactory());
            _factories.Add(teacher.GetOutputNumber(OutputBehaviorType.GetAwayFromKing), new KingEvadingGoalFactory());
        }

        public Type EvaluateGoal(INeuralNetwork network)
        {
            return
                _factories.ContainsKey(network.MaxOutputIDFake)
                    ? _factories[network.MaxOutputIDFake].TypeOfGoal
                    : null;
        }

        public bool RequiredToCreate(Type type)
        {
            return type != null ? !memory.Exists(type) : false;
        }

        public IGoalFactory GetRequiredFactory(INeuralNetwork network)
        {
            return
                _factories.ContainsKey(network.MaxOutputIDFake)
                    ? _factories[network.MaxOutputIDFake]
                    : null;
        }
    }
}
