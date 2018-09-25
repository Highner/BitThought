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
            BigBrain = new DeepThought.DeepThoughtNetwork(5, new int[] { 4, 3, 2 }, 2, ActivationFunction);
            BigBrain.CreateNetwork();
            
        }

        public Decimal ActivationFunction(decimal value)
        {
            return 0;
        }

       DeepThought.DeepThoughtNetwork BigBrain { get; set; }
    }
}
