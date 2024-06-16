using BoxStorage;
using BoxStorage.Models;
using BoxStorage.Models.Queue;
using BoxStorage.Models.Saver;
using BoxStorage.Services;
using BoxStorage.Services.Remover;
using BoxStorageUI.Command;
using BoxStorageUI.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    internal class BoxEditViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private BoxViewModel _boxViewModel;
        public BoxViewModel BoxVM
        {
            get { return _boxViewModel; }
            set {
                _boxViewModel = value;
                OnPropertyChanged(nameof(BoxVM));
            }
        }
        private Box _item;
        public Box Item {
            get { return _item; }
            set
            {
                _item = value;
                _boxViewModel = new BoxViewModel(null, Storage.GetBoxSpot(Item));
                OnPropertyChanged(nameof(BoxVM));
                OnPropertyChanged(nameof(Item));
            }
        }
        public double Width
        {
            get { return Item.Width; }
            set { Item.Width = value; }
        }
        public double Height
        {
            get { return Item.Height; }
            set { Item.Height = value; }
        }
        public int Amount
        {
            get { return int.Parse(BoxVM.Amount); }
            set
            {
                Item.Amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        private int _amountToAdd;
        public string AmountToAdd
        {
            get { return _amountToAdd.ToString(); }
            set
            {
                if (int.TryParse(value, out int amountToAdd)) _amountToAdd = amountToAdd;
                else _amountToAdd = 0;
                OnPropertyChanged(nameof(AmountToAdd));
            }
        }

        private int _amountToRemove;
        public string AmountToRemove
        {
            get { return _amountToRemove.ToString(); }
            set
            {
                if (int.TryParse(value, out int amountToRem)) _amountToRemove = amountToRem;
                else _amountToRemove = 0;
                OnPropertyChanged(nameof(AmountToRemove));
            }
        }
        private List<Box> _stockList;
        public List<Box> BoxesList
        {
            get{return _stockList;}
            private set
            {
                _stockList = value;
                OnPropertyChanged(nameof(BoxesList));
            }
        }
        private Visibility _cardVisibility;
        private Visibility _queueVisibility;
        public Visibility CardVisibility
        {
            get{return _cardVisibility;}
            set
            {
                _cardVisibility = value;
                OnPropertyChanged(nameof(CardVisibility));
                OnPropertyChanged(nameof(BoxesList));
            }
        }
        public Visibility QueueVisibility
        {
            get{return _queueVisibility;}
            set
            {
                _queueVisibility = value;
                OnPropertyChanged(nameof(QueueVisibility));
                OnPropertyChanged(nameof(BoxPreviewButtonVisibility));
                OnPropertyChanged(nameof(BoxesList));
            }
        }

        public Visibility BoxPreviewButtonVisibility { get { return _queueVisibility; } }

        public BoxEditViewModel(NavigationStore navigationStore ,BoxViewModel boxViewModel)
        {
            
            _navigationStore = navigationStore;
            _boxViewModel = boxViewModel;
            Box b = new Box(
                double.Parse(boxViewModel.Width),
                double.Parse(boxViewModel.Height),
                int.Parse(boxViewModel.Amount),
                DateTime.Parse(boxViewModel.AdditionDate.ToString()));
            _item = b;
            CancelCommand = new NavigateCommand(navigationStore, () => new BoxListViewModel(navigationStore));
            RestockCommand = new RelayCommand(HandleRestockCommand);
            CheckStockCommand = new RelayCommand(HandleCheckStock);
            BoxPreviewCommand = new RelayCommand(HandleBoxPreview);
            CardVisibility = Visibility.Visible;
            QueueVisibility = Visibility.Hidden;
        }

        private void HandleBoxPreview(object obj)
        {
            CardVisibility = Visibility.Visible;
            QueueVisibility = Visibility.Hidden;
        }

        private void HandleCheckStock(object obj)
        {
            try
            {
                if (Storage.Contains(this.Item))
                {
                    UpdateList();
                    if (BoxesList.Count > 0)
                    {
                        QueueVisibility = Visibility.Visible;
                        CardVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        MessageBox.Show("No boxes in stock!");
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HandleRestockCommand(object obj)
        {
            try
            {
                if ((AmountToAdd == "0" && AmountToRemove == "0")) return;
                if ((Amount + _amountToAdd > UserSettings.MaxAmount && _amountToAdd != 0)
                    || (Amount - _amountToRemove < UserSettings.MinAmount && _amountToRemove != 0))
                {
                    throw new Exception("Amount out of range!");
                }
                 if (_amountToRemove > 0 )Remover.Remove(this.Item, _amountToRemove);
                if (_amountToAdd > 0) Saver.Save(this.Item, _amountToAdd);
                MessageBox.Show("Restock Successful!");
                ResetAmount();
                UpdateItem();
                UpdateList();
            }
            catch (BoxExpiredException ex)
            {
                MessageBox.Show(ex.Message);
                JsonHandler.Save();
                _amountToAdd = 0;
                _amountToRemove = 0;
                UpdateItem();
                UpdateList();
                ResetAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _amountToAdd = 0;
                _amountToRemove = 0;
                ResetAmount();
            }
        }

        private void UpdateList()
        {
            BoxQueue queue = Storage.GetBoxQueue(this.Item);
            if (queue.Count > 0)
            {
                BoxesList = queue.ToList();
            }
        }

        private void ResetAmount()
        {
            Amount += _amountToAdd;
            Amount -= _amountToRemove;
            AmountToAdd = "0";
            AmountToRemove = "0";
        }
        private void UpdateItem()
        {
            Item = new Box(Width, Height, Amount);
        }

        public ICommand CancelCommand { get; }
        public ICommand RestockCommand { get; }
        public ICommand CheckStockCommand { get; }
        public ICommand BoxPreviewCommand { get; }
        
    }
}
