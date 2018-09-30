using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class OutputNeuron: NeuronBase
    {
        #region constructor
        public OutputNeuron(Data.EnumActivationFunctions activationfunction, double learningrate) : base(activationfunction, learningrate)
        {

        }
        #endregion

        #region basemethods
        public override void BackPropagate()
        {
            PreviousPartialDerivative = NeuronLoss * ActivationFunction.Derivative(OutputValue);

            foreach (Synapse synapse in InputSynapses)
            {
                var netInput = synapse.OutputValue;
                var delta = -1 * netInput * PreviousPartialDerivative;
                synapse.CalculateNewWeight(delta, LearningRate);
            }
        }


        #endregion

        #region properties
        public double NeuronLoss
        {
            get
            {
                return ExpectedOutputValue - OutputValue;
            }
        }
        #endregion

        #region fields
        public double ExpectedOutputValue;
        #endregion
    }
}
