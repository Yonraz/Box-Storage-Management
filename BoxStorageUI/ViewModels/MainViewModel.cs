using BoxStorageUI.Command;
using BoxStorageUI.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            ShowItemsCommand = new NavigateCommand(_navigationStore, () => new BoxListViewModel(navigationStore));
            ShowHomeScreenCommand = new NavigateCommand(_navigationStore, () => new HomeViewModel());
            GoToAddCommand = new NavigateCommand(_navigationStore, () => new AddNewBoxViewModel());
            GoToSettingsCommand = new NavigateCommand(_navigationStore, () => new UserSettingsViewModel());
            GoToFindItemCommand = new NavigateCommand(_navigationStore, () => new FindPresentViewModel());
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ICommand ShowItemsCommand { get; }
        public ICommand ShowHomeScreenCommand { get; }
        public ICommand GoToAddCommand { get; }
        public ICommand GoToSettingsCommand { get; }
        public ICommand GoToFindItemCommand { get; }
    }
}
