using BoxStorage;
using BoxStorage.Models.Queue;
using BoxStorageUI.Command;
using BoxStorageUI.Stores;
using System;
using System.Windows.Input;

namespace BoxStorageUI.ViewModels
{
    public class BoxViewModel : ViewModelBase
    {
        readonly BoxQueue? _boxQueue;
        private NavigationStore _navigationStore;
        private BoxSpotNode _boxSpotNode;
        public string BoxName => $"{Width}X{Width}X{Height}";
        public string Width => _boxSpotNode.Width.ToString();
        public string Height => _boxSpotNode.Height.ToString();
        public string Amount => _boxSpotNode.Amount.ToString();
        public string ExipryDate { 
            get
            { 
                if (_boxQueue == null || _boxQueue.Count == 0)
                    return "No boxes in queue";
                return _boxQueue.Peek().ExpiryDate.ToString("dd/MM/yy"); 
            }
        }
        public DateTime? AdditionDate
        {
            get
            {
                return _boxSpotNode.LastAccessed;
            }
        }
        public BoxViewModel(NavigationStore navigationStore, BoxSpotNode boxSpotNode)
        {
            _navigationStore = navigationStore;
            this._boxSpotNode = boxSpotNode;
            this._boxQueue = boxSpotNode.Boxes;
            MoveToEditCommand = new NavigateCommand(_navigationStore, () => new BoxEditViewModel(navigationStore, this));
        }

        public ICommand MoveToEditCommand { get; }
    }
}