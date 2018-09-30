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
            return ActivationFunction(value) + _Sigmoid.ActivationFunction(value) * (1 - ActivationFunction(value));
        }

        public double ActivationFunction(double value)
        {
            //double z = value / 2;
            //double sig = (1 + Math.Tanh(z));
            //double sigmoid = sig / 2;
            //return value * sigmoid * beta;

            return value * beta * _Sigmoid.ActivationFunction(value); // beta * (1 / (1 + Math.Pow(2.71828, -value)));
        }

        private double beta;
        private IActivationFunction _Sigmoid = new Sigmoid();
    }
}
