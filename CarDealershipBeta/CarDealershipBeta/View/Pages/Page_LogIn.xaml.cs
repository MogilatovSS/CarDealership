using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class Page_LogIn : Page
    {
        private User _currentUser = new User();
        public Page_LogIn()
        {
            InitializeComponent();

            DataContext = _currentUser;
        }

        private void Registration_Button(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_Registration());
        }

        private void LogIn_Button(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            string stlogin = Login.Text.Trim();
            string stPass = Password.Password.Trim();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
            {
                Login.ToolTip = "Введите логин";
                Login.Background = Brushes.Red;
                errors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(Password.Password))
            {
                Login.ToolTip = "Введите пароль";
                Login.Background = Brushes.Red;
                errors.AppendLine("Введите пароль");
            }

            bool valid = false;
            string userType = "";
            var personal = DataBaseEntities.GetContext().User.ToList();
            foreach (var user in personal)
            {
                if (_currentUser.Login == user.Login && GetHash(Password.Password) == user.Password)
                {
                    valid = true;
                    userType = user.UserType;
                    MainViewModel.currentUser = user.User_id;
                    MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                    mainWindow.Name_User_Text.Text = user.Login;
                }
            }

            if (errors.Length > 0)
            {
                return;
            }

            if (valid)
            {
                MainViewModel.MainFrame.Navigate(null);
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.WarehouseButton.Visibility = Visibility.Hidden;
                mainWindow.LogOutButton.Visibility = Visibility.Visible;
                mainWindow.LogInButton.Visibility = Visibility.Hidden;
                mainWindow.Name_User.Visibility = Visibility.Visible;
                


                if (userType == "admin")
                {
                    mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                    MainViewModel.typeUser = userType;
                    new Page_Warehouse().BtnSwitch.Visibility = Visibility.Visible;
                    new Page_WarehouseAutopart().BtnSwitch.Visibility = Visibility.Visible;
                    return;
                }

                if (userType == "service")
                {
                    mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                    MainViewModel.typeUser = userType;
                    return;
                }

                if (userType == "consultant")
                {
                    mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                    MainViewModel.typeUser = userType;
                    return;
                }

                if (userType == "call")
                {
                    mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                    MainViewModel.typeUser = userType;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        public static string GetHash(string input) //кодирование пароля
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
