using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    abstract class NeuronBase
    {
        #region constructor
        public NeuronBase()
        {
            OutputSynapses = new List<Synapse>();
            InputSynapses = new List<Synapse>();
        }
        public NeuronBase(IActivationFunction activationfunction)
        {
            ActivationFunction = activationfunction;
            OutputSynapses = new List<Synapse>();
            InputSynapses = new List<Synapse>();
        }
        #endregion

        #region methods
        public void AddInput(double input)
        {
            InputValue += input;
        }

        public void Activate()
        {
            OutputValue = GetOutputValue();

            foreach (Synapse syn in OutputSynapses)
            {
                syn.PassValue(OutputValue);
            }
        }

        public void ResetInputValue()
        {
            InputValue = 0;
        }

        public Synapse CreateOutputSynapse(NeuronBase targetneuron, int nextlayersize, int prelayersize)
        {
            Synapse synapse = new Synapse(this, targetneuron);

            synapse.InputLayerSize = prelayersize;

            synapse.OutputLayerSize = nextlayersize;

            targetneuron.InputSynapses.Add(synapse);

            OutputSynapses.Add(synapse);

            return synapse;
        }
        protected double GetOutputValue()
        {
            if (!IsBias)
            {
                return ActivationFunction.ActivationFunction(InputValue);
            }
            else
            {
                return 1;
            }
            
        }
        #endregion

        #region properties
        public List<Synapse> OutputSynapses { get; set; }
        public List<Synapse> InputSynapses { get; set; }
        private IActivationFunction ActivationFunction { get; set; }
        public bool IsBias { get; set; }
        #endregion

        #region fields
        private double InputValue;
        public double OutputValue;
        #endregion
        
    }

}
