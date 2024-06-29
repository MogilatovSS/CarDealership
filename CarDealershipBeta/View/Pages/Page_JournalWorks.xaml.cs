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
    /// Логика взаимодействия для Page_JournalWorks.xaml
    /// </summary>
    public partial class Page_JournalWorks : Page
    {
        private ApplicationOrderService applicationOrderService = new ApplicationOrderService();

        public Page_JournalWorks()
        {
            InitializeComponent();

            GridJournalWorks.ItemsSource = YourRoadDataBaseEntities.GetContext().ApplicationOrderService.Include("OrderService.WarehouseAutopart")
                .Include("OrderService.Service").Include("OrderService.Car").ToList();
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var journalForRemoving = GridJournalWorks.SelectedItems.Cast<ApplicationOrderService>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {journalForRemoving.Count()} элементов ?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    YourRoadDataBaseEntities.GetContext().ApplicationOrderService.RemoveRange(journalForRemoving);
                    YourRoadDataBaseEntities.GetContext().SaveChanges();

                    MessageBox.Show("Данные удалены");

                    GridJournalWorks.ItemsSource = YourRoadDataBaseEntities.GetContext().ApplicationOrderService.Include("OrderService.WarehouseAutopart")
                        .Include("OrderService.Service").Include("OrderService.Car").ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentApplication = (sender as Button).DataContext as ApplicationOrderService;
            applicationOrderService = currentApplication;

            if(currentApplication.EndDate != null)
                EndDateTxt.Text = currentApplication.EndDate.ToString();

            if (currentApplication.Status != null)
                StatusTxt.Text = currentApplication.Status.ToString();

            if (currentApplication.performer != null)
                PerformerTxt.Text = currentApplication.performer.ToString();

            WindowEdit.Visibility = Visibility.Visible;
        }

        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            string endDateStr = EndDateTxt.Text;
            DateTime endDate;

            if (DateTime.TryParse(endDateStr, out endDate))
            {
                applicationOrderService.EndDate = endDate;
            }

            applicationOrderService.Status = StatusTxt.Text;
            applicationOrderService.performer = PerformerTxt.Text;

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
                GridJournalWorks.ItemsSource = YourRoadDataBaseEntities.GetContext().ApplicationOrderService.Include("OrderService.WarehouseAutopart")
                .Include("OrderService.Service").Include("OrderService.Car").ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void Close_ClickA(object sender, RoutedEventArgs e)
        {
            WindowEdit.Visibility = Visibility.Hidden;
        }
    }
}
