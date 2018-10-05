using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Data
{
    public class OHLCData
    {

        private dynamic dyn;
        
        public object ID { get { return dyn.time; } set { } }

        public double open { get { return dyn.open; } }

    }
}
