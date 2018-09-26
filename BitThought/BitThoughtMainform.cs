using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitThought
{
    public partial class BitThoughtMainform : Form
    {
        public BitThoughtMainform()
        {
            InitializeComponent();

            BigBrain = new DeepThought.CryptoThoughtNetwork(4, new int[] { 3 }, 2, new DeepThought.ActivationFunctions.Swish(1.5));

            BigBrain.Train(Trainingdata(), 1);
        }

        private List<double[]> Trainingdata()
        {
            List<double[]> trainingdata = new List<double[]>();
            trainingdata.Add(new double[] { 0.01, 2, 3, 4 });
            trainingdata.Add(new double[] { 0.03, 2, 5, 1 });
            trainingdata.Add(new double[] { 0.03, 3, 4, 6 });
            trainingdata.Add(new double[] { 0.01, 2, 3, 4 });
            return trainingdata;
        }

       DeepThought.CryptoThoughtNetwork BigBrain { get; set; }
    }
}
