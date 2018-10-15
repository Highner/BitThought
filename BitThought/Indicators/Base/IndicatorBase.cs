using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public abstract class IndicatorBase : IIndicator
    {
        #region constructor
        public IndicatorBase()
        {
            
        }
        #endregion

        #region public methods

        public void SetTrainingData(double[][] trainingdata, double[][] testdata)
        {
            Network.SetTrainingData(trainingdata, testdata);
        }

        public void Test()
        {
            Network.Test();
        }

        public void Validate()
        {
            Network.Validate();
        }

        public void TrainNetwork()
        {
            Network.Train(TrainingEpochs);
        }
        #endregion

        #region private methods

        #endregion

        #region properties
        protected NeuroNetworks.INeuroNetwork Network { get; set; }
        public List<IndicatorSignal> Signals { get; set; } = new List<IndicatorSignal>();
        public List<double[]> TestResult { get { return Network.TestResult; } }
        public List<double[]> TestExpected { get { return Network.TestExpected; } }

        public int TrainingEpochs { get; set; }
        #endregion
    }
}
