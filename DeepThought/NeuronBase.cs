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
        }
        public NeuronBase(Func<double, double> activationfunction)
        {
            ActivationFunction = activationfunction;
            OutputSynapses = new List<Synapse>();
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

            OutputSynapses.Add(synapse);

            return synapse;
        }
        protected double GetOutputValue()
        {
            if (!IsBias)
            {
                return ActivationFunction.Invoke(InputValue);
            }
            else
            {
                return 1;
            }
            
        }
        #endregion

        #region properties
        public List< Synapse> OutputSynapses { get; set; }
        private Func<double, double> ActivationFunction { get; set; }
        public bool IsBias { get; set; }
        #endregion

        #region fields
        private double InputValue;
        private double OutputValue;
        #endregion
        
    }

}
