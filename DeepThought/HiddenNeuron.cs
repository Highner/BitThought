using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class HiddenNeuron: NeuronBase
    {
        public HiddenNeuron(Func<double, double> activationfunction, int layer) : base(activationfunction)
        {
            Layer = layer;
        }

        public HiddenNeuron(Func<double, double> activationfunction, int layer, bool isbias) : base(activationfunction)
        {
            Layer = layer;
            IsBias = isbias;
        }

        public int Layer { get; set; }
    }
}
