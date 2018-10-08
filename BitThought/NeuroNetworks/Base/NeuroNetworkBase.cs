using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public abstract class NeuroNetworkBase : INeuroNetwork
    {

        #region private methods
        bool ValidateData()
        {
            if (_TrainingSourceData == null) { return false; }

            if (_TrainingInput == null) { return false; }

            if (_TrainingOutput == null) { return false; }

            if (!(_TrainingOutput.Count() == _TrainingInput.Count())) { return false; }

            if (!(_TrainingInput[0].Length == _Network.InputsCount)) { return false; }

            return true;
        }

        void CalculateMSE()
        {
            int length = TestResult.Count();
            double sum = 0;
            for(int i = 0; i < length; i++)
            {
                sum += Math.Pow( TestResult[i].Sum() - TestExpected[i].Sum(), 2);
            }
            MeanSquareError = sum / length;
        }

        static double Scale(double x, double min, double max)
        {
            double fieldlow = 0;
            double fieldhigh = 1;
            double temp = ((x - min) / (max - min)) * (fieldhigh - fieldlow) + fieldlow;
            return temp;
        }
        static double DeScale(double x, double min, double max)
        {
            double fieldlow = 0;
            double fieldhigh = 1;
            double temp = ((min - max) * x - fieldhigh * min + max * fieldlow) / (fieldlow - fieldhigh);
            return temp;
        }

        void ScaleData(ref double[][] data)
        {
            List<double[]> newdata = new List<double[]>();
            for(int i = 0; i < data[0].Length; i++)
            {   
                if(!(i==5))
                {
                    double min = data.Select(y => y[i]).Min();
                    double max = data.Select(y => y[i]).Max();

                    for (int x = 0; x < data.Length; x++)
                    {
                        data[x][i] = Scale(data[x][i], min, max);
                    }
                }

            }
        }

        void DeScaleData(ref double[][] data)
        {
            List<double[]> newdata = new List<double[]>();
            for (int i = 0; i < data[0].Length; i++)
            {
                double min = data.Select(y => y[i]).Min();
                double max = data.Select(y => y[i]).Max();

                for (int x = 0; x < data.Length; x++)
                {
                    data[x][i] = DeScale(data[x][i], min, max);
                }
            }
        }
        protected abstract void Convert(double[][] source, int[] distribution, out double[][] input, out double[][] output);

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

            CalculateMSE();
            
        }

        public virtual void Validate()
        {
            ValidationResult.Clear();
            ValidationExpected.Clear();

            int testlength = _TrainingInput.Length;
            for (int i = 0; i < testlength; i += 10)
            {
                ValidationResult.Add(Compute(_TrainingInput[i]));
                ValidationExpected.Add(_TrainingOutput[i]);
            }


        }

        public void SetTrainingData(double[][] trainingdata, double[][] testdata)
        {

            _TrainingSourceData = trainingdata;
            _TestSourceData = testdata;

            if (_ScaleData)
            {
                var data1 = trainingdata;
                ScaleData(ref data1);
                _TrainingSourceData = data1;

                var data2 = testdata;
                ScaleData(ref data2);
                _TestSourceData = data2;
            }

            double[][] _traininginput;
            double[][] _trainingoutput;

            Convert(_TrainingSourceData, _InputDistribution, out _traininginput, out _trainingoutput);
            _TrainingInput = _traininginput;
            _TrainingOutput = _trainingoutput;

            double[][] _testinput;
            double[][] _testoutput;

            Convert(_TestSourceData, _InputDistribution, out _testinput, out _testoutput);
            _TestInput = _testinput;
            _TestOutput = _testoutput;
        }

        public double[] Compute(double[] value)
        {
            return _Network.Compute(value);
        }
        #endregion

        #region private properties
        protected Accord.Neuro.Network _Network { get; set; }
        protected Accord.Neuro.Learning.ISupervisedLearning _Teacher { get; set; }
        protected double[][] _TrainingSourceData { get; set; }
        protected double[][] _TrainingInput { get; set; }
        protected double[][] _TrainingOutput { get; set; }
        protected double[][] _TestSourceData { get; set; }
        protected double[][] _TestInput { get; set; }
        protected double[][] _TestOutput { get; set; }
        protected int _InputNeurons { get; set; }
        protected bool _ScaleData { get; set; }
        protected int[] _InputDistribution { get; set; }
        #endregion

        #region public properties
        public double DeterminationCoefficient { get; set; }
        public double MeanSquareError { get; set; }
        public List<double> ErrorFunction { get; set; }

        public List<double[]> TestResult { get; set; } = new List<double[]>();
        public List<double[]> TestExpected { get; set; } = new List<double[]>();

        public List<double[]> ValidationResult { get; set; } = new List<double[]>();
        public List<double[]> ValidationExpected { get; set; } = new List<double[]>();

        public double ValidationPercentage { get; set; }
        public double TestPercentage { get; set; }
        #endregion
    }
}
