using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public class PriceTendencyIndicator: IndicatorBase
    {
        public PriceTendencyIndicator(int intervals)
        {
            Intervals = intervals;
            Network = new NeuroNetworks.PriceTendencyNetwork(intervals);
        }


        public void TrainNetwork(int epochs)
        {
            List<double[]> convertedorig = (new Data.CryptocompareDataController()).GetDataHours();
            var train = convertedorig.Take((convertedorig.Count() - 1000)).ToArray();
            var test = convertedorig.Skip((convertedorig.Count() - 1000)).ToArray();
            Network.SetTrainingData(train, test);
            Network.Train(epochs);
        }

        public int Intervals { get; set; }
    }
}
