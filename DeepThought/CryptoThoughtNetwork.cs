using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    public class CryptoThoughtNetwork: DeepThoughtNetwork
    {
        #region constructor
        public CryptoThoughtNetwork()
        { }
        public CryptoThoughtNetwork(int numberofinputneurons, int[] layers, int numberofoutputneurons, Func<decimal, decimal> activationfunction): base(numberofinputneurons, layers, numberofoutputneurons, activationfunction)
        {
            CreateNetwork();
        }
        #endregion

        #region public methods
        public void Train(List<decimal[]> trainingdata, int trainingoffset)
        {
            if (trainingdata.Count() < trainingoffset)
            {
                Console.WriteLine("Not enough datasets");
            }
            else if (!ValidateData(trainingdata[0]))
            {
                Console.WriteLine("Wrong Dataformat");
            }
            else
            {
                TrainingData = trainingdata;
                TrainingOffset = trainingoffset;
                for (int i = 1; i < trainingdata.Count() - trainingoffset; i++)
                {
                    Train(TrainingData[i], TrainingData[i + trainingoffset]);
                }
            }
        }
        #endregion

        #region private methods
        private bool ValidateData(decimal[] inputdata)
        {
            return (inputdata.Count() == NumberOfInputNeurons);
        }

        private void Train(decimal[] inputdata, decimal[] expecteddata)
        {

        }

        private decimal CalculateLoss(decimal[] expectedvalues, decimal[] actualvalues)
        {
            decimal sum = 0;
            for (int i = 1; i < expectedvalues.Count(); i++)
            {
                decimal error = expectedvalues[i] - actualvalues[i];
                sum += error * error;
            }
            return sum;
        }
        #endregion

        #region fields
        private int TrainingOffset;

        private List<decimal[]> TrainingData { get; set; }
        #endregion  
    }
}
