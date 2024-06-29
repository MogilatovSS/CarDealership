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
    /// Логика взаимодействия для Page_Applications.xaml
    /// </summary>
    public partial class Page_Applications : Page
    {
        public Page_Applications()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                YourRoadDataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GridAutopart.ItemsSource = YourRoadDataBaseEntities.GetContext().PhoneService.ToList();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var Applications = GridAutopart.SelectedItems.Cast<PhoneService>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Applications.Count()} элементов ?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    YourRoadDataBaseEntities.GetContext().PhoneService.RemoveRange(Applications);
                    YourRoadDataBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");

                    GridAutopart.ItemsSource = YourRoadDataBaseEntities.GetContext().PhoneService.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }
    }
}
