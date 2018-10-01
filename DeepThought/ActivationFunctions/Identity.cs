using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.ActivationFunctions
{
    class Identity: IActivationFunction
    {
        public double Derivative(double value)
        {
            return 1;
        }

        public double ActivationFunction(double value)
        {
            return value;
        }
    }
}
