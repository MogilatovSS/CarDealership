using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class Page_LogIn : Page
    {
        private User _currentUser = new User();
        private int numberOfAttempts = 0;
        private DispatcherTimer LogoutTimer;
        private TimeSpan remainingTime;

        public Page_LogIn()
        {
            InitializeComponent();
            InitializeLogoutTimer();

            DataContext = _currentUser;
        }

        private void Registration_Button(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_Registration());
        }

        private void LogIn_Button(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            var password = CheckBoxShowPassword.IsChecked.Value ? PasswordTextBox.Text : PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
            {
                Login.ToolTip = "Введите логин";
                Login.Background = Brushes.Red;
                errors.AppendLine("Введите логин");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                Login.ToolTip = "Введите пароль";
                Login.Background = Brushes.Red;
                errors.AppendLine("Введите пароль");
            }

            bool valid = false;
            string userType = "";
            var personal = YourRoadDataBaseEntities.GetContext().User.ToList();
            foreach (var user in personal)
            {
                if (_currentUser.Login == user.Login && GetHash(password) == user.Password)
                {
                    if (user.TimeLastEntry != null)
                    {
                        TimeSpan timeSinceLastLogout = DateTime.Now - (DateTime)user.TimeLastEntry;

                        if (timeSinceLastLogout.TotalSeconds < 15)
                        {
                            MessageBox.Show($"Вход заблокирован\nОсталось {15 - timeSinceLastLogout.TotalSeconds}");
                            BlockingEntranceAsync();
                            return;
                        }
                    }
                    valid = true;
                    userType = user.UserType;
                    MainViewModel.currentUser = user.User_id;
                    mainWindow.Name_User_Text.Text = user.Login;
                }
            }

            if (errors.Length > 0)
            {
                BlockingEntranceAsync();
                return;
            }

            if (!valid)
            {
                MessageBox.Show("Неверный логин или пароль");
                BlockingEntranceAsync();
                return;
            }

            MainViewModel.MainFrame.Navigate(null);
            mainWindow.WarehouseButton.Visibility = Visibility.Hidden;
            mainWindow.LogOutButton.Visibility = Visibility.Visible;
            mainWindow.LogInButton.Visibility = Visibility.Hidden;
            mainWindow.Name_User.Visibility = Visibility.Visible;
            mainWindow.Timer.Visibility = Visibility.Visible;

            numberOfAttempts = 0;

            if (userType == "admin")
            {
                mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                MainViewModel.typeUser = userType;
                new Page_Warehouse().BtnSwitch.Visibility = Visibility.Visible;
                new Page_WarehouseAutopart().BtnSwitch.Visibility = Visibility.Visible;
                StartLogoutTimer();
                return;
            }

            if (userType == "service")
            {
                mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                MainViewModel.typeUser = userType;
                StartLogoutTimer();
                return;
            }

            if (userType == "consultant")
            {
                mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                MainViewModel.typeUser = userType;
                StartLogoutTimer();
                return;
            }

            if (userType == "call")
            {
                mainWindow.WarehouseButton.Visibility = Visibility.Visible;
                MainViewModel.typeUser = userType;
                StartLogoutTimer();
                return;
            }
        }

        private async Task BlockingEntranceAsync()
        {
            numberOfAttempts++;

            if (numberOfAttempts == 3)
            {
                LoginButton.IsEnabled = false;

                await Task.Delay(10000);

                LoginButton.IsEnabled = true;
                numberOfAttempts = 0;
            }
        }

        public static string GetHash(string input) //кодирование пароля
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as System.Windows.Controls.CheckBox;
            if (checkBox.IsChecked.Value)
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Visibility = Visibility.Visible; 
                PasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void InitializeLogoutTimer()
        {
            LogoutTimer = new DispatcherTimer();
            LogoutTimer.Interval = TimeSpan.FromSeconds(1);
            LogoutTimer.Tick += LogoutTimer_Tick;
        }

        private void LogoutTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > TimeSpan.Zero)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                Logout();
            }
        }

        private void UpdateTimerDisplay()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Timer.Text = $"{remainingTime:mm\\:ss}";
        }

        private void StartLogoutTimer()
        {
            remainingTime = TimeSpan.FromSeconds(10);
            UpdateTimerDisplay();
            LogoutTimer.Start();
        }

        private void Logout()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            LogoutTimer.Stop();

            User user = YourRoadDataBaseEntities.GetContext().User.FirstOrDefault(c => c.User_id == MainViewModel.currentUser);
            user.TimeLastEntry = DateTime.Now;

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            mainWindow.LogOutButton_Click(null, null);
        }
    }
}
