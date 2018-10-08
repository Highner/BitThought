﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Data
{
    public class CryptocompareDataController: Controllers.IOHLCDataController
    {
        #region public methods
        public List<double[]> GetDataDays()
        {
            return GetData("day");
        }

        public List<double[]> GetDataHours()
        {
            return GetData("hour");
        }

        public List<double[]> GetDataMinutes()
        {
            return GetData("minute");
        }
        #endregion

        #region private methods
        private List<double[]> GetData(string interval)
        {
            List<double[]> convertedorig = new List<double[]>();

            int limit = 2000;
            int maxintervals = 5;
            for (int x = 0; x < maxintervals; x++)
            {
                string timestamp = "";
                if (!(x == 0))
                {
                    timestamp = "&toTs=" + convertedorig.Min(z => z[5]);
                }

                string contents = "";
                string url = "https://min-api.cryptocompare.com/data/histo" + interval + "?fsym=BTC&tsym=USD&limit=" + limit + timestamp;
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
            return convertedorig;
        }

        private double[] ConvertFromSource(dynamic item)
        {
            return new double[] { item["open"], item["close"], item["high"], item["low"], item["volumefrom"], item["time"] };
        }
        #endregion
    }
}