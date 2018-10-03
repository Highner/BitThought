using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Linq;
using Accord;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;
using Accord.Statistics.Filters;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace BitThought
{
    public partial class BitThoughtMainform : Form
    {
        public BitThoughtMainform()
        { 
            InitializeComponent();

            //Indicator = new Indicators.MinMaxPriceChangeIndicator();

            Indicator = new Indicators.NextIntervalIndicator();

            Indicator.TrainNetwork();
            Indicator.Test();

            List<double[]> newlist = new List<double[]>();
            List<double[]> emptylist = new List<double[]>();

            //for (int i = 0; i < Indicator.TestResult.Count(); i++)
            //{
            //    newlist.Add(new double[] { Indicator.TestResult[i][0], Indicator.TestResult[i][1], Indicator.TestResult[i][2], Indicator.TestExpected[i][0], Indicator.TestExpected[i][1] });
            //    emptylist.Add(new double[] { 0 });
            //}


            //chart1.Series.Clear();
            //Series series = chart1.Series.Add("Calculated Min");
            //series.ChartType = SeriesChartType.Column;
            //series.Points.DataBindY(newlist.Select(x => x[0]).ToList());

            //Series series3 = chart1.Series.Add("Real Min");
            //series3.ChartType = SeriesChartType.Column;
            //series3.Points.DataBindY(newlist.Select(x => x[3]).ToList());

            //Series series6 = chart1.Series.Add("Gap");
            //series6.ChartType = SeriesChartType.Column;
            //series6.Points.DataBindY(emptylist.Select(x => x[0]).ToList());

            //Series series2 = chart1.Series.Add("Calculated Max");
            //series2.ChartType = SeriesChartType.Column;
            //series2.Points.DataBindY(newlist.Select(x => x[1]).ToList());

            //Series series4 = chart1.Series.Add("Real Max");
            //series4.ChartType = SeriesChartType.Column;
            //series4.Points.DataBindY(newlist.Select(x => x[4]).ToList());

            //Series series5 = chart1.Series.Add("Last Close");
            //series5.ChartType = SeriesChartType.Line;
            //series5.Points.DataBindY(newlist.Select(x => x[2]).ToList());

            chart1.Series.Clear();

            for(int i = 0; i < 3; i++)
            {
                Series series = chart1.Series.Add("Computed " + i);
                series.ChartType = SeriesChartType.Column;
                series.Points.DataBindY(Indicator.TestResult.Select(x => x[i]).ToList());

            }


            for (int i = 0; i < 3; i++)
            {
  

                Series series2 = chart1.Series.Add("Real " + i);
                series2.ChartType = SeriesChartType.Column;
                series2.Points.DataBindY(Indicator.TestExpected.Select(x => x[i]).ToList());
            }

        }




        // private Indicators.MinMaxPriceChangeIndicator Indicator;


        private Indicators.NextIntervalIndicator Indicator;
    }
}
