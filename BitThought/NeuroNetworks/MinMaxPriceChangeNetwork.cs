using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public class MinMaxPriceChangeNetwork: NeuroNetworks.Base.ActivationNetworkBase 
    {
        #region constructor
        public MinMaxPriceChangeNetwork(int inputneurons, int advanceintervall, int[] inputdistribution) : base(new Accord.Neuro.IdentityFunction(), inputneurons, new int[] { 40, 35, 30, 2 }, inputdistribution)
        {
            AdvanceIntervall = advanceintervall;
        }
        #endregion

        #region base methods
        protected override void Convert(double[][] source_ohlcv, int[] distribution, out double[][] input, out double[][] output)
        {
            List<double[]> _input = new List<double[]>();
            List<double[]> _output = new List<double[]>();

            for(int i = 0; i < source_ohlcv.Count() - _InputNeurons - AdvanceIntervall; i++)
            {
                List<double> _neuronlist = new List<double>();
                List<double> _followlist = new List<double>();

                for(int y = i; y < i + (_InputNeurons / 2); y++)
                {
                    _neuronlist.Add(source_ohlcv[y][3]);
                    _neuronlist.Add(source_ohlcv[y][4]);
                }

                for (int x = i + _InputNeurons; x < i + _InputNeurons + AdvanceIntervall; x++)
                {
                    _followlist.Add(source_ohlcv[x].Take(4).Max());
                    _followlist.Add(source_ohlcv[x].Take(4).Min());
                }

                _input.Add(_neuronlist.ToArray());

                _output.Add(CalculateMinMax(_followlist.ToArray()));

            }

            input = _input.ToArray();
            output = _output.ToArray();
        }

        public override void Test()
        {
            TestResult.Clear();
            TestExpected.Clear();

            int testlength = _TestInput.Length;
            for (int i = 0; i < testlength; i++)
            {
                double[] computed = Compute(_TestInput[i]);
                double[] result = new double[] { computed[0], computed[1], _TestInput[i][4] };
                TestResult.Add(result);
                TestExpected.Add(_TestOutput[i]);
            }
        }
        #endregion

        #region private methods
        double[] CalculateMinMax(double[] followingvalues)
        {
            return new double[] { followingvalues.Min(), followingvalues.Max() };
        }
        #endregion

        #region properties
        public int AdvanceIntervall { get; set; }
        #endregion
    }
}