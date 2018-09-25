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
        public NeuronBase(Func<decimal, decimal> activationfunction)
        {
            ActivationFunction = activationfunction;
            OutputSynapses = new List<Synapse>();
        }
        #endregion

        #region methods
        public void AddInput(decimal input)
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
        public Synapse CreateOutputSynapse(NeuronBase targetneuron)
        {
            Synapse synapse = new Synapse(this, targetneuron);
            OutputSynapses.Add(synapse);
            return synapse;
        }
        protected decimal GetOutputValue()
        {
            return ActivationFunction.Invoke(InputValue);
        }
        #endregion

        #region properties
        public List< Synapse> OutputSynapses { get; set; }
        private Func<decimal, decimal> ActivationFunction { get; set; }
        #endregion

        #region fields
        private Decimal InputValue;
        private Decimal OutputValue;
        #endregion
        
    }

}
