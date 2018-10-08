using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public class PriceTendencyNetwork: NeuroNetworkBase
    {
        #region constructor
        public PriceTendencyNetwork(int intervals) : base(new Accord.Neuro.IdentityFunction(), 10, new int[] { 20, 20, 3 })
        {
            _Intervals = intervals;
            _ScaleData = true;
        }
        #endregion

        #region base methods
        protected override void Convert(double[][] source_ohlcv, out double[][] input, out double[][] output)
        {
            List<double[]> _input = new List<double[]>();
            List<double[]> _output = new List<double[]>();

            for (int i = 0; i < source_ohlcv.Count() - _InputNeurons - _Intervals; i++)
            {
                List<double> _neuronlist = new List<double>();
                List<double> _followlist = new List<double>();

                for (int y = i; y < i + (_InputNeurons / 2); y++)
                {
                    _neuronlist.Add(source_ohlcv[y][3]);
                    _neuronlist.Add(source_ohlcv[y][4]);
                }

                double lastprice = source_ohlcv[(i + (_InputNeurons / 2) - 1)][3];

                for (int x = i + _InputNeurons; x < i + _InputNeurons + _Intervals; x++)
                {
                    _followlist.Add(source_ohlcv[x][3]);
                }

                double[] result;
                if (_followlist.Max() > lastprice && _followlist.Min() > lastprice)
                {
                    result = new double[] { 1, 0, 0 };
                }
                else if (_followlist.Max() > lastprice && _followlist.Min() < lastprice)
                {
                    result = new double[] { 0, 1, 0 };
                }
                else
                {
                    result = new double[] { 0, 0, 1 };
                }

                _input.Add(_neuronlist.ToArray());

                _output.Add(result);

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

                double maxValuecomputed = computed.Max();
                int maxIndexcomputed = computed.ToList().IndexOf(maxValuecomputed);

                double maxValuereal = _TestOutput[i].Max();
                int maxIndexreal = _TestOutput[i].ToList().IndexOf(maxValuereal);

                if(maxIndexcomputed == maxIndexreal)
                {
                    Result.Add(1);
                }
                else
                {
                    Result.Add(0);
                }

                AggregateResult = new int[] { Result.Count(x => x == 0), Result.Count(x => x == 1) };
                //TestResult.Add(result);
                //TestExpected.Add(_TestOutput[i]);
            }
        }
        #endregion

        #region properties
        private int _Intervals { get; set; }
        public List<int> Result { get; set; } = new List<int>();
        public int[] AggregateResult { get; set; }
        #endregion
    }
}
