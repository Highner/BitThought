using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class HiddenNeuron: NeuronBase
    {
        public HiddenNeuron(Data.EnumActivationFunctions activationfunction, int layer, double learningrate) : base(activationfunction, learningrate)
        {
            Layer = layer;
        }

        public HiddenNeuron(Data.EnumActivationFunctions activationfunction, int layer, bool isbias, double learningrate) : base(activationfunction, learningrate)
        {
            Layer = layer;
            IsBias = isbias;
        }


        public int Layer { get; set; }
    }
}
