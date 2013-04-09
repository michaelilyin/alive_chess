using System;
using BehaviorAILibrary.DecisionLayer.NeuralNetwork;

namespace BehaviorAILibrary.DecisionLayer
{
    public enum OutputBehaviorType
    {
        ComeBackCastle = 0, //10000000
        ChaseEnemyKing = 1, //01000000
        GetAwayFromKing = 2, //00100000
        CollectResources = 3, //00010000
        Patrol = 4, //10001000
        Wander = 5, //10000100
        CaptureCastle = 6, //10000010
        CreateUnits = 7  //10000001
    }

    public enum InputState
    {
        DistanceToEnemyKing = 0,
        DistanceToPlayerCastle = 1,
        DistanceToEnemyCastle = 2,
        EnemyUnitsCountInsideCastle = 3,
        EnemyUnitsCountTogetherWithKing = 4,
        ResourceCountOnHand = 5,
        ResourceCountInVisibleArea = 6
    }

    public class NeuroTeacher
    {
        private const double elapsed = 0.0001;

        private int iterator;

        // arbitraty values to learn network
        private int _playerResourceCount;
        private int _distanceToPlayerCastle;
        private int _distanceToEnemyCastle;
        private int _distanceToEnemyKing;
        private int _enemyUnitsCountTogetherWithKing;
        private int _enemyUnitsCountInsideCastle;
        private int _resourceCountInVisibleArea;

        private const int CastleNearDistance             = 5; // Maximum distance to near castle
        private const int KingNearDistance               = 5; // Maximim distance to near king
        private const int UnitsCountForWeakKing          = 5; // King is weak if has unit number less than this constant
        private const int BigResourceNumber              = 5; // If king has resource number more than this constant then he is rich and can create units
        private const int ResourceNumberInVisibleSpace   = 1; // If there are resources in visible space then king can collect its

        private int coeff = 1;
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public int Iterator
        {
            get { return iterator; }
        }

        public void Teach(INeuralNetwork network)
        {
            do
            {
                ResetOutputs(network);
                PrepareInputData(network);
                if (IsCastleNear(_distanceToEnemyCastle))
                {
                    if (IsKingWeak(_enemyUnitsCountInsideCastle))
                    {
                        network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.CaptureCastle), 1);
                    }
                    else
                    {
                        AnalizeWorld(network);
                    }
                }
                else
                {
                    AnalizeWorld(network);
                }
                network.FeedForward();
                network.BackPropagate();
                iterator++;
                coeff = -coeff;
            } while (network.CalculateError() >= elapsed);
        }

        public int GetInputNumber(InputState inputState)
        {
            return Convert.ToInt32(inputState);
        }

        public int GetOutputNumber(OutputBehaviorType outputBehaviorType)
        {
            return Convert.ToInt32(outputBehaviorType);
        }

        public OutputBehaviorType GetBehaviorType(int outputNumber)
        {
            return (OutputBehaviorType) outputNumber;
        }

        private void ResetOutputs(INeuralNetwork network)
        {
            network.SetDesiredOutput(0, 0);
            network.SetDesiredOutput(1, 0);
            network.SetDesiredOutput(2, 0);
            network.SetDesiredOutput(3, 0);
            network.SetDesiredOutput(4, 0);
            network.SetDesiredOutput(5, 0);
            network.SetDesiredOutput(6, 0);
            network.SetDesiredOutput(7, 0);
        }

        private void PrepareInputData(INeuralNetwork network)
        {
            // generate arbitrary inputs
            _playerResourceCount = GenerateMetric(0, 10);
            _resourceCountInVisibleArea = GenerateMetric(0, 10);

            _distanceToEnemyKing = coeff * GenerateMetric(0, 10);
            _distanceToEnemyCastle = coeff * GenerateMetric(0, 10);
            _distanceToPlayerCastle = coeff * GenerateMetric(0, 10);

            _enemyUnitsCountInsideCastle = GenerateMetric(0, 10);
            _enemyUnitsCountTogetherWithKing = GenerateMetric(0, 10);

            // set neural network inputs. The newtwork has 7 inputs
            network.SetInput((int)InputState.ResourceCountOnHand, _playerResourceCount);
            network.SetInput((int)InputState.DistanceToEnemyKing, _distanceToEnemyKing);
            network.SetInput((int)InputState.DistanceToPlayerCastle, _distanceToPlayerCastle);
            network.SetInput((int)InputState.DistanceToEnemyCastle, _distanceToEnemyCastle);
            network.SetInput((int)InputState.EnemyUnitsCountInsideCastle, _enemyUnitsCountInsideCastle);
            network.SetInput((int)InputState.EnemyUnitsCountTogetherWithKing, _enemyUnitsCountTogetherWithKing);
            network.SetInput((int)InputState.ResourceCountInVisibleArea, _resourceCountInVisibleArea);
        }

        private void AnalizeWorld(INeuralNetwork network)
        {
            if (IsKingNear(_distanceToEnemyKing))
            {
                if (IsKingWeak(_enemyUnitsCountTogetherWithKing))
                {
                    network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.ChaseEnemyKing), 1);
                }
                else
                {
                    if (IsCastleNear(_distanceToPlayerCastle))
                    {
                        network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.ComeBackCastle), 1);
                    }
                    else
                    {
                        network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.GetAwayFromKing), 1);
                    }
                }
            }
            else
            {
                if (IsCastleNear(_distanceToPlayerCastle))
                {
                    if (IsManyResources(_playerResourceCount))
                    {
                        network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.CreateUnits), 1);
                    }
                    else
                    {
                        AnalizeResources(network);
                    }
                }
                else
                {
                    AnalizeResources(network);
                }
            }
        }

        private void AnalizeResources(INeuralNetwork network)
        {
            if (IsResourcesNear(_resourceCountInVisibleArea))
            {
                network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.CollectResources), 1);
            }
            else
            {
                network.SetDesiredOutput(GetOutputNumber(OutputBehaviorType.Wander), 1);
            }
        }

        private int GenerateMetric(int min, int max){return rnd.Next(min, max);}

        private bool IsKingNear(double distance) { return distance > 0 && distance < KingNearDistance; }
        private bool IsCastleNear(double distance) { return distance > 0 && distance < CastleNearDistance;}
       
        private bool IsKingWeak(double count){return count >= 0 && count < UnitsCountForWeakKing;}

        private bool IsManyResources(double count){return count > BigResourceNumber;}
        private bool IsResourcesNear(double count){return count > ResourceNumberInVisibleSpace;}
    }
}
