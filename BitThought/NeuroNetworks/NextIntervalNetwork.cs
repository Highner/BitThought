﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public class NextIntervalNetwork: Base.ActivationNetworkBase
    {
        #region constructor
        public NextIntervalNetwork(int intervals, int[] inputdistribution) : base(new Accord.Neuro.IdentityFunction(), 20, new int[] { 40, 40, intervals }, inputdistribution)
        {
            _Intervals = intervals;
            _ScaleData = true;
        }
        #endregion

        #region base methods
        protected override void Convert(double[][] source_ohlcv, int[] distribution, out double[][] input, out double[][] output)
        {
            List<double[]> _input = new List<double[]>();
            List<double[]> _output = new List<double[]>();

            for (int i = 0; i < source_ohlcv.Count() - _InputNeurons - _Intervals; i++)
            {
                List<double> _neuronlist = new List<double>();
                List<double> _followlist = new List<double>();

                for (int y = i; y < i + (_InputNeurons / 2); y++)
                {
                    _neuronlist.Add(source_ohlcv[y].Take(4).Average());
                    _neuronlist.Add(source_ohlcv[y][4]);
                }

                for (int x = i + _InputNeurons; x < i + _InputNeurons + _Intervals; x++)
                {
                    _followlist.Add(source_ohlcv[x].Take(4).Average());
                }

                _input.Add(_neuronlist.ToArray());

                _output.Add(_followlist.ToArray());

            }

            input = _input.ToArray();
            output = _output.ToArray();
        }
        #endregion

        #region properties
        private int _Intervals { get; set; }
        #endregion
    }
}
