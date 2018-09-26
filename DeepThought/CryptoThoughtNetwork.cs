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
        public CryptoThoughtNetwork(int numberofinputneurons, int[] layers, int numberofoutputneurons, IActivationFunction activationfunction): base(numberofinputneurons, layers, numberofoutputneurons, activationfunction)
        {
            CreateNetwork();
        }
        #endregion

        #region public methods
        public void Train(List<double[]> trainingdata, int resultoffset)
        {
           if (ValidateData(trainingdata, resultoffset))
            {
                TrainingData = trainingdata;
                TrainingOffset = resultoffset;
                for (int i = 1; i < trainingdata.Count() - resultoffset; i++)
                {
                    TrainCycle(TrainingData[i], TrainingData[i + resultoffset]);
                }
            }
        }
        #endregion

        #region private methods
        private bool ValidateData(List<double[]> trainingdata, int resultoffset)
        {
            if (trainingdata.Count() < resultoffset)
            {
                Console.WriteLine("Not enough datasets");
                return false;
            }
            else if (!(trainingdata[0].Count() == NumberOfInputNeurons))
            {
                Console.WriteLine("Wrong Dataformat");
                return false;
            }
            return true;
        }
        #endregion

        #region fields
        private int TrainingOffset;
        #endregion  
    }
}
