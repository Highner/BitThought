using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public class MinMaxPriceChangeIndicator: IndicatorBase
    {
        #region contructor
        public MinMaxPriceChangeIndicator()
        {
            Network = new NeuroNetworks.MinMaxPriceChangeNetwork(20, 5);
        }
        #endregion

        public void TrainNetwork()
        {
            List<double[]> convertedorig = new List<double[]>();

            
            int maxintervalls = 8;
            for(int x = 0; x < maxintervalls; x++)
            {
                string timestamp = "";
                if (!(x == 0))
                {
                    var lalasa = convertedorig.Min(z => z[5]);
                    var lalwerasa = convertedorig.Max(z => z[5]);
                    timestamp = "&toTs=" + convertedorig.Min(z => z[5]);
                }

                string contents = "";
                string url = "https://min-api.cryptocompare.com/data/histohour?fsym=BTC&tsym=USD&limit=2000" + timestamp;
                using (var wc = new System.Net.WebClient())
                    contents = wc.DownloadString(url);

                dynamic JSONObj = Newtonsoft.Json.Linq.JObject.Parse(contents);

                int length = JSONObj["Data"].Count;
                for (int i = 0; i < length; i++)
                {
                    convertedorig.Add(ConvertFromSource(JSONObj["Data"][i]));
                }
            }

            convertedorig = convertedorig.OrderBy(x => x[5]).ToList();

            var lala = convertedorig.Select(x => x[5]).Distinct().Count();

            Network.SetTrainingData(convertedorig.Take(15000).ToArray(), convertedorig.Skip(15000).ToArray());
            Network.Train(10000);
        }

        #region private methods
        private double[] ConvertFromSource(dynamic item)
        {
            return new double[] { item["open"], item["close"], item["high"], item["low"], item["volumefrom"], item["time"] };
        }
        #endregion

        #region public properties
    
        #endregion
    }
}
