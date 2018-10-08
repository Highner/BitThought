using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public class NextIntervalIndicator: IndicatorBase
    {
        public NextIntervalIndicator(int intervals)
        {
            Intervals = intervals;
            Network = new NeuroNetworks.NextIntervalNetwork(intervals, new int[] { 1});
        }


        public void TrainNetwork(int epochs)
        {
            List<double[]> convertedorig = (new Data.CryptocompareDataController()).GetDataHours();
            Network.SetTrainingData(convertedorig.Take((convertedorig.Count() - 1000)).ToArray(), convertedorig.Skip((convertedorig.Count() - 1000)).ToArray());
            Network.Train(epochs);
        }

        public int Intervals { get; set; }
      
    }

}
