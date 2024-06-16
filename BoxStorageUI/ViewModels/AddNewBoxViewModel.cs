using BoxStorage.Models.Saver;
using BoxStorageUI.Command;
using System.Windows.Input;
using System.Windows;
using System;

namespace BoxStorageUI.ViewModels
{
    internal class AddNewBoxViewModel : ViewModelBase
    {
        private double _width;

        public string? Width
        {
            get
            {
                return _width == 0 ? null : _width.ToString();
            }
            set
            {
                if (double.TryParse(value, out double val))
                {
                    if (IsLegalWidthHeightVal(val))
                    {
                        _width = val;
                    }
                }
                if (string.IsNullOrEmpty(value)) _width = 0;
                OnPropertyChanged(nameof(Width));
            }
        }
        private double _height;

        public string? Height
        {
            get { return _height == 0 ? null : _height.ToString(); }
            set
            {
                if (double.TryParse(value, out double val))
                {
                    if (IsLegalWidthHeightVal(val))
                    {
                        _height = val;
                    }
                }
                if (string.IsNullOrEmpty(value)) _height = 0;
                OnPropertyChanged(nameof(Height));
            }
        }
        private int _amount;

        public string? Amount
        {
            get { return _amount == 0 ? null : _amount.ToString(); }
            set
            {
                if (int.TryParse(value, out int val))
                {
                    _amount = val;
                }
                if (string.IsNullOrEmpty(value)) _amount = 0;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public AddNewBoxViewModel()
        {
            SaveCommand = new RelayCommand(HandleSave);
            ResetCommand = new RelayCommand(HandleReset);
        }

        private void HandleReset(object obj)
        {
            ResetBox();
        }

        private void HandleSave(object obj)
        {
            if (_width <= 0 && _height <= 0 && _amount <= 0) return;
            try
            {
                Saver.Save(new BoxStorage.Box(_width, _height), _amount);
                MessageBox.Show("Saved Successfuly!");
                ResetBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetBox()
        {
            Width = "0";
            Height = "0";
            Amount = "0";
        }

        private static bool IsLegalWidthHeightVal(double value)
        {
            if (value == Math.Floor(value) || value % 1 == 0.5) return true;
            return false;
        }

        public ICommand SaveCommand { get; }
        public ICommand ResetCommand { get; }
    }
}