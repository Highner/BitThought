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

            BigBrain = new DeepThought.CryptoThoughtNetwork(4, new int[] { 3 }, 2, ActivationFunction);

            BigBrain.Train(Trainingdata(), 1);
        }

        public double ActivationFunction(double value)
        {
            double x = (double)value;
            double z = x / 2;
            double sig = (1 + Math.Tanh(z));
            double sigmoid = sig / 2;
            double y =(double)(x * sigmoid);
            return y;
        }

        private List<double[]> Trainingdata()
        {
            List<double[]> trainingdata = new List<double[]>();
            trainingdata.Add(new double[] { 1, 2, 3, 4 });
            trainingdata.Add(new double[] { 3, 2, 5, 1 });
            trainingdata.Add(new double[] { 3, 3, 4, 6 });
            trainingdata.Add(new double[] { 1, 2, 3, 4 });
            return trainingdata;
        }

       DeepThought.CryptoThoughtNetwork BigBrain { get; set; }
    }
}
