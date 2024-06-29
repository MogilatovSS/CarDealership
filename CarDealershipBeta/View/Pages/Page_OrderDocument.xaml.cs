using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для Page_OrderDocument.xaml
    /// </summary>
    public partial class Page_OrderDocument : Page
    {
        private OrderService orderService = new OrderService();
        private Service service = new Service();
        private WarehouseAutopart autopart = new WarehouseAutopart();
        private User user = new User();
        private List<Service> serviceList = new List<Service>();
        private List<WarehouseAutopart> autopartList = new List<WarehouseAutopart>(); 
        public Page_OrderDocument()
        {
            InitializeComponent();
            Marka.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Name).Distinct().ToList();
            Model.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Model).Distinct().ToList();
            Engine.ItemsSource = YourRoadDataBaseEntities.GetContext().Car.Select(c => c.Model_Engine).Distinct().ToList();
            Service.ItemsSource = YourRoadDataBaseEntities.GetContext().Service.Select(c => c.Type_service).Distinct().ToList();
            Autopart.ItemsSource = YourRoadDataBaseEntities.GetContext().WarehouseAutopart.Select(c => c.Name_item).Distinct().ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var orderForRemoving = GridOrderDocument.SelectedItems.Cast<OrderService>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {orderForRemoving.Count()} элементов ?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    YourRoadDataBaseEntities.GetContext().OrderService.RemoveRange(orderForRemoving);
                    YourRoadDataBaseEntities.GetContext().SaveChanges();

                    MessageBox.Show("Данные удалены");

                    GridOrderDocument.ItemsSource = YourRoadDataBaseEntities.GetContext().OrderService.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            WindowAddOrder.Visibility = Visibility.Visible;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                YourRoadDataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GridOrderDocument.ItemsSource = YourRoadDataBaseEntities.GetContext().OrderService.ToList();

                /*if (MainViewModel.typeUser == "admin")
                {
                    BtnSwitch.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnSwitch.Visibility = Visibility.Hidden;
                    BtnDelete.Visibility = Visibility.Hidden;
                }*/
            }
        }

        private void Close_ClickA(object sender, RoutedEventArgs e)
        {
            WindowAddOrder.Visibility = Visibility.Hidden;
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
                orderService.Car_id = (int)carId;
            }
        }

        private void BtnInsertService_Click(object sender, RoutedEventArgs e)
        {
            serviceList.Add(service);
            ServiceItemsControl.ItemsSource = null;
            ServiceItemsControl.ItemsSource = serviceList;
            ServiceItemsControl.UpdateLayout();
        }

        private void BtnInsertAutopart_Click(object sender, RoutedEventArgs e)
        {
            autopartList.Add(autopart);
            AutopartItemsControl.ItemsSource = null;
            AutopartItemsControl.ItemsSource = autopartList;
            ServiceItemsControl.UpdateLayout();
        }

        private void Autopart_SelectionChangeCommitted(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string selectedAutopartName = comboBox.SelectedItem as string;
                autopart = YourRoadDataBaseEntities.GetContext().WarehouseAutopart.FirstOrDefault(a => a.Name_item == selectedAutopartName);
            }
        }


        private void Service_SelectionChangeCommitted(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string selectedServiceType = comboBox.SelectedItem as string;
                service = YourRoadDataBaseEntities.GetContext().Service.FirstOrDefault(a => a.Type_service == selectedServiceType);
            }
        }

        private void BtnInsertUser_Click(object sender, RoutedEventArgs e)
        {
            if(loginUser.Text != "")
            {
                user = YourRoadDataBaseEntities.GetContext().User.FirstOrDefault(a => a.Login == loginUser.Text);
                if(user == null) 
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if(orderService.Car_id == 0)
            {
                MessageBox.Show("Автомобиль не выбран");
                return;
            }

            if (user != null)
            {
                orderService.User_id = user.User_id;
            }

            orderService.Date = DateTime.Now;

            if(serviceList.Count == 0) 
            {
                MessageBox.Show("Услуга не выбрана");
                return;
            }

            foreach(var service in serviceList)
            {
                orderService.Service.Add(service);
            }

            foreach (var autopart in autopartList)
            {
                orderService.WarehouseAutopart.Add(autopart);
            }

            YourRoadDataBaseEntities.GetContext().OrderService.AddOrUpdate(orderService);

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
                ApplicationOrderService applicationOrderService = new ApplicationOrderService();
                applicationOrderService.OrderService_id = orderService.OrderService_id;
                applicationOrderService.Status = "Зарегистрирован";
                YourRoadDataBaseEntities.GetContext().ApplicationOrderService.Add(applicationOrderService);
                YourRoadDataBaseEntities.GetContext().SaveChanges();
                orderService = new OrderService();
                autopartList.Clear();
                serviceList.Clear();
                AutopartItemsControl.ItemsSource = null;
                ServiceItemsControl.ItemsSource = null;
                Marka.SelectedItem = null;
                Engine.SelectedItem = null;
                Model.SelectedItem = null;
                Autopart.SelectedItem = null;
                Service.SelectedItem = null;
                loginUser.Text = null;
                GridOrderDocument.ItemsSource = YourRoadDataBaseEntities.GetContext().OrderService.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException}");
            }
        }

        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }
    }
}
