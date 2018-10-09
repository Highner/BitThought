using HynrFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BitThought.ViewModels.Base
{
    public abstract class IndicatorViewModelBase: HynrFramework.ViewModelBase
    {
        #region commands
        [Browsable(false)]
        public Command LoadTrainingDataCommand { get; set; }
        #endregion

        #region constructor
        public IndicatorViewModelBase(HynrFramework.ListViewModelBase<double[]> trainingdatalvm, Indicators.IndicatorBase indicator)
        {
            TrainingDataListViewModel = trainingdatalvm;
            Indicator = indicator;
            LoadTrainingDataCommand = new Command(LoadTrainingData);
        }
        #endregion

        #region public methods

        #endregion

        #region private methods
        void LoadTrainingData()
        {
            TrainingDataListViewModel.RefreshAllCommand.Execute();
        }
        #endregion

        #region properties
        HynrFramework.ListViewModelBase<double[]> TrainingDataListViewModel { get; set; }
        Indicators.IndicatorBase Indicator { get; set; }
        #endregion
    }
}
