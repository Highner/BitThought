using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class Synapse
    {
        #region constructor
        public Synapse(NeuronBase sourceneuron, NeuronBase targetneuron)
        {
            SourceNeuron = sourceneuron;
            TargetNeuron = targetneuron;
            Weight = 0;
        }
        #endregion

        #region methods
        public void PassValue(double inputvalue)
        {
            OutputValue = Weight * inputvalue;
            TargetNeuron.AddInput((OutputValue));
        }
        public void InitializeWeight(double weight)
        {
            Weight = weight;
        }
        public void UpdateToNewWeights()
        {
            //Weight = NewWeight;
        }
        public void CalculateNewWeight(double value, double learningrate)
        {
            PreviousWeight = Weight;
            Weight += (value * learningrate);
        }
        #endregion

        #region properties
        
        #endregion

        #region fields
        private NeuronBase SourceNeuron;
        public NeuronBase TargetNeuron;
        public double Weight;
        public double PreviousWeight;
        public int InputLayerSize;
        public int OutputLayerSize;
        public double OutputValue;
        #endregion
    }
}
