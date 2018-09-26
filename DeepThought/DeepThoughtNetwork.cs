using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    abstract public class DeepThoughtNetwork
    {
        #region constructor
        public DeepThoughtNetwork()
        { }
        public DeepThoughtNetwork(int numberofinputneurons, int[] layers, int numberofoutputneurons, Func<double, double> activationfunction)
        {
            LayerDistribution = layers;

            NumberOfInputNeurons = numberofinputneurons;

            NumberOfOutputNeurons = numberofoutputneurons;

            ActivationFunction = activationfunction;
        }
        #endregion
        
        #region network creation
        protected void CreateNetwork()
        {
            CreateInputNeurons();

            CreateOutputNeurons();

            CreateHiddenNeurons();

            CreateSynapses();

            InitializeWeights();
        }

        protected void InitializeWeights()
        {
            GaussianRandom ran = new GaussianRandom();
            foreach (Synapse synapse in Synapses)
            {
                double weight = (double)(ran.NextGaussian(synapse.InputLayerSize, synapse.OutputLayerSize));// * Math.Sqrt(2/synapse.InputLayerSize));
                synapse.InitializeWeight(weight);
            }
        }

        private void CreateInputNeurons()
        {
            InputNeurons = new List<InputNeuron>();
            for (int i = 1; i <= NumberOfInputNeurons; i++)
            {
                if (ActivationFunctionInput != null)
                {
                    InputNeurons.Add(new InputNeuron(ActivationFunctionInput, i));
                }
                else
                {
                    InputNeurons.Add(new InputNeuron(ActivationFunction, i));
                }
            }
        }

        private void CreateOutputNeurons()
        {
            OutputNeurons = new List<OutputNeuron>();
            for (int i = 1; i <= NumberOfOutputNeurons; i++)
            {
                if (ActivationFunctionOutput != null)
                {
                    OutputNeurons.Add(new OutputNeuron(ActivationFunctionOutput));
                }
                else
                {
                    OutputNeurons.Add(new OutputNeuron(ActivationFunction));
                }
            }
        }

        private void CreateHiddenNeurons()
        {
            HiddenNeurons = new List<HiddenNeuron>();
            HiddenLayers = new List<List<HiddenNeuron>>();
            for (int i = 1; i <= NumberOfLayers; i++)
            {
                HiddenLayers.Add(new List<HiddenNeuron>());
                HiddenNeuron biasneuron = new HiddenNeuron(ActivationFunction, i, true);
                HiddenNeurons.Add(biasneuron);
                HiddenLayers[i-1].Add(biasneuron);
                for (int x = 1; x <= LayerDistribution[i - 1]; x++)
                {
                    HiddenNeuron hiddenneuron = new HiddenNeuron(ActivationFunction, i);
                    HiddenNeurons.Add(hiddenneuron);
                    HiddenLayers[i - 1].Add(hiddenneuron);
                }
            }
        }

        private void CreateSynapses()
        {
            Synapses = new List<Synapse>();
            //connect first layer to inputneurons
            foreach(InputNeuron neuron in InputNeurons)
            {
                foreach (HiddenNeuron hiddenneuron in HiddenNeurons.Where(x => x.Layer == 1 && !x.IsBias))
                {
                    Synapses.Add(neuron.CreateOutputSynapse(hiddenneuron, HiddenNeurons.Where(x => x.Layer == 1).Count(), InputNeurons.Count()));
                }
            }

            //connect hidden layers
            for (int i = 1; i < NumberOfLayers; i++)
            {
                foreach(HiddenNeuron sourceneuron in HiddenNeurons.Where(x => x.Layer == i))
                {
                    foreach (HiddenNeuron targetneuron in HiddenNeurons.Where(x => x.Layer == i + 1 && !x.IsBias))
                    {
                        Synapses.Add(sourceneuron.CreateOutputSynapse(targetneuron, sourceneuron.Layer, targetneuron.Layer));
                    }
                }
            }

            //connect to outputneurons
            foreach (HiddenNeuron sourceneuron in HiddenNeurons.Where(x => x.Layer == NumberOfLayers))
            {
                foreach (OutputNeuron targetneuron in OutputNeurons)
                {
                    Synapses.Add(sourceneuron.CreateOutputSynapse(targetneuron, sourceneuron.Layer, OutputNeurons.Count()));
                }
            }
        }
        #endregion

        #region public methods
        public double[] GetResult()
        {
            List<double> resultlist = new List<double>();
            foreach(OutputNeuron outputneuron in OutputNeurons)
            {
                resultlist.Add(outputneuron.OutputValue);
            }

            return resultlist.ToArray();
        }

        public void SetActivationFunctionOutput(Func<double,double> activationfunction)
        {
            ActivationFunctionOutput = activationfunction;
        }

        public void SetActivationFunctionInput(Func<double, double> activationfunction)
        {
            ActivationFunctionInput = activationfunction;
        }
        #endregion

        #region training
        public void SetLearningRate(double learningrate)
        {
            LearningRate = learningrate;
        }

        private void ResetInputValues()
        {
            foreach (InputNeuron inputneuron in InputNeurons)
            {
                inputneuron.ResetInputValue();
            }

            foreach (List<HiddenNeuron> layer in HiddenLayers)
            {
                foreach (HiddenNeuron neuron in layer)
                {
                    neuron.ResetInputValue();
                }
            }

            foreach (OutputNeuron outputneuron in OutputNeurons)
            {
                outputneuron.ResetInputValue();
            }
        }

        protected void TrainCycle(double[] inputdata, double[] expecteddata)
        {

            ResetInputValues();

            ForwardPropagate(inputdata);

            double[] result = GetResult();

            Loss = CalculateLoss(GetResult(), expecteddata);




            UpdateWeights();
        }

        private void ForwardPropagate(double[] inputdata)
        {
            foreach (InputNeuron inputneuron in InputNeurons)
            {
                inputneuron.AddInput(inputdata[inputneuron.Index - 1]);

                inputneuron.Activate();
            }

            foreach (List<HiddenNeuron> layer in HiddenLayers)
            {
                foreach (HiddenNeuron neuron in layer)
                {
                    neuron.Activate();
                }
            }

            foreach (OutputNeuron outputneuron in OutputNeurons)
            {
                outputneuron.Activate();
            }
        }

        
        private double CalculateLoss(double[] expectedvalues, double[] actualvalues)
        {
            double sum = 0;
            for (int i = 1; i < expectedvalues.Count(); i++)
            {
                double error = expectedvalues[i] - actualvalues[i];

                sum += (error * error) / 2;
            }
            return sum;
        }

        private void UpdateWeights()
        {
            foreach(Synapse synapse in Synapses)
            {
                synapse.UpdateWeights();
            }
        }
        #endregion

        #region properties
        private List<InputNeuron> InputNeurons { get; set; }

        private List<OutputNeuron> OutputNeurons { get; set; }

        private List<HiddenNeuron> HiddenNeurons { get; set; }

        private List<Synapse> Synapses { get; set; }

        private List<List<HiddenNeuron>> HiddenLayers { get; set; }
        #endregion

        #region fields
        protected int NumberOfLayers
        {
            get
            {
                return LayerDistribution.Count();
            }
        }

        protected int[] LayerDistribution;

        protected int NumberOfInputNeurons;

        protected int NumberOfOutputNeurons;

        protected List<double[]> TrainingData { get; set; }

        private Func<double, double> ActivationFunction;

        private Func<double, double> ActivationFunctionInput;

        private Func<double, double> ActivationFunctionOutput;

        public double Loss;

        protected double LearningRate = 1;
        #endregion
    }
}
