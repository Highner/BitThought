using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Linq;


namespace BitThought
{
    public partial class BitThoughtMainform : Form
    {
        public BitThoughtMainform()
        { 
            InitializeComponent();

            WineBrain = new DeepThought.WineThought(0.1);

            GetTrainingData();
            WineBrain.Train(Trainingdata, Expectedvalues, 10);

         }

        private void GetTrainingData()
        {


            //Trainingdata.Add(new double[] { 1, 2 });
            //Expectedvalues.Add(new double[] { 3 });

            //Trainingdata.Add(new double[] { 2, 2 });
            //Expectedvalues.Add(new double[] { 4 });

            //Trainingdata.Add(new double[] { 3, 2 });
            //Expectedvalues.Add(new double[] { 5 });


            var trainingdata = GetTrainingdata();

            foreach (double[] item in trainingdata)
            {
                Trainingdata.Add(item.Skip(1).ToArray());
                Expectedvalues.Add(new double[] { item[0] });
            }




        }

        private List<double[]> GetTrainingdata()
        {
            List<double[]> trainingdata = new List<double[]>();
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              "http://archive.ics.uci.edu/ml/machine-learning-databases/wine/wine.data");
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();


            string[] values = responseFromServer.Split('\n');

            for (int i = 0; i < values.Length - 1; i++)
            {
                values[i] = values[i].Trim();
                string[] lala = values[i].Split(',');

                List<double> listrsr = new List<double>();
                foreach (string str in lala)
                {
                    listrsr.Add(Convert.ToDouble(str.Replace(str, "0" + str).Replace(".", ",")));
                }
                trainingdata.Add(listrsr.ToArray());
            }

            // Clean up the streams and the response.  
            reader.Close();
            response.Close();
            var shuffledcards = trainingdata.OrderBy(a => Guid.NewGuid()).ToList();
            return shuffledcards;
        }


        DeepThought.CryptoThoughtNetwork BigBrain { get; set; }

        DeepThought.WineThought WineBrain { get; set; }

        private List<double[]> Trainingdata { get; set; } = new List<double[]>();
        private List<double[]> Expectedvalues { get; set; } = new List<double[]>();
    }
}
