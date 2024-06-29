using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using MailMessage = System.Net.Mail.MailMessage;
using System.Threading;

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Page_Registration : Page
    {
        private User _currentUser = new User();
        Random random = new Random();
        private int keyForEmail;

        public Page_Registration()
        {
            InitializeComponent();

            DataContext = _currentUser;
        }

        private void LogIn_Button(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_LogIn());
        }

        private async void RegOne_Button(object sender, RoutedEventArgs e)
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
            if (!Regex.IsMatch(Email.Text, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
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

            if (Password.Password.Length < 8)
            {
                Password.ToolTip = "Длина пароля должна превышать 8 символов.";
                Password.Background = Brushes.Red;
            }

            if (errors.Length > 0 || SendingEmail(true) == false)
            {
                return;
            }

            RegistrationBorder.Visibility = Visibility.Hidden;
            ValidationEmailBorder.Visibility = Visibility.Visible;

        }

        private bool SendingEmail(bool valid)
        {
            keyForEmail = random.Next(1000, 10000);

            MailAddress from = new MailAddress("cardealership2022@mail.ru", "CarDealer");
            MailAddress to = new MailAddress($"{Convert.ToString(Email.Text)}");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Подтверждение почты";
            m.Body = $"Ваша ключ для подтверждения почты: {Convert.ToString(keyForEmail)}<br/>Перейдите в приложение и введите" +
                $" ключ в указанном поле. Если это не вы, проигнорируйте данное сообщение." +
                $" <br/><br/>Удачного дня!<br/>С уважением ваш CarDealer.";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("cardealership2022@mail.ru", "dFkbrdv2ap928ER7GF3k");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(m);
                return (true);
            }
            catch
            {
                Email.ToolTip = "Введён несуществующий E-mail";
                Email.Background = Brushes.Red;
                return (false);
            }
        }

        private void RegTwo_Button(object sender, RoutedEventArgs e)
        {
            RegistrationUser();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            RegistrationBorder.Visibility = Visibility.Visible;
            ValidationEmailBorder.Visibility = Visibility.Hidden;
        }

        private void RepitEmail_Button(object sender, RoutedEventArgs e)
        {
            if (SendingEmail(true))
                return;
        }

        private void RegistrationUser()
        {
            txtError.Visibility = Visibility.Hidden;

            if (int.Parse(keyEmail.Text) != keyForEmail)
            {
                txtError.Visibility = Visibility.Visible;
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

        private void keyEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (char c in keyEmail.Text)
            {
                if (!Char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }

            if (keyEmail.Text.Length == 4)
                RegistrationUser();
        }
    }
}
