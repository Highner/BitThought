using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.ActivationFunctions
{
    public class Swish : IActivationFunction
    {
        public Swish(double beta = 1)
        {
            this.beta = beta;
        }
        public double Derivative(double value)
        {
            return 0;
        }

        public double ActivationFunction(double value)
        {
            double z = value / 2;
            double sig = (1 + Math.Tanh(z));
            double sigmoid = sig / 2;
            return value * sigmoid * beta;
        }

        private double beta;
    }
}
