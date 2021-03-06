﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public class PriceTendencyIndicator: IndicatorBase
    {
        public PriceTendencyIndicator()
        {
            Intervals = 3;
            Network = new NeuroNetworks.PriceTendencyNetwork(Intervals, true, new int[] { 3, 4 }, 30);

        }


        public async void TrainNetwork()
        {
           
            List<double[]> convertedorig = (new Data.CryptocompareDataController()).GetDataDays();
            var train = convertedorig.Take((convertedorig.Count() - 300)).ToArray();
            var test3 = train.Last();
            var test2 = train[train.Count() - 2];
            var test = convertedorig.Skip((convertedorig.Count() - 300)).ToArray();
            Network.SetTrainingData(train, test);
            Network.Train(TrainingEpochs);
        }

        public int Intervals { get; set; }

    }
}
