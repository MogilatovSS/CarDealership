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
            _user = DataBaseEntities.GetContext().User.SingleOrDefault(u => u.User_id == MainViewModel.currentUser);
            if (_user != null)
                GridProfile.Visibility = Visibility.Visible;
        }

        private void MouseDownImgCar(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.png; *.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                _user.Image = openFileDialog.FileName;

                DataBaseEntities.GetContext().User.AddOrUpdate(_user);

                try
                {
                    DataBaseEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            Reload();
        }

        private void Reload()
        {
            var user = DataBaseEntities.GetContext().User.FirstOrDefault(u => u.User_id == MainViewModel.currentUser);
            var autoparts = DataBaseEntities.GetContext().BasketAutopart.Where(a => a.User_id == MainViewModel.currentUser && a.Sold == true).ToList();

            DataContext = user;
            ListViewCatalogCars.ItemsSource = autoparts;
            if (autoparts.Count > 0)
            {
                txtHistoryOne.Visibility = Visibility.Visible;
                txtHistoryTwo.Visibility = Visibility.Hidden;
            }
            else
            {
                txtHistoryOne.Visibility = Visibility.Visible;
                txtHistoryTwo.Visibility = Visibility.Hidden;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Reload();
            BtnEmailSave.Visibility = Visibility.Hidden;
            BtnLogInSave.Visibility = Visibility.Hidden;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedAutopart = (sender as Button).DataContext as BasketAutopart;

            if (selectedAutopart != null)
            {
                DataBaseEntities.GetContext().BasketAutopart.Remove(selectedAutopart);

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
            DataBaseEntities.GetContext().User.AddOrUpdate(_user);

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
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
            DataBaseEntities.GetContext().User.AddOrUpdate(_user);

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            BtnEmailSave.Visibility = Visibility.Hidden;

            Reload();
        }
    }
}
