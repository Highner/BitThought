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
            TargetNeuron.AddInput((Weight * inputvalue));
        }
        public void InitializeWeight(double weight)
        {
            Weight = weight;
        }
        public void UpdateWeights()
        {
            Weight = NewWeight;
        }
        #endregion

        #region properties
        
        #endregion

        #region fields
        private NeuronBase SourceNeuron;
        private NeuronBase TargetNeuron;
        private double Weight;
        private double NewWeight;
        public int InputLayerSize;
        public int OutputLayerSize;
        #endregion
    }
}
