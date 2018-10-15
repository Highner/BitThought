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
        [Browsable(false)]
        public Command TrainNetworkCommand { get; set; }
        #endregion

        #region constructor
        public IndicatorViewModelBase(Indicators.IndicatorBase indicator)
        {
            _Indicator = indicator;
            LoadTrainingDataCommand = new Command(ExecuteLoadTrainingData);
            TrainNetworkCommand = new Command(ExecuteTrainNetwork);
        }
        #endregion

        #region public methods

        #endregion

        #region async methods
        async void ExecuteLoadTrainingData()
        {
            SetBusyStatus("Loading trainingdata");

            await Task.Run(() => LoadTrainingData());

            _Indicator.SetTrainingData(_TrainingData.ToArray(), _TestData.ToArray());

            SetNotBusyStatus();
        }
        async void ExecuteTrainNetwork()
        {
            SetBusyStatus("Training Network");

            await Task.Run(() => _Indicator.TrainNetwork());

            SetNotBusyStatus();
        }
        #endregion

        #region private methods
        void ResetStatus()
        {
            Status = "";
        }
        void SetBusyStatus(string status)
        {
            IsBusy = true;
            Status = status;
        }
        void SetNotBusyStatus()
        {
            IsBusy = false;
            ResetStatus();
        }
        #endregion

        #region overridable methods
        protected virtual void LoadTrainingData()
        {

        }
        #endregion

        #region private properties
        List<double[]> _TrainingData { get; set; }
        List<double[]> _TestData { get; set; }
        Indicators.IndicatorBase _Indicator { get; set; }
        #endregion

        #region public properties
        string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public int TrainingEpochs
        {
            get
            {
                return _Indicator.TrainingEpochs;
            }

            set
            {
                _Indicator.TrainingEpochs = value;
                OnPropertyChanged("TrainingEpochs");
            }
        }



        #endregion
    }
}
