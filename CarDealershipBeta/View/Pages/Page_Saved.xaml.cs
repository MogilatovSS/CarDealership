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
    /// Логика взаимодействия для Page_Saved.xaml
    /// </summary>
    public partial class Page_Saved : Page
    {
        private static Liked liked = new Liked();
        public Page_Saved()
        {
            InitializeComponent();
        }

        private void RemoveCar_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = (sender as Button).DataContext as Liked;

            if (selectedCar != null)
            {
                DataBaseEntities.GetContext().Liked.Remove(selectedCar);

                try
                {
                    DataBaseEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Reload();
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Reload();
        }
        private void Reload()
        {
            var car = DataBaseEntities.GetContext().Liked.Where(l => l.User_id == MainViewModel.currentUser).ToList();
            int sumMoney = 0;

            foreach(var currentCar in car)
            {
                sumMoney += currentCar.Car.Price;
            }

            Money_TextBlock.Text = $"Сумма: {sumMoney} $";
            ListViewCatalogCars.ItemsSource = car;

            if (car.Count > 0)
                Text_Basket.Visibility = Visibility.Hidden;
            else
                Text_Basket.Visibility = Visibility.Visible;
        }

        private void MouseDownImgCar(object sender, MouseButtonEventArgs e)
        {
            var selectedCar = (sender as FrameworkElement)?.DataContext as Liked;
            var selectedCarExport = DataBaseEntities.GetContext().Car.SingleOrDefault(c => c.Car_id == selectedCar.Car_id);

            if (selectedCar != null)
            {
                MainViewModel.MainFrame.Navigate(new Page_InformationCar(selectedCarExport));
            }
        }
    }
}
