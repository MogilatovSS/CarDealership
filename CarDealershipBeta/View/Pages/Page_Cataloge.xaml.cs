using CarDealershipBeta.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Логика взаимодействия для Page_Cataloge.xaml
    /// </summary>
    public partial class Page_Cataloge : Page
    {
        public Page_Cataloge()
        {
            InitializeComponent();
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListViewCatalogCars.ItemsSource = DataBaseEntities.GetContext().Car.ToList();

                Marka.ItemsSource = DataBaseEntities.GetContext().Car.Select(c => c.Name).Distinct().ToList();
                Carcase.ItemsSource = DataBaseEntities.GetContext().Car.Select(c => c.Carcase).Distinct().ToList();
                Year.ItemsSource = DataBaseEntities.GetContext().Car.Select(c => c.Year).Distinct().ToList();
                Hatch.ItemsSource = DataBaseEntities.GetContext().Car.Select(c => c.Hatch).Distinct().ToList();
                Drive_unit.ItemsSource = DataBaseEntities.GetContext().Car.Select(c => c.Drive_unit).Distinct().ToList();

                Marka.Foreground = Brushes.White;
                Carcase.Foreground = Brushes.White;
                Year.Foreground = Brushes.White;
                Hatch.Foreground = Brushes.White;
                Drive_unit.Foreground = Brushes.White;
            }
        }

        private void MouseDownImgCar(object sender, MouseButtonEventArgs e)
        {
            var selectedCar = (sender as FrameworkElement)?.DataContext as Car;

            if (selectedCar != null)
            {
                MainViewModel.MainFrame.Navigate(new Page_InformationCar(selectedCar));
            }
        }

        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFiltrCars();
        }

        private void ComboBoxFiltr_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFiltrCars();
        }

        private void UpdateFiltrCars()
        {
            var currentCar = DataBaseEntities.GetContext().Car.ToList();

            currentCar = currentCar.Where(c => (Marka.SelectedItem as String == null || c.Name == Marka.SelectedItem as String) &&
            (Carcase.SelectedItem as String == null || c.Carcase == Carcase.SelectedItem as String) &&
            (Year.SelectedItem as String == null || c.Year == Year.SelectedItem as String) &&
            (Hatch.SelectedItem as String == null || c.Hatch == Hatch.SelectedItem as String) &&
            (Drive_unit.SelectedItem as String == null || c.Drive_unit == Drive_unit.SelectedItem as String)).
                Where(c => c.Name.ToLower().Contains(NameFilter.Text.ToLower()) || c.Model.ToLower().Contains(NameFilter.Text.ToLower())).ToList();

            ListViewCatalogCars.ItemsSource = currentCar.ToList();
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Marka.SelectedValue = null;
            Carcase.SelectedValue = null;
            Year.SelectedValue = null;
            Hatch.SelectedValue = null;
            Drive_unit.SelectedValue = null;
            NameFilter.Text = null;
        }

        private void SwitchCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_CatalogeAutopart());
        }

        //    string[] searchTerms = NameFilter.Text.Split(' ');
        //    foreach (string searchTerm in searchTerms)
        //    {
        //        var resultSearch = DataBaseEntities.GetContext().Car.
        //            Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()) ||
        //            c.Model.ToLower().Contains(searchTerm.ToLower())).ToList();

        //        currentCar = currentCar.;
        //    }

        //    ListViewCatalogCars.ItemsSource = currentCar.ToList();

    }
}
