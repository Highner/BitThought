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
        protected decimal GetOutputValue()
        {
            return ActivationFunction.Invoke(InputValue);
        }
        #endregion

        #region properties
        private Synapse[] OutputSynapses { get; set; }
        private Func<decimal, decimal> ActivationFunction { get; set; }
        #endregion

        #region fields
        private Decimal InputValue;
        private Decimal OutputValue;
        #endregion
        
    }

}
