using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks.Base
{
    public abstract class DeepBeliefNetworkBase: NeuroNetworkBase
    {
        #region contructor
        public DeepBeliefNetworkBase(Accord.Neuro.ActivationFunctions.IStochasticFunction function, int inputneurons, int[] hiddenlayers)
        {
            CreateNetwork(function, inputneurons, hiddenlayers);

            CreateTeacher();
        }
        #endregion

        #region private methods
        void CreateNetwork(Accord.Neuro.ActivationFunctions.IStochasticFunction function, int inputneurons, int[] hiddenlayers)
        {
            _Network = new Accord.Neuro.Networks.DeepBeliefNetwork(function, inputneurons, hiddenlayers);
            _InputNeurons = inputneurons;
        }
        void CreateTeacher()
        {
            Accord.Neuro.Networks.DeepBeliefNetwork net = (Accord.Neuro.Networks.DeepBeliefNetwork)_Network;
            _Teacher = new Accord.Neuro.Learning.ResilientBackpropagationLearning(net);
           
        }
        #endregion

        #region abstract methods
        protected override abstract void Convert(double[][] source, int[] distribution, out double[][] input, out double[][] output);
        #endregion
    }
}
