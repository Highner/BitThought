using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class OutputNeuron: NeuronBase
    {
        public OutputNeuron(Func<decimal, decimal> activationfunction) : base(activationfunction)
        {

        }
    }
}
