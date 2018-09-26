using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.ActivationFunctions
{
    public  class Sigmoid : IActivationFunction
    {
        public double Derivative(double value)
        {
            return 0;
        }

        public double ActivationFunction(double value)
        {
            double x = value;
            double z = x / 2;
            double sig = (1 + Math.Tanh(z));
            double sigmoid = sig / 2;
            return sigmoid;
        }
    }
}
