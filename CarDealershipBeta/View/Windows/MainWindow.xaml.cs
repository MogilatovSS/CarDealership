using CarDealershipBeta.View.Pages;
using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarDealershipBeta
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.MainFrame = MainFrame;
        }

        private void TopMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState != WindowState.Maximized)
            {
                this.WindowState=WindowState.Maximized;
            }
            else
            {
                this.WindowState=WindowState.Normal;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuHumburger.Visibility == Visibility)
            {
                MenuHumburger.Visibility = Visibility.Hidden;
                return;
            }
            MenuHumburger.Visibility = Visibility.Visible;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MenuHumburger.Visibility = Visibility.Hidden;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            
            MenuHumburger.Visibility = Visibility.Hidden;
        }

        private void FavouritesButton_Click(object sender, RoutedEventArgs e)
        {
            MenuHumburger.Visibility = Visibility.Hidden;
        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            MenuHumburger.Visibility = Visibility.Hidden;
        }

        public void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.currentUser = 0;
            WarehouseButton.Visibility = Visibility.Hidden;
            LogOutButton.Visibility = Visibility.Hidden;
            LogInButton.Visibility = Visibility.Visible;
            Name_User.Visibility = Visibility.Hidden;
            Timer.Visibility = Visibility.Hidden;
            MainFrame.Navigate(null);
            MenuHumburger.Visibility = Visibility.Hidden;
        }
    }
}
