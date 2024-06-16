using BoxStorage;
using BoxStorage.Models;
using BoxStorage.Models.Queue;
using BoxStorageUI.Command;
using BoxStorageUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    class BoxListViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;
        ObservableCollection<BoxViewModel> _boxes;
        private enum _viewOptions
        {
            All,
            AboutToExpire
        }
        public string[] ViewOptions { get; }
        private string _selectedViewOption;
        public string SelectedViewOption
        {
            get => _selectedViewOption;
            set
            {
                _selectedViewOption = value;
                OnPropertyChanged(nameof(SelectedViewOption));
                OnPropertyChanged(nameof(CurrentBoxView));
            }
        }
        public ObservableCollection<BoxViewModel> Boxes
        {
            get => _boxes;
            set
            {
                _boxes = value;
                OnPropertyChanged(nameof(Boxes));
            }
        }
        public ObservableCollection<BoxViewModel> CurrentBoxView
        {
            get
            {
                switch (SelectedViewOption)
                {
                    case "All":
                        return Boxes;
                    case "AboutToExpire":
                        return new ObservableCollection<BoxViewModel>(Boxes.Where(x => IsAboutToExpire(x)));
                    default:
                        return Boxes;
                }
            }
        }
        public BoxListViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _boxes = GetBoxes();
            ViewOptions = Enum.GetNames(typeof(_viewOptions));
        }
        private static bool IsAboutToExpire(BoxViewModel bvm)
        {
            DateTime? expiredBy = DateTime.Parse(bvm.ExipryDate);
            return expiredBy < DateTime.Now.AddDays(UserSettings.ExpiryTimeForBoxView);
        }
        private ObservableCollection<BoxViewModel> GetBoxes()
        {
            IEnumerable<BoxSpotNode> boxSpotNodes = Storage.GetBoxSpots();
            IEnumerable<BoxViewModel> boxViewModels = boxSpotNodes.Select(boxSpotNode => 
                                                      new BoxViewModel(_navigationStore, boxSpotNode));
            return new ObservableCollection<BoxViewModel>(boxViewModels);
        }
    }
}
