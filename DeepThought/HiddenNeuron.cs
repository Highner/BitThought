using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class HiddenNeuron: InputNeuron
    {
        public HiddenNeuron(Func<decimal, decimal> activationfunction, int layer) : base(activationfunction)
        {
            Layer = layer;
        }

        public HiddenNeuron(Func<decimal, decimal> activationfunction, int layer, bool isbias) : base(activationfunction)
        {
            Layer = layer;
            IsBias = isbias;
        }

        public int Layer { get; set; }
    }
}
