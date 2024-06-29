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
    /// Логика взаимодействия для Page_Warehouse.xaml
    /// </summary>
    public partial class Page_Warehouse : Page
    {
        public Page_Warehouse()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_EditCar((sender as Button).DataContext as Car));
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_EditCar(null));
        }
        
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GridCar.ItemsSource = DataBaseEntities.GetContext().Car.ToList();

                if(MainViewModel.typeUser == "admin")
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var CarsForRemoving = GridCar.SelectedItems.Cast<Car>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {CarsForRemoving.Count()} элементов ?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBaseEntities.GetContext().Car.RemoveRange(CarsForRemoving);
                    DataBaseEntities.GetContext().SaveChanges();

                    MessageBox.Show("Данные удалены");

                    GridCar.ItemsSource = DataBaseEntities.GetContext().Car.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnSwitch_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_WarehouseAutopart());
        }
    }
}
