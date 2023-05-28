using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Page_CatalogeAutopart.xaml
    /// </summary>
    public partial class Page_CatalogeAutopart : Page
    {
        private BasketAutopart basket = new BasketAutopart();
        public Page_CatalogeAutopart()
        {
            InitializeComponent();

            var autiparts = DataBaseEntities.GetContext().WarehouseAutopart.ToList();
            Type.ItemsSource = DataBaseEntities.GetContext().WarehouseAutopart.Select(c => c.Type_item).Distinct().ToList();

            ListViewCatalogCars.ItemsSource = autiparts.ToList();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            NameFilter.Text = null;
            Type.SelectedValue = null;
        }

        private void ComboBoxFiltr_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFiltrAutopart();
        }

        private void SwitchCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.Navigate(new Page_Cataloge());
        }

        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFiltrAutopart();
        }
        private void UpdateFiltrAutopart()
        {
            var autiparts = DataBaseEntities.GetContext().WarehouseAutopart.ToList();

            autiparts = autiparts.Where(c => (Type.SelectedItem as String == null || c.Type_item == Type.SelectedItem as String)).
                Where(c => c.Name_item.ToLower().Contains(NameFilter.Text.ToLower()) || c.Type_item.ToLower().Contains(NameFilter.Text.ToLower())).ToList();

            ListViewCatalogCars.ItemsSource = autiparts.ToList();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            var selectedAutopart = (sender as Button).DataContext as WarehouseAutopart;

            if (MainViewModel.currentUser == 0)
            {
                MessageBox.Show("Войдите в аккаунт");
                return;
            }

            Button button = (Button)sender;
            DependencyObject container = VisualTreeHelper.GetParent(button);

            TextBox textBox = FindVisualChild<TextBox>(container);
            if (textBox != null)
            {
                if (!Regex.IsMatch(textBox.Text, @"^\d+$"))
                {
                    MessageBox.Show("Введите кол-во товара");
                    return;
                }

                int amountItem = int.Parse(textBox.Text);

                if (amountItem == 0)
                {
                    MessageBox.Show("Введите кол-во товара");
                    return;
                }

                basket.AmountItem = amountItem;
                textBox.Text = null;
            }
            
            if (selectedAutopart != null)
            {
                basket.User_id = MainViewModel.currentUser;
                basket.Item_id = selectedAutopart.Item_id;
                if (basket.BasketAutopart_id == 0)
                {
                    DataBaseEntities.GetContext().BasketAutopart.Add(basket);
                }

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
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            int numChildren = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numChildren; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T found)
                    return found;

                T foundInChildren = FindVisualChild<T>(child);
                if (foundInChildren != null)
                    return foundInChildren;
            }

            return null;
        }
    }
}
