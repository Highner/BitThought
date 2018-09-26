using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    public interface IActivationFunction
    {
        double ActivationFunction(double value);

        double Derivative(double value);
    }
}
