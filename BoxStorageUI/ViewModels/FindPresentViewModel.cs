using BoxStorage;
using BoxStorage.Models.Queue;
using BoxStorage.Services;
using BoxStorage.Services.Remover;
using BoxStorageUI.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    public class FindPresentViewModel : ViewModelBase
    {
        
        private int _amount;
        private double _width;
        private double _height;
        private List<Box> _boxList;
        private bool _isListVisible;
        public Visibility VisibilityControl
        {
            get
            {
                return _isListVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }
        public List<Box> BoxesList
        {
            get
            {
                return _boxList;
            }
            set
            {
                _boxList = value;
                _isListVisible = !(BoxesList is null) && BoxesList.Count != 0;
                OnPropertyChanged(nameof(BoxesList));
                OnPropertyChanged(nameof(VisibilityControl));
            }
        }

        public int Amount { get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
        public double Width { get { return _width; }
            set 
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            } 
        }
        public double Height { get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public FindPresentViewModel()
        {
            CheckStockCommand = new RelayCommand(OnCheckStock);
            GetPresentCommand = new RelayCommand(OnGetPresent);
        }

        private void OnGetPresent(object obj)
        {
            try
            {
                GetBox();
            } catch (Exception)
            {
                MessageBoxResult result = MessageBox.Show("There aren't enough boxes\n" +
                    "Would you like to get the closest sizes available?", 
                    "Too Few Boxes", 
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    GetBoxes();
                }
            }
        }

        private void GetBoxes()
        {
            List<Box> temp = Storage.GetBoxesForPresent(Width, Height, Amount);
            if (!IsListAmountEnough(temp))
            {
                int found = GetListAmount(temp);
                int missing = Amount - found;
                MessageBox.Show($"Not Enough Boxes for this order\n" +
                    $"Amount was {found} boxes, missing {missing}.\n" +
                    $"Try to expand search radius in settings!"); 
                return;
            }
            MessageBoxResult result = MessageBox.Show("Enough boxes were found, would you like to take them?",
                    "Success",
                    MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var box in temp)
                {
                    Remover.ForceRemove(box, box.Amount);
                    JsonHandler.Save();
                }
                BoxesList = temp;
                MessageBox.Show("Order Successful!");
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        private bool IsListAmountEnough(List<Box> boxesList)
        {
            int sum = GetListAmount(boxesList);
            return sum >= Amount;
        }

        private int GetListAmount(List<Box> boxesList)
        {
            int sum = 0;
            foreach (Box box in boxesList)
            {
                sum += box.Amount;
            }
            return sum;
        }

        private void GetBox()
        {
            try
            {
                Box box = new Box(Width, Height, Amount);
                Storage.RemoveBox(box, Amount);
                JsonHandler.Save();
                BoxesList = new List<Box>() { box };
                MessageBox.Show("Present is ready");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnCheckStock(object obj)
        {
            CheckStock();
        }

        private void CheckStock()
        {
            try
            {
                BoxQueue boxesInStock = Storage.GetBoxQueue(new Box(Width, Height));
                if (boxesInStock is null) throw new NullReferenceException();
                if (boxesInStock.Count < Amount) MessageBox.Show("Not Enough Boxes for this order");
                BoxesList = boxesInStock.ToList();
            } catch (NullReferenceException)
            {
                MessageBox.Show("No boxes in desired size. try a different size that exists on the 'Show sizes' page");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand GetPresentCommand { get; }
        public ICommand CheckStockCommand { get; }

    }
}
