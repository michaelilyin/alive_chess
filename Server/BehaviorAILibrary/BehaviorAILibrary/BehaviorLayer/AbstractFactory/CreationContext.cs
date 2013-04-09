using BehaviorAILibrary.DecisionLayer;
using BehaviorAILibrary.PerseptionLayer;

namespace BehaviorAILibrary.BehaviorLayer.AbstractFactory
{
    public class CreationContext
    {
        private BotKing _botKing;
        private NeuroTeacher _teacher;
        private PoolingManager _poolingManager;
        private PriorityQueue _priorityQueue;

        public CreationContext(BotKing botKing, NeuroTeacher teacher, PoolingManager poolingManager,
            PriorityQueue priorityQueue)
        {
            this._botKing = botKing;
            this._teacher = teacher;
            this._poolingManager = poolingManager;
            this._priorityQueue = priorityQueue;
        }

        public BotKing BotKing
        {
            get { return _botKing; }
            set { _botKing = value; }
        }

        public NeuroTeacher Teacher
        {
            get { return _teacher; }
            set { _teacher = value; }
        }

        public PoolingManager PoolingManager
        {
            get { return _poolingManager; }
            set { _poolingManager = value; }
        }

        public PriorityQueue PriorityQueue
        {
            get { return _priorityQueue; }
            set { _priorityQueue = value; }
        }
    }
}
