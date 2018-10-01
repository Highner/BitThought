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
        public DeepThoughtNetwork(int numberofinputneurons, int[] layers, int numberofoutputneurons, Data.EnumActivationFunctions activationfunction) : this(numberofinputneurons, layers, numberofoutputneurons, new Data.EnumActivationFunctions[] { activationfunction } )
        {
        }
        public DeepThoughtNetwork(int numberofinputneurons, int[] layers, int numberofoutputneurons, Data.EnumActivationFunctions[] activationfunction)
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
            if(ValidateNetworkPlan())
            {
                CreateInputNeurons();

                CreateOutputNeurons();

                CreateHiddenNeurons();

                CreateSynapses();

                InitializeWeights();
            }
        }

        protected void InitializeWeights()
        {
            GaussianRandom ran = new GaussianRandom();
            foreach (Synapse synapse in Synapses)
            {
                double xavier = (InputNeurons.Count() + OutputNeurons.Count()) / 2;
                double weight = (double)(ran.NextGaussian(0, xavier));
                synapse.InitializeWeight(weight / 100);
            }
        }

        private void CreateInputNeurons()
        {
            for (int i = 1; i <= NumberOfInputNeurons; i++)
            {
                InputNeurons.Add(new InputNeuron(ActivationFunction[0], i, LearningRate));
            }
        }

        private void CreateOutputNeurons()
        {
            for (int i = 1; i <= NumberOfOutputNeurons; i++)
            {
                if(ActivationFunction.Count() == 1)
                {
                    OutputNeurons.Add(new OutputNeuron(ActivationFunction[0], LearningRate));
                }
                else
                {
                    OutputNeurons.Add(new OutputNeuron(ActivationFunction[NumberOfLayers - 1], LearningRate));
                }
                
            }
        }

        private void CreateHiddenNeurons()
        {
            //add layers
            for (int i = 1; i <= NumberOfLayers; i++)
            {
                //add bias neuron to each layer and to general hidden neuron collection
                HiddenLayers.Add(new List<HiddenNeuron>());
                HiddenNeuron biasneuron;
                if (ActivationFunction.Count() == 1)
                {
                    biasneuron = new HiddenNeuron(ActivationFunction[0], i, true, LearningRate);
                }
                else
                {
                    biasneuron = new HiddenNeuron(ActivationFunction[i - 1], i, true, LearningRate);
                }

                HiddenNeurons.Add(biasneuron);
                HiddenLayers[i - 1].Add(biasneuron);

                //add hidden neurons to each layer according to layerdistribution and to general hidden neuron collection
                for (int x = 1; x <= LayerDistribution[i - 1]; x++)
                {
                    HiddenNeuron hiddenneuron;
                    if (ActivationFunction.Count() == 1)
                    {
                        hiddenneuron = new HiddenNeuron(ActivationFunction[0], i, false, LearningRate);
                    }
                    else
                    {
                        hiddenneuron = new HiddenNeuron(ActivationFunction[i - 1], i, false, LearningRate);
                    }

                    HiddenNeurons.Add(hiddenneuron);
                    HiddenLayers[i - 1].Add(hiddenneuron);
                }
            }
        }

        private void CreateSynapses()
        {
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
                    Synapses.Add(sourceneuron.CreateOutputSynapse(targetneuron, OutputNeurons.Count(), HiddenNeurons.Where(x => x.Layer == NumberOfLayers).Count()));
                }
            }
        }

        private bool ValidateNetworkPlan()
        {
            if(!(ActivationFunction.Count() == 1) && !(LayerDistribution.Count() == ActivationFunction.Count()))
            {
                return false;
            }
            return true;
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
        public double[] GetResult(double[] values)
        {

            ForwardPropagate(values);

            List<double> resultlist = new List<double>();
            foreach (OutputNeuron outputneuron in OutputNeurons)
            {
                resultlist.Add(outputneuron.OutputValue);
            }

            return resultlist.ToArray();
        }

        #endregion

        #region training
        private void AdjustLearningRate()
        {
            LearningRate = StoredLearningrate / (1 + Epoch / 1);
        }

        public void SetLearningRate(double learningrate)
        {
            StoredLearningrate = learningrate;
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

        private void SetExpectedOutputValues(double[] expecteddata)
        {
            foreach (OutputNeuron outputneuron in OutputNeurons)
            {
                outputneuron.ExpectedOutputValue = expecteddata[OutputNeurons.IndexOf(outputneuron)];
            }
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

        private void BackPropagate()
        {
            foreach (OutputNeuron outputneuron in OutputNeurons)
            {
                outputneuron.BackPropagate();
            }

            for(int i = HiddenLayers.Count() - 1; i >= 0; i--)
            {
                foreach (HiddenNeuron neuron in HiddenLayers[i])
                {
                    neuron.BackPropagate();
                }
            }

            //foreach (InputNeuron inputneuron in InputNeurons)
            //{
            //    inputneuron.BackPropagate();
            //}
        }
        
        private double CalculateLoss(double[] expectedvalues, double[] actualvalues)
        {
            double sum = 0;
            for (int i = 0; i < expectedvalues.Count(); i++)
            {
                var error = Math.Pow(expectedvalues[i] - actualvalues[i], 2);

                sum += error;
            }
            return sum / expectedvalues.Count();

        }

        private void UpdateWeights()
        {
            foreach (Synapse synapse in Synapses)
            {
                synapse.UpdateToNewWeights();
            }
        }

        protected void TrainCycle(double[] inputdata, double[] expecteddata, bool addtolossfunction = false)
        {
            ResetInputValues();

            Epoch += 1;

            ForwardPropagate(inputdata);

            SetExpectedOutputValues(expecteddata);

            double[] result = GetResult();

            Loss = CalculateLoss(expecteddata, result);

            if(addtolossfunction)
            {
                LossFunction.Add(Loss);
                Testdata.Add(Synapses[2].Weight);
            }


            BackPropagate();

            UpdateWeights();

            //AdjustLearningRate();
        }

        public void Train(List<double[]> trainingdata, List<double[]> expectedvalues, int epochs)
        {
            TrainingData = trainingdata;
            for(int i = 1; i <= epochs; i++)
            {
                foreach (double[] data in TrainingData) //.Where(x => x[0] == 1))
                {
                    TrainCycle(data, expectedvalues[TrainingData.IndexOf(data)], true); // data.Skip(1).ToArray(), new double[] { data[0] },true);//, (TrainingData.IndexOf(data) == 0));
                }
            }

        }
        #endregion

        #region properties
        private List<InputNeuron> InputNeurons { get; set; } = new List<InputNeuron>();

        private List<OutputNeuron> OutputNeurons { get; set; } = new List<OutputNeuron>();

        private List<HiddenNeuron> HiddenNeurons { get; set; } = new List<HiddenNeuron>();

        private List<Synapse> Synapses { get; set; } = new List<Synapse>();

        private List<List<HiddenNeuron>> HiddenLayers { get; set; } = new List<List<HiddenNeuron>>();

        protected int NumberOfLayers
        {
            get
            {
                return LayerDistribution.Count();
            }
        }

        public List<double> LossFunction { get; set; } = new List<double>();

        public List<double> Testdata = new List<double>();
        #endregion

        #region fields
        protected int[] LayerDistribution;

        protected int NumberOfInputNeurons;

        protected int NumberOfOutputNeurons;

        protected List<double[]> TrainingData { get; set; }

        private Data.EnumActivationFunctions[] ActivationFunction;

        public double Loss;

        private double StoredLearningrate = 0;

        protected double LearningRate = 0.01;

        private int Epoch;
        #endregion
    }
}
