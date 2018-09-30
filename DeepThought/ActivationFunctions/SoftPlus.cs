using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.ActivationFunctions
{
    public class SoftPlus: IActivationFunction
    {
        public double Derivative(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -value));
        }

        public double ActivationFunction(double value)
        {
            return Math.Log(Math.Exp(value) + 1);
        }
    }
}
