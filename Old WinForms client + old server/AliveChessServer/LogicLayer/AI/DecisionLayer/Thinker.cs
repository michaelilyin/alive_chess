using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessServer.LogicLayer.AI.BehaviorLayer;
using AliveChessServer.LogicLayer.AI.DecisionLayer.NeuralNetwork;
using AliveChessServer.LogicLayer.AI.MotionLayer;

//using NeuralNetwork = AliveChessServer.LogicLayer.AI.DecisionLayer.NeuralNetwork;

namespace AliveChessServer.LogicLayer.AI.DecisionLayer
{
    public class Thinker
    {
        private GoalFactory _factory;
        private PathPlanner _planner;
        private INeuralNetwork _network;
        
        public Thinker(ILocalizable map, GameData context)
        {
            _factory = new GoalFactory();
            _planner = new PathPlanner(map, context);
            _network = new NeuralNetwork.NeuralNetwork(7, 15, 8);
        }
       
        public Goal MakeDecision()
        {
            return null;
        }

        public INeuralNetwork Network
        {
            get { return _network; }
            set { _network = value; }
        }
    }
}
