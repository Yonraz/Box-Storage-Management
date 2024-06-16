using BoxStorage.Models;
using BoxStorageUI.Command;
using System.Windows;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    class UserSettingsViewModel : ViewModelBase
    {
        private int _minAmount;
        private int _maxAmount;
        private int _timeToExpire;
        private int _deviationPercentage;
        private int _expiryTimeForBoxView;

        public int MinAmount
        {
            get { return _minAmount; }
            set
            {
                _minAmount = value;
                OnPropertyChanged(nameof(MinAmount));
            }
        }
        public int MaxAmount
        {
            get { return _maxAmount; }
            set
            {
                _maxAmount = value;
                OnPropertyChanged(nameof(MaxAmount));
            }
        }
        public int TimeToExpire
        {
            get { return _timeToExpire; }
            set
            {
                _timeToExpire = value;
                OnPropertyChanged(nameof(TimeToExpire));
            }
        }
        public int DeviationPercentage
        {
            get { return _deviationPercentage; }
            set
            {
                _deviationPercentage = value;
                OnPropertyChanged(nameof(DeviationPercentage));
            }
        }
        public int ExpiryTimeForBoxView
        {
            get { return _expiryTimeForBoxView; }
            set
            {
                _expiryTimeForBoxView = value;
                OnPropertyChanged(nameof(ExpiryTimeForBoxView));
            }
        }
        public UserSettingsViewModel()
        {
            UpdateSettings();
            ResetCommand = new RelayCommand(OnReset);
            SaveSettingsCommand = new RelayCommand(OnSaveSettings);
        }
        private void UpdateSettings()
        {
            MinAmount = UserSettings.MinAmount;
            MaxAmount = UserSettings.MaxAmount;
            TimeToExpire = UserSettings.DaysToExpire;
            DeviationPercentage = UserSettings.DeviationPercentage;
            ExpiryTimeForBoxView = UserSettings.ExpiryTimeForBoxView;
        }
        private void OnSaveSettings(object obj)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (_deviationPercentage < 0 ||
                _maxAmount < 0 ||
                _minAmount < 0 ||
                _timeToExpire < 1 ||
                _expiryTimeForBoxView < 0)
            {
                MessageBox.Show("Values are invalid!");
                return;
            }
            UserSettings.SaveSettings(_maxAmount, _minAmount, _timeToExpire, _deviationPercentage, _expiryTimeForBoxView);
            MessageBox.Show("Settings saved!");
        }
        private void OnReset(object obj)
        {
            UserSettings.ResetSettings();
            UpdateSettings();
        }
        public ICommand SaveSettingsCommand { get; }
        public ICommand ResetCommand { get; }
    }
}
