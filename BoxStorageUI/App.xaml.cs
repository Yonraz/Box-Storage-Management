using BoxStorage;
using BoxStorage.Services;
using BoxStorageUI.Stores;
using BoxStorageUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BoxStorageUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        public App()
        {
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new HomeViewModel();
            GetStartUpItems();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private void GetStartUpItems()
        {
            try
            {
                if (File.Exists(JsonHandler.JsonFilePath))
                {
                    Storage.LoadFromJson();
                    return;
                }
                else
                {
                    Storage.Restock(new Box(50, 180), 804);
                    Storage.Restock(new Box(50, 170), 1230);
                    Storage.Restock(new Box(55, 70), 700);
                    Storage.Restock(new Box(55, 60), 1000);
                    Storage.Restock(new Box(50, 170), 550);
                    Storage.Restock(new Box(50, 160), 1000);
                    Storage.Restock(new Box(55, 70), 1390);
                    Storage.Restock(new Box(55, 60), 1000);
                    Storage.Restock(new Box(55, 50), 600);
                    Storage.Restock(new Box(48, 125), 1000);
                    Storage.Restock(new Box(48, 145), 170);
                    Storage.Restock(new Box(42, 145), 1550);
                    Storage.Restock(new Box(500, 145), 1705);
                    Storage.Restock(new Box(49, 145), 1210);
                    Storage.Restock(new Box(48, 145), 170);
                    Storage.Restock(new Box(42, 145), 1550);
                    Storage.Restock(new Box(500, 145), 1705);
                    JsonHandler.Save();
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
