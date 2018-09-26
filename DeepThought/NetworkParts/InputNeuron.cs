﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    class InputNeuron: NeuronBase
    {
        public InputNeuron(IActivationFunction activationfunction, int index) : base(activationfunction)
        {
            Index = index;
        }

        public int Index;
    }
}