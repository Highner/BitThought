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
            Network = new NeuroNetworks.NextIntervalNetwork(intervals);
        }


        public void TrainNetwork(int epochs)
        {
            List<double[]> convertedorig = new List<double[]>();

            int limit = 2000;
            int maxintervalls = 5;
            for (int x = 0; x < maxintervalls; x++)
            {
                string timestamp = "";
                if (!(x == 0))
                {
                    var lalasa = convertedorig.Min(z => z[5]);
                    var lalwerasa = convertedorig.Max(z => z[5]);
                    timestamp = "&toTs=" + convertedorig.Min(z => z[5]);
                }

                string contents = "";
                string url = "https://min-api.cryptocompare.com/data/histohour?fsym=BTC&tsym=USD&limit=" + limit + timestamp;
                using (var wc = new System.Net.WebClient())
                    contents = wc.DownloadString(url);

                dynamic JSONObj = Newtonsoft.Json.Linq.JObject.Parse(contents);

                int length = JSONObj["Data"].Count;
                for (int i = 0; i < length; i++)
                {
                    convertedorig.Add(ConvertFromSource(JSONObj["Data"][i]));
                }
            }

            convertedorig = convertedorig.OrderBy(x => x[5]).Distinct().ToList();

            Network.SetTrainingData(convertedorig.Take((convertedorig.Count() - 1000)).ToArray(), convertedorig.Skip((convertedorig.Count() - 1000)).ToArray());
            Network.Train(epochs);
        }

        private double[] ConvertFromSource(dynamic item)
        {
            return new double[] { item["open"], item["close"], item["high"], item["low"], item["volumefrom"], item["time"] };
        }

        public int Intervals { get; set; }
      
    }

}
