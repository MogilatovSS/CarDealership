using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page_WarehouseAutopart.xaml
    /// </summary>
    public partial class Page_WarehouseAutopart : Page
    {
        public Page_WarehouseAutopart()
        {
            InitializeComponent();
        }

        private void BtnSwitch_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_Warehouse());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var CarsForRemoving = GridAutopart.SelectedItems.Cast<WarehouseAutopart>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {CarsForRemoving.Count()} элементов ?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBaseEntities.GetContext().WarehouseAutopart.RemoveRange(CarsForRemoving);
                    DataBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");

                    GridAutopart.ItemsSource = DataBaseEntities.GetContext().WarehouseAutopart.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_EgitAutopart((sender as Button).DataContext as WarehouseAutopart));
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_EgitAutopart(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GridAutopart.ItemsSource = DataBaseEntities.GetContext().WarehouseAutopart.ToList();

                if (MainViewModel.typeUser == "admin")
                {
                    BtnSwitch.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnSwitch.Visibility = Visibility.Hidden;
                    BtnDelete.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
