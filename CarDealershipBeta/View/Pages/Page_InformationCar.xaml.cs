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
    /// Логика взаимодействия для Page_InformationCar.xaml
    /// </summary>
    public partial class Page_InformationCar : Page
    {
        private Liked liked = new Liked();
        public Page_InformationCar(Car selectedCar)
        {
            InitializeComponent();

            DataContext = selectedCar;
            liked.Car_id = selectedCar.Car_id;
            liked.User_id = MainViewModel.currentUser;
        }

        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(MainViewModel.currentUser == 0)
            {
                Bad.Visibility = Visibility.Visible;
                return;
            }

            Good.Visibility = Visibility.Visible;
            if (liked.Liked_id == 0)
                DataBaseEntities.GetContext().Liked.Add(liked);

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
