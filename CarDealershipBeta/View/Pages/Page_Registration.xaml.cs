using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mail;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Page_Registration : Page
    {
        private User _currentUser = new User();
        public Page_Registration()
        {
            InitializeComponent();

            DataContext = _currentUser;
        }

        private void LogIn_Button(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_LogIn());
        }

        private void Reg_Button(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            Login.ToolTip = null;
            Login.Background = Brushes.Transparent;
            Password.ToolTip = null;
            Password.Background = Brushes.Transparent;
            TwoPassword.ToolTip = null;
            TwoPassword.Background = Brushes.Transparent;
            Email.ToolTip = null;
            Email.Background = Brushes.Transparent;

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
            {
                Login.ToolTip = "Введите логин.";
                Login.Background = Brushes.Red;
                errors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(Password.Password))
                errors.AppendLine("Введите пароль");
            if (Password.Password != TwoPassword.Password)
            {
                errors.AppendLine("Пароли не совпадают");
                TwoPassword.ToolTip = "Пароли должны совпадать.";
                TwoPassword.Background = Brushes.Red;
            }
            if (!Regex.IsMatch(Email.Text, @"[a-zA - Z0 - 9]+@[a-zA - Z0 - 9]+\.[a-zA - Z0 - 9]+"))
            {
                Email.ToolTip = "Некорректный E-mail";
                Email.Background = Brushes.Red;
                errors.AppendLine("Неккоректный E-mai");
            }

            var users = DataBaseEntities.GetContext().User.ToList();
            foreach (var user in users)
            {
                if (user.Login == _currentUser.Login)
                {
                    errors.AppendLine("Пользователь с таким логином уже существует");
                    Login.ToolTip = "Данный логин уже занят.";
                    Login.Background = Brushes.Red;
                }
            }

            if(Password.Password.Length < 8)
            {
                Password.ToolTip = "Длина пароля должна превышать 8 символов.";
                Password.Background = Brushes.Red;
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentUser.Password = GetHash(Password.Password);
            if (_currentUser.User_id == 0)
                DataBaseEntities.GetContext().User.Add(_currentUser);

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
                MainViewModel.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public static string GetHash(string input) 
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
