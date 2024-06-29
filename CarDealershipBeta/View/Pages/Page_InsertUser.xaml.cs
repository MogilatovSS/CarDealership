using CarDealershipBeta.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;
using Button = System.Windows.Controls.Button;

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page_InsertUser.xaml
    /// </summary>
    public partial class Page_InsertUser : Page
    {
        private User currentUser = new User();
        public Page_InsertUser()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Reload();
            }
        }

        private void Reload()
        {
            if (Visibility == Visibility.Visible)
            {
                YourRoadDataBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GridUser.ItemsSource = YourRoadDataBaseEntities.GetContext().User.ToList();
                UserType.ItemsSource = YourRoadDataBaseEntities.GetContext().User.Select(c => c.UserType).Distinct().ToList();
                DataContext = currentUser;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataContext = (sender as Button).DataContext as User;
            currentUser = DataContext as User;
        }

            private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            currentUser = DataContext as User;



            if (string.IsNullOrWhiteSpace(textLogin.Text))
            {
                errors.AppendLine("Введите логин");
            }
            
            if (string.IsNullOrWhiteSpace(textPassword.Text))
                errors.AppendLine("Введите пароль");
            
            if (!Regex.IsMatch(textEmail.Text, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                errors.AppendLine("Неккоректный E-mai");
            }

            var users = YourRoadDataBaseEntities.GetContext().User.ToList();
            foreach (var user in users)
            {
                if (user.Login == textLogin.Text)
                {
                    errors.AppendLine("Пользователь с таким логином уже существует");
                }
            }

            if (errors.Length > 0 )
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentUser.Login = textLogin.Text;
            currentUser.Password = textPassword.Text;
            currentUser.Email = textEmail.Text;
            currentUser.UserType = UserType.Text;
            currentUser.Password = GetHash(textPassword.Text);

            if (currentUser.User_id == 0)
                YourRoadDataBaseEntities.GetContext().User.AddOrUpdate(currentUser);

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
        public static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentUser = DataContext as User;

            currentUser = YourRoadDataBaseEntities.GetContext().User.FirstOrDefault(u => u.Login == currentUser.Login || u.User_id == currentUser.User_id );
            DataContext = currentUser;
        }

        private void UserInformation_Change(object sender, TextChangedEventArgs e)
        {
            currentUser = DataContext as User;
            foreach (char c in textId.Text)
            {
                if (!Char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void UserInformation_Change(object sender, SelectionChangedEventArgs e)
        {
            currentUser = DataContext as User;
        }

        private void MouseDownImgCar(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.png; *.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                currentUser.Image = openFileDialog.FileName;

            }
            Reload();
        }

        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }
    }
}
