using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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
                DataBaseEntities.GetContext().PhoneService.Add(phoneService);
            }
            else
            {
                DataBaseEntities.GetContext().PhoneService.Add(phoneService);
            }

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            Good.Visibility = Visibility.Visible;
        }
        private void Marka_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
        private void Model_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void Engine_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void Transmission_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void Privod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
        private void TypeTO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
    }
}
