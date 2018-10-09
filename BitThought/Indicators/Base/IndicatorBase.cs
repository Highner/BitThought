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

        public void GetTrainingData()
        {

        }
        public void Train(int epochs)
        {
            Network.Train(epochs);
        }

        public void Test()
        {
            Network.Test();
        }

        public void Validate()
        {
            Network.Validate();
        }

        public async void TrainNetwork()
        {

            List<double[]> convertedorig = (new Data.CryptocompareDataController()).GetDataDays();
            var train = convertedorig.Take((convertedorig.Count() - 300)).ToArray();
            var test3 = train.Last();
            var test2 = train[train.Count() - 2];
            var test = convertedorig.Skip((convertedorig.Count() - 300)).ToArray();
            Network.SetTrainingData(train, test);
            Network.Train(_TrainingEpochs);
        }
        #endregion

        #region properties
        protected NeuroNetworks.INeuroNetwork Network { get; set; }
        public List<IndicatorSignal> Signals { get; set; } = new List<IndicatorSignal>();
        public List<double[]> TestResult { get { return Network.TestResult; } }
        public List<double[]> TestExpected { get { return Network.TestExpected; } }

        protected int _TrainingEpochs { get; set; }
        #endregion
    }
}
