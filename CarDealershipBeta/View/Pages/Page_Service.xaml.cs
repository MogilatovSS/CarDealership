using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Page_Service.xaml
    /// </summary>
    public partial class Page_Service : Page
    {
        private static PhoneService phoneService = new PhoneService();
        public Page_Service()
        {
            InitializeComponent();
            Marka.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Name).Distinct().ToList();
            Model.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Model).Distinct().ToList();
            Engine.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Model_Engine).Distinct().ToList();

        }

        private void Reg_Button(object sender, RoutedEventArgs e)
        {
            Number.ToolTip = "";
            Number.Background = Brushes.Transparent;
            phoneService.Name = Name.Text;
            phoneService.Number = Number.Text;
            Good.Visibility = Visibility.Hidden;

            if (Number.Text.Length > 12 || Number.Text.Length > 0 && !Regex.IsMatch(Number.Text,
                @"(\+7|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)"))
            {
                Number.ToolTip = "Некорректно введён номер";
                Number.Background = Brushes.Red;
                return;
            }
            if (MainViewModel.currentUser != 0)
            {
                phoneService.User_id_Service = MainViewModel.currentUser;
                YourRoadDataBaseEntities.GetContext().PhoneService.Add(phoneService);
            }
            else
            {
                YourRoadDataBaseEntities.GetContext().PhoneService.Add(phoneService);
            }

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            Good.Visibility = Visibility.Visible;
        }

        private void Engine_SelectionChangeCommitted(object sender, SelectionChangedEventArgs e)
        {
            UpdateCarId();
        }

        private void Model_SelectionChangeCommitted(object sender, SelectionChangedEventArgs e)
        {
            UpdateCarId();
            if (Model.SelectedItem != null)
            {
                string selectedMarka = Marka.SelectedItem.ToString();
                string selectedModel = Model.SelectedItem.ToString();

                Engine.ItemsSource = YourRoadDataBaseEntities.GetContext().Car
                    .Where(c => c.Name == selectedMarka && c.Model == selectedModel)
                    .Select(c => c.Model_Engine)
                    .Distinct()
                    .ToList();
            }
        }

        private void Marka_SelectionChangeCommitted(object sender, SelectionChangedEventArgs e)
        {
            UpdateCarId();
            if (Marka.SelectedItem != null)
            {
                string selectedMarka = Marka.SelectedItem.ToString();

                Model.ItemsSource = YourRoadDataBaseEntities.GetContext().Car
                    .Where(c => c.Name == selectedMarka)
                    .Select(c => c.Model)
                    .Distinct()
                    .ToList();
            }

        }
        private void UpdateCarId()
        {
            var selectedMarka = Marka.SelectedItem as string;
            var selectedModel = Model.SelectedItem as string;
            var selectedEngine = Engine.SelectedItem as string;

            if (selectedMarka != null && selectedModel != null && selectedEngine != null)
            {
                var carId = YourRoadDataBaseEntities.GetContext().Car.FirstOrDefault(c => c.Name == selectedMarka && c.Model == selectedModel && c.Model_Engine == selectedEngine)?.Car_id;

                if (carId != null)
                {
                    var categoryServiceData = YourRoadDataBaseEntities.GetContext().CategoryService.Where(c => c.Car_id == carId).ToList();
                    GridService.ItemsSource = categoryServiceData;
                }
            }
        }
    }
}
