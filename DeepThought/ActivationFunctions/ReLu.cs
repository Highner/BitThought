using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.ActivationFunctions
{
    public class ReLU : IActivationFunction
    {
        public double Derivative(double value)
        {
            return Math.Max(0, 1); 
        }

        public double ActivationFunction(double value)
        {
            return Math.Max(0, value);
        }
    }
}
