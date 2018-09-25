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
        public void PassValue(decimal inputvalue)
        {
            TargetNeuron.AddInput((Weight * inputvalue));
        }
        #endregion

        #region fields
        private NeuronBase SourceNeuron;
        private NeuronBase TargetNeuron;
        private Decimal Weight;
        #endregion
    }
}
