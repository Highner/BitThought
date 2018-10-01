using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;

namespace DeepThought
{
    public class WineThought: DeepThoughtNetwork
    {
        public WineThought(double learningrate): base(13, new int[] { 35,35 }, 1, Data.EnumActivationFunctions.Identity)
        {
            SetLearningRate(learningrate);
            CreateNetwork();
        }

        #region public methods

        #endregion


  
    }
}
