using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.NeuroNetworks
{
    public class PriceTendencyNetwork: Base.ActivationNetworkBase
    {
        #region constructor
        public PriceTendencyNetwork(int intervals, bool scale, int[] inputdistribution, int inputintervals) : base(new Accord.Neuro.SigmoidFunction(0.5), (inputintervals * inputdistribution.Count()), new int[] {100, 100, 100, 3 }, inputdistribution)
        {
            _Intervals = intervals;
            _ScaleData = scale;
        }
        #endregion

        #region base methods
        //protected override void Convert(double[][] source_ohlcv, out double[][] input, out double[][] output)
        //{
        //    List<double[]> _input = new List<double[]>();
        //    List<double[]> _output = new List<double[]>();

        //    for (int i = 0; i < source_ohlcv.Count() - _InputNeurons - _Intervals; i++)
        //    {
        //        List<double> _neuronlist = new List<double>();
        //        List<double> _followlist = new List<double>();

        //        for (int y = i; y < i + _InputNeurons ; y++)
        //        {
        //            _neuronlist.Add(source_ohlcv[y][3]);
        //        }

        //        double lastprice = source_ohlcv[(i + _InputNeurons - 1)][3];

        //        for (int x = i + _InputNeurons; x < i + _InputNeurons + _Intervals; x++)
        //        {
        //            _followlist.Add(source_ohlcv[x][3]);
        //        }

        //        double[] result;
        //        if (_followlist.Max() > lastprice && _followlist.Min() > lastprice)
        //        {
        //            result = new double[] { 1, 0, 0 };
        //        }
        //        else if (_followlist.Max() > lastprice && _followlist.Min() < lastprice)
        //        {
        //            result = new double[] { 0, 1, 0 };
        //        }
        //        else
        //        {
        //            result = new double[] { 0, 0, 1 };
        //        }

        //        _input.Add(_neuronlist.ToArray());

        //        _output.Add(result);

        //    }

        //    input = _input.ToArray();
        //    output = _output.ToArray();
        //}

        protected override void Convert(double[][] source_ohlcv, int[] distribution, out double[][] input, out double[][] output)
        {
            List<double[]> _input = new List<double[]>();
            List<double[]> _output = new List<double[]>();

            int variables = distribution.Count();

            for (int i = 0; i < source_ohlcv.Count() - _InputNeurons - _Intervals; i++)
            {
                List<double> _neuronlist = new List<double>();
                List<double> _followlist = new List<double>();

                for (int y = i; y < i + (_InputNeurons / variables); y++)
                {
                    for(int x = 0; x < variables; x ++)
                    {
                        _neuronlist.Add(source_ohlcv[y][distribution[x]]);
                    }
                }

                double lastprice = source_ohlcv[(i + (_InputNeurons / variables) - 1)][3];

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
            }

            AggregateResult = new int[] { Result.Count(x => x == 0), Result.Count(x => x == 1) };
            var good = AggregateResult[1];
            var total = AggregateResult.Sum();
            TestPercentage = (double)good / total;
        }

        public override void Validate()
        {
            int[] lala;// = null;
            int testlength = _TrainingInput.Length;
            for (int i = 0; i < testlength; i+= 10)
            {
                double[] computed = Compute(_TrainingInput[i]);

                double maxValuecomputed = computed.Max();
                int maxIndexcomputed = computed.ToList().IndexOf(maxValuecomputed);

                double maxValuereal = _TrainingOutput[i].Max();
                int maxIndexreal = _TrainingOutput[i].ToList().IndexOf(maxValuereal);

                if (maxIndexcomputed == maxIndexreal)
                {
                    ValResult.Add(1);
                }
                else
                {
                    ValResult.Add(0);
                }

               

            }
            lala = new int[] { ValResult.Count(x => x == 0), ValResult.Count(x => x == 1) };

            var good = lala[1];
            var total = lala.Sum();
            ValidationPercentage = (double)good / total;
        }
        #endregion

        #region properties
        private int _Intervals { get; set; }
        public List<int> Result { get; set; } = new List<int>();
        public List<int> ValResult { get; set; } = new List<int>();
        public int[] AggregateResult { get; set; }

        #endregion
    }
}
