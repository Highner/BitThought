using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public abstract class NeuroNetworkBase : INeuroNetwork
    {
        #region contructor
        public NeuroNetworkBase(Accord.Neuro.IActivationFunction function, int inputneurons, int[] hiddenlayers)
        {
            CreateNetwork(function, inputneurons, hiddenlayers);

            CreateTeacher();
        }
        #endregion

        #region private methods
        void CreateNetwork(Accord.Neuro.IActivationFunction function, int inputneurons, int[] hiddenlayers)
        {
            _Network = new Accord.Neuro.ActivationNetwork(function, inputneurons, hiddenlayers);
            _InputNeurons = inputneurons;
        }
        void CreateTeacher()
        {
            _Teacher = new Accord.Neuro.Learning.ParallelResilientBackpropagationLearning(_Network);
        }
        bool ValidateData()
        {
            if (_TrainingSourceData == null) { return false; }

            if (_TrainingInput == null) { return false; }

            if (_TrainingOutput == null) { return false; }

            if (!(_TrainingOutput.Count() == _TrainingInput.Count())) { return false; }

            if (!(_TrainingInput[0].Length == _Network.InputsCount)) { return false; }

            return true;
        }

        protected abstract void Convert(double[][] source, out double[][] input, out double[][] output);

        #endregion

        #region public methods
        public void Train(int epochs)
        {
            if(!ValidateData()) { return; }

            ErrorFunction = new List<double>();

            for (int i = 0; i < epochs; i++)
            {
                ErrorFunction.Add(_Teacher.RunEpoch(_TrainingInput, _TrainingOutput));
            }
        }

        public virtual void Test()
        {
            TestResult.Clear();
            TestExpected.Clear();

            int testlength = _TestInput.Length;
            for (int i = 0; i < testlength; i += 10)
            {
                TestResult.Add(Compute(_TestInput[i]));
                TestExpected.Add(_TestOutput[i]);
            }
        }

        public void SetTrainingData(double[][] trainingdata, double[][] testdata)
        {
            _TrainingSourceData = trainingdata;
            _TestSourceData = testdata;

            double[][] _traininginput;
            double[][] _trainingoutput;

            Convert(_TrainingSourceData, out _traininginput, out _trainingoutput);
            _TrainingInput = _traininginput;
            _TrainingOutput = _trainingoutput;

            double[][] _testinput;
            double[][] _testoutput;

            Convert(_TestSourceData, out _testinput, out _testoutput);
            _TestInput = _testinput;
            _TestOutput = _testoutput;
        }

        public double[] Compute(double[] value)
        {
            return _Network.Compute(value);
        }
        #endregion

        #region private properties
        private Accord.Neuro.ActivationNetwork _Network { get; set; }
        private Accord.Neuro.Learning.ParallelResilientBackpropagationLearning _Teacher { get; set; }
        protected double[][] _TrainingSourceData { get; set; }
        protected double[][] _TrainingInput { get; set; }
        protected double[][] _TrainingOutput { get; set; }
        protected double[][] _TestSourceData { get; set; }
        protected double[][] _TestInput { get; set; }
        protected double[][] _TestOutput { get; set; }
        protected int _InputNeurons { get; set; }
        #endregion

        #region public properties
        public List<double> ErrorFunction { get; set; }

        public List<double[]> TestResult { get; set; } = new List<double[]>();
        public List<double[]> TestExpected { get; set; } = new List<double[]>();
        #endregion
    }
}
