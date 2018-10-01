using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Linq;
using Accord;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;
using Accord.Statistics.Filters;

namespace BitThought
{
    public partial class BitThoughtMainform : Form
    {
        public BitThoughtMainform()
        { 
            InitializeComponent();

            WineBrain = new DeepThought.WineThought(1);

            GetTrainingData();
            //WineBrain.Train(Trainingdata, Expectedvalues, 1000);

            //var testdata = WineBrain.GetResult(Trainingdata[0]);
            //double[][] inputs = Trainingdata.ToArray();

            //// Get only the output labels (last column)
            //int[] outputs = newespected.ToArray();


            //Accord.MachineLearning.DecisionTrees.DecisionTree asad;

            //DecisionVariable[] variables =
            //{
            //    new DecisionVariable("x", new DoubleRange( Trainingdata[0].Min(), Trainingdata[0].Max())),
            //    new DecisionVariable("y", new DoubleRange( Trainingdata[1].Min(), Trainingdata[1].Max())),
            //    new DecisionVariable("x1", new DoubleRange( Trainingdata[2].Min(), Trainingdata[2].Max())),
            //    new DecisionVariable("y2", new DoubleRange( Trainingdata[3].Min(), Trainingdata[3].Max())),
            //    new DecisionVariable("x3", new DoubleRange( Trainingdata[4].Min(), Trainingdata[4].Max())),
            //    new DecisionVariable("y4", new DoubleRange( Trainingdata[5].Min(), Trainingdata[5].Max())),
            //    new DecisionVariable("x5", new DoubleRange( Trainingdata[6].Min(), Trainingdata[6].Max())),
            //    new DecisionVariable("y6", new DoubleRange( Trainingdata[7].Min(), Trainingdata[7].Max())),
            //    new DecisionVariable("x7", new DoubleRange( Trainingdata[8].Min(), Trainingdata[8].Max())),
            //    new DecisionVariable("y8", new DoubleRange( Trainingdata[9].Min(), Trainingdata[9].Max())),
            //    new DecisionVariable("x9", new DoubleRange( Trainingdata[10].Min(), Trainingdata[10].Max())),
            //    new DecisionVariable("y0", new DoubleRange( Trainingdata[11].Min(), Trainingdata[11].Max())),
            //    new DecisionVariable("x11", new DoubleRange( Trainingdata[12].Min(), Trainingdata[12].Max()))
               
            //};
            //var c45 = new C45Learning(variables);
            
            //tree = c45.Learn(inputs, outputs);

         

            var neuroNetwork = new Accord.Neuro.ActivationNetwork(new Accord.Neuro.IdentityFunction(),  13, new int[] { 10 , 1 });
            var deepLearning = new Accord.Neuro.Learning.ParallelResilientBackpropagationLearning(neuroNetwork);
            //deepLearning.Algorithm = (activationNetwork, index) => new BackPropagationLearning(activationNetwork);

            //// Setup the learning algorithm.
            //BackPropagationLearning teacher = new BackPropagationLearning(neuroNetwork)
            //{

            //        LearningRate = 0.01,
            //        Momentum = 0.5

            //};

            //List<double[]> list = new List<double[]>();

            //foreach (double[] item in Trainingdata)
            //{
            //    list.Add(NormalizeStretch(item, 0, 1));
            //}

            //Trainingdata = list;

            var asd = neuroNetwork.Compute(Trainingdata[0]);
            double[][] layerData = Trainingdata.ToArray();
            double[][] outpuata = Expectedvalues.ToArray();

            List<double> errorfun = new List<double>();
            
         
            for (int i = 0; i < 100000; i++)
            {
                //teacher.RunEpoch(layerData, outpuata);
               errorfun.Add( deepLearning.RunEpoch(layerData, outpuata));

            }


            //var asdsa1 = neuroNetwork.Compute(new double[] { 1,2 });
            var asdsa = neuroNetwork.Compute(Trainingdata[0]);
            var asdsa2 = neuroNetwork.Compute(Trainingdata[1]);
            var asdsa3 = neuroNetwork.Compute(Trainingdata[2]);
            var asdsa4 = neuroNetwork.Compute(Trainingdata[3]);

            //var test = neuroNetwork.Compute(new double[] { 5, 6 });

            actual = tree.Decide(Trainingdata[0]);
            actual2 = tree.Decide(Trainingdata[1]);
            actual3 = tree.Decide(Trainingdata[2]);
            actual4 = tree.Decide(Trainingdata[3]);
        }

        private void GetTrainingData()
        {

            //Trainingdata.Add(new double[] { 1, 2 });
            //Expectedvalues.Add(new double[] { 3 });
            //newespected.Add(3);


            //Trainingdata.Add(new double[] { 3, 2 });
            //Expectedvalues.Add(new double[] { 5 });
            //newespected.Add(5);


            //Trainingdata.Add(new double[] { 2, 2 });
            //Expectedvalues.Add(new double[] { 4 });
            //newespected.Add(4);


            //Trainingdata.Add(new double[] { 4, 2 });
            //Expectedvalues.Add(new double[] { 6 });
            //newespected.Add(4);


            //Trainingdata.Add(new double[] { 2, 6 });
            //Expectedvalues.Add(new double[] { 8 });
            //newespected.Add(4);


            //Trainingdata.Add(new double[] { 3, 4 });
            //Expectedvalues.Add(new double[] { 7 });
            //newespected.Add(4);

            //Trainingdata.Add(new double[] { 5 });
            //Expectedvalues.Add(new double[] { 5 });

            var trainingdata = GetTrainingdata();

            foreach (double[] item in trainingdata)
            {
                Trainingdata.Add(item.Skip(1).ToArray());
                Expectedvalues.Add(new double[] { item[0] });
                newespected.Add((int)item[0]);
            }

        }

        public static double[] NormalizeStretch(double[] array, double newMin, double newMax)
        {
            double minValue = double.MaxValue;
            double maxValue = double.MinValue;

            for (int index = 0; index < array.Length; index++)
            {
                if (array[index] < minValue) minValue = array[index];
                if (array[index] > maxValue) maxValue = array[index];
            }

            double scaler = (newMax - newMin) / (maxValue - minValue);

            for (int index = 0; index < array.Length; index++)
            {
                array[index] = (array[index] - minValue) * scaler + newMin;
            }

            return array;
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

        private List<int> newespected = new List<int>();

        public DecisionTree tree;

        public int actual;
        public int actual2;
        public int actual3;
        public int actual4;
    }
}
