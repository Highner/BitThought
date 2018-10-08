using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitThought.Data.Controllers
{
    public interface IOHLCDataController
    {
        List<double[]> GetDataDays();
        List<double[]> GetDataHours();
        List<double[]> GetDataMinutes();
    }
}
