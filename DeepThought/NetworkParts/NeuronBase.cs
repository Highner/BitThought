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
        public NeuronBase(Data.EnumActivationFunctions activationfunction, double learningrate)
        {
            ActivationFunction = Data.ActivationFunctionDataFactory.GenerateActivationFunction(activationfunction);
            LearningRate = learningrate;
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

        public void Activate()
        {
            OutputValue = GetOutputValue();

            foreach (Synapse syn in OutputSynapses)
            {
                syn.PassValue(OutputValue);
            }
        }

        public virtual void BackPropagate() 
        {
            double sumPartial = 0;
            foreach (Synapse outsyn in OutputSynapses)
            {
                sumPartial += outsyn.PreviousWeight * outsyn.TargetNeuron.PreviousPartialDerivative;
            }


            foreach (Synapse synapse in InputSynapses)
            {

                var netInput = synapse.OutputValue;
                

                var delta = -1 * netInput * sumPartial * ActivationFunction.Derivative(OutputValue);
                synapse.CalculateNewWeight(delta, LearningRate);

            }
            PreviousPartialDerivative = sumPartial;
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
        protected IActivationFunction ActivationFunction { get; set; }
        public bool IsBias { get; set; }
        #endregion

        #region fields
        protected double InputValue;
        public double OutputValue;
        public double ErrorSignal;
        protected double LearningRate;
        public double PreviousPartialDerivative;
        #endregion

    }

}
