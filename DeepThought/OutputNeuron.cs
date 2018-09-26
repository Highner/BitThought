using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class OutputNeuron: NeuronBase
    {
        #region constructor
        public OutputNeuron(Func<double, double> activationfunction) : base(activationfunction)
        {

        }
        #endregion

        #region properties
        public double OutputValue
        { get
            {
                return this.GetOutputValue();
            }
        }
        #endregion
    }
}
