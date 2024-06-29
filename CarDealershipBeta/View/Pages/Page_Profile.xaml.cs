using CarDealershipBeta.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
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
    /// Логика взаимодействия для Page_Profile.xaml
    /// </summary>
    public partial class Page_Profile : Page
    {
        User _user = new User();
        public Page_Profile()
        {
            InitializeComponent();
        }

        private void MouseDownImgCar(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.png; *.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                _user.Image = openFileDialog.FileName;

                try
                {
                    YourRoadDataBaseEntities.GetContext().SaveChanges();
                    _user = YourRoadDataBaseEntities.GetContext().User.SingleOrDefault(u => u.User_id == MainViewModel.currentUser); 
                    DataContext = _user; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }


        private void Reload()
        {

            var user = YourRoadDataBaseEntities.GetContext().User.FirstOrDefault(u => u.User_id == MainViewModel.currentUser);
            var autoparts = YourRoadDataBaseEntities.GetContext().BasketAutopart.Where(a => a.User_id == MainViewModel.currentUser && a.Sold == true).ToList();

            BtnEmailSave.Visibility = Visibility.Hidden;
            BtnLogInSave.Visibility = Visibility.Hidden;
            InsertUser.Visibility = Visibility.Hidden;
            JournalWorks.Visibility = Visibility.Hidden;
            OrderDocument.Visibility = Visibility.Hidden;
            ViewUserApplications.Visibility = Visibility.Hidden;

            if (user == null || user.User_id == 0)
                return;

            GridProfile.Visibility = Visibility.Visible;

            DataContext = user;
            ListViewCatalogCars.ItemsSource = autoparts;

            if (autoparts.Count > 0)
            {
                txtHistoryOne.Visibility = Visibility.Visible;
                txtHistoryTwo.Visibility = Visibility.Hidden;
            }
            else
            {
                txtHistoryOne.Visibility = Visibility.Hidden;
                txtHistoryTwo.Visibility = Visibility.Visible;
            }

            if (MainViewModel.typeUser == "admin")
            {
                InsertUser.Visibility = Visibility.Visible;
                ViewUserApplications.Visibility = Visibility.Visible;
                JournalWorks.Visibility= Visibility.Visible;
                OrderDocument.Visibility= Visibility.Visible;
                return;
            }

            if (MainViewModel.typeUser == "call")
            {
                ViewUserApplications.Visibility = Visibility.Visible;
                return;
            }

            if (MainViewModel.typeUser == "service")
            {
                JournalWorks.Visibility = Visibility.Visible;
                return;
            }

            if (MainViewModel.typeUser == "master")
            {
                ViewUserApplications.Visibility = Visibility.Visible;
                JournalWorks.Visibility = Visibility.Visible;
                OrderDocument.Visibility = Visibility.Visible;
                return;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Reload();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedAutopart = (sender as Button).DataContext as BasketAutopart;

            if (selectedAutopart != null)
            {
                YourRoadDataBaseEntities.GetContext().BasketAutopart.Remove(selectedAutopart);

                try
                {
                    YourRoadDataBaseEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Reload();
            }
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnEmailSave.Visibility = Visibility.Visible;
        }

        private void LogIn_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLogInSave.Visibility = Visibility.Visible;
        }

        private void BtnSaveLogin_Click(object sender, RoutedEventArgs e)
        {
            _user.Login = textLogin.Text;
            YourRoadDataBaseEntities.GetContext().User.AddOrUpdate(_user);

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            BtnLogInSave.Visibility = Visibility.Hidden;

            Reload();

        }
        private void BtnSaveEmail_Click(object sender, RoutedEventArgs e)
        {
            _user.Email = textEmail.Text;
            YourRoadDataBaseEntities.GetContext().User.AddOrUpdate(_user);

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            BtnEmailSave.Visibility = Visibility.Hidden;

            Reload();
        }

        private void InsertUser_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_InsertUser());
        }

        private void ViewUserApplications_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_Applications());
        }

        private void OrderDocument_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_OrderDocument());
        }

        private void JournalWorks_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_JournalWorks());
        }
    }
}
