using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public interface INeuroNetwork
    {
        void Train(int epochs);

        void Test();

        void Validate();

        void SetTrainingData(double[][] trainingdata, double[][] testdata);

        double[] Compute(double[] value);

        List<double> ErrorFunction { get; set; }
        List<double[]> TestResult { get; set; }
        List<double[]> TestExpected { get; set; }
    }
}
