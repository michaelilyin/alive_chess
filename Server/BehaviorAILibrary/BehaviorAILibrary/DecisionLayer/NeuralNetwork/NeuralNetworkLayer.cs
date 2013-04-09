// Static Model

using System;

namespace BehaviorAILibrary.DecisionLayer.NeuralNetwork
{
    [Serializable]
	public class NeuralNetworkLayer
	{
        private int numberOfNodes = 0;
        public int NumberOfNodes
        {
            get { return numberOfNodes; }
            set { numberOfNodes = value; }
        }

        private int numberOfChildNodes = 0;
        public int NumberOfChildNodes
        {
            get { return numberOfChildNodes; }
            set { numberOfChildNodes = value; }
        }

        private int numberOfParentNodes = 0;
        public int NumberOfParentNodes
        {
            get { return numberOfParentNodes; }
            set { numberOfParentNodes = value; }
        }

        private double learningRate = 0;
        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

        private double momentumFactor = 0.9;
        public double MomentumFactor
        {
            get { return momentumFactor; }
            set { momentumFactor = value; }
        }

        private bool linearOutput = false;
        public bool LinearOutput
        {
            get { return linearOutput; }
            set { linearOutput = value; }
        }

        private bool useMomentum = false;
        public bool UseMomentum
        {
            get { return useMomentum; }
            set { useMomentum = value; }
        }

        private double[,] weights = null;
        public double[,] Weights
        {
            get { return weights; }
            set { weights = value; }
        }
        [NonSerialized]
		private double [,] weightChanges = null;

        private double[] neuronValues = null;
        public double[] NeuronValues
        {
            get { return neuronValues; }
            set { neuronValues = value; }
        }

        private double[] desiredValues = null;
        public double[] DesiredValues
        {
            get { return desiredValues; }
            set { desiredValues = value; }
        }

        private double[] errors = null;
        public double[] Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        private double[] biasWeights = null;

        public double[] BiasWeights
        {
            get { return biasWeights; }
            set { biasWeights = value; }
        }

        private double[] biasValues = null;
        public double[] BiasValues
        {
            get { return biasValues; }
            set { biasValues = value; }
        }

		private NeuralNetworkLayer parentLayer = null;
		private NeuralNetworkLayer childLayer = null;

        public NeuralNetworkLayer()
        {
            //
        }

		public NeuralNetworkLayer(int numNodes,	NeuralNetworkLayer parent,NeuralNetworkLayer child)
		{
			Initialize(numNodes,  parent, child);
		}

        public void Initialize(int numNodes, NeuralNetworkLayer parent, NeuralNetworkLayer child)
        {
            // Allocate memory
            NumberOfNodes=numNodes;
            NeuronValues = new double[NumberOfNodes];
            DesiredValues = new double[NumberOfNodes];
            Errors = new double[NumberOfNodes];
            if (parent != null)
            {
                parentLayer = parent;
            }
            if (child != null)
            {
                childLayer = child;
                Weights = new double[NumberOfNodes, NumberOfChildNodes];
                weightChanges = new double[NumberOfNodes,NumberOfChildNodes];
                BiasValues = new double[NumberOfChildNodes];
                BiasWeights = new double[NumberOfChildNodes];
            }
            // Initialize the bias values and Weights
            if (childLayer != null)
            {
                for (int j = 0; j < NumberOfChildNodes; j++)
                {
                    BiasValues[j] = -1;
                    BiasWeights[j] = 0;
                }
            }
        }

		public void RandomizeWeights()
		{
            int min = 0;
            int max = 200;
            int number = 0;            
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < NumberOfNodes; i++)
            {
                for (int j = 0; j < NumberOfChildNodes; j++)
                {
                    number = (rand.Next() % (max - min + 1)) + min;
                    if (number > max)
                        number = max;
                    if (number < min)
                        number = min;
                    Weights[i,j] = number / 100.0f - 1;
                }
            }
            for (int j = 0; j < NumberOfChildNodes; j++)
            {
                number = (rand.Next() % (max - min + 1)) + min;
                if (number > max)
                    number = max;
                if (number < min)
                    number = min;
                BiasWeights[j] = number / 100.0f - 1;
            }
		}

		public void CalculateErrors()
		{
            double sum=0;
            if (childLayer == null) 
            {// output layer
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    Errors[i] = (DesiredValues[i] - NeuronValues[i]) * NeuronValues[i] * (1.0f - NeuronValues[i]);
                }
            }
            else
            {
                if (parentLayer == null)
                { // input layer
                    for (int i = 0; i < NumberOfNodes; i++)
                    {
                        Errors[i] = 0.0f;
                    }
                }
                else
                { // hidden layer
                    for (int i = 0; i < NumberOfNodes; i++)
                    {
                        sum = 0;
                        for (int j = 0; j < NumberOfChildNodes; j++)
                        {
                            sum += childLayer.Errors[j] * Weights[i,j];
                        }
                        Errors[i] = sum * NeuronValues[i] * (1.0f - NeuronValues[i]);
                    }
                }
            }
		}

		public void AdjustWeights()
		{
            double dw;
            if (childLayer != null)
            {
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    for (int j = 0; j < NumberOfChildNodes; j++)
                    {
                        dw = LearningRate * childLayer.Errors[j] * NeuronValues[i];
                        if (UseMomentum)
                        {
                            Weights[i,j] += dw + MomentumFactor * weightChanges[i,j];
                            weightChanges[i,j] = dw;
                        }
                        else
                        {
                            Weights[i,j] += dw;
                        }
                    }
                }
                for (int j = 0; j < NumberOfChildNodes; j++)
                {
                    BiasWeights[j] += LearningRate * childLayer.Errors[j] *   BiasValues[j];
                }
            }
		}

		public void CalculateNeuronValues()
		{
            double x=0.0;
            if (parentLayer != null)
            {
                for (int j = 0; j < NumberOfNodes; j++)
                {
                    x = 0;
                    for (int i = 0; i < NumberOfParentNodes; i++)
                    {
                        x += parentLayer.NeuronValues[i] * parentLayer.Weights[i,j];
                    }
                    x += parentLayer.BiasValues[j] * parentLayer.BiasWeights[j];
                    if ((childLayer ==  null) && LinearOutput)
                        NeuronValues[j] = x;
                    else
                        NeuronValues[j] = LogisticFunc(x);
                }
            }
		}

        protected static double LogisticFunc(double x)
        {
            return 1.0f / (1 + Math.Exp(-x));
        }

		public virtual void Dispose()
		{
			
		}

	}// END CLASS DEFINITION NeuralNetworkLayer

} // AliveChess.Logic.GameLogic.BotsLogic.DecisionLayer.NeuralNetwork
