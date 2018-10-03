using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Indicators
{
    public abstract class IndicatorBase : IIndicator
    {
        #region constructor

        #endregion

        #region public methods
        public void Train(int epochs)
        {
            Network.Train(epochs);
        }

        public void Test()
        {
            Network.Test();
        }
        #endregion

        #region properties
        protected NeuroNetworks.INeuroNetwork Network { get; set; }
        public List<IndicatorSignal> Signals { get; set; } = new List<IndicatorSignal>();
        public List<double[]> TestResult { get { return Network.TestResult; } }
        public List<double[]> TestExpected { get { return Network.TestExpected; } }
        #endregion
    }
}
