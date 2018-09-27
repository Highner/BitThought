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
        public NeuronBase(Data.EnumActivationFunctions activationfunction)
        {
            ActivationFunction = Data.ActivationFunctionDataFactory.GenerateActivationFunction(activationfunction);
            OutputSynapses = new List<Synapse>();
            InputSynapses = new List<Synapse>();
        }
        #endregion

        #region methods
        public void AddInput(double input)
        {
            InputValue += input;
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

        private void CalculateErrorSignal()
        {
            ErrorSignal = ActivationFunction.Derivative(OutputValue) * NeuronLoss;
        }

        public void Activate()
        {
            OutputValue = GetOutputValue();

            foreach (Synapse syn in OutputSynapses)
            {
                syn.PassValue(OutputValue);
            }
        }

        public void BackPropagate()
        {
            CalculateErrorSignal();

            foreach(Synapse synapse in InputSynapses)
            {
                
            }
        }

        public void ResetInputValue()
        {
            InputValue = 0;
        }

        #endregion

        #region creation
        public Synapse CreateOutputSynapse(NeuronBase targetneuron, int nextlayersize, int prelayersize)
        {
            Synapse synapse = new Synapse(this, targetneuron);

            synapse.InputLayerSize = prelayersize;

            synapse.OutputLayerSize = nextlayersize;

            targetneuron.InputSynapses.Add(synapse);

            OutputSynapses.Add(synapse);

            return synapse;
        }

        #endregion

        #region properties
        public List<Synapse> OutputSynapses { get; set; }
        public List<Synapse> InputSynapses { get; set; }
        private IActivationFunction ActivationFunction { get; set; }
        public bool IsBias { get; set; }
        public double NeuronLoss
        {
            get
            {
                return ExpectedOutputValue - OutputValue;
            }
        }
        #endregion

        #region fields
        private double InputValue;
        public double OutputValue;
        public double ExpectedOutputValue;
        public double ErrorSignal;
        #endregion

    }

}
