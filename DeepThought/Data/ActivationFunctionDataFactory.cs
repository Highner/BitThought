using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought.Data
{
    public static class ActivationFunctionDataFactory
    {
        public static IActivationFunction GenerateActivationFunction(EnumActivationFunctions functiontype)
        {
            switch (functiontype)
            {
                case EnumActivationFunctions.Sigmoid:
                    return new ActivationFunctions.Sigmoid();
                case EnumActivationFunctions.Swish:
                    return new ActivationFunctions.Swish();
                default:
                    return null;
            }
        }
    }
}
