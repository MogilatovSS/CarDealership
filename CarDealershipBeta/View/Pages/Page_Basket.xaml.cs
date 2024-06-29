using CarDealershipBeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Data.Entity.Migrations;

namespace CarDealershipBeta.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page_Basket.xaml
    /// </summary>
    public partial class Page_Basket : Page
    {
        private int sumMoney = 0;
        public Page_Basket()
        {
            InitializeComponent();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            var autoparts = YourRoadDataBaseEntities.GetContext().BasketAutopart.
                Where(a => a.User_id == MainViewModel.currentUser && a.Sold == false || a.Sold == null).ToList();
            var user = YourRoadDataBaseEntities.GetContext().User.SingleOrDefault(u => u.User_id == MainViewModel.currentUser);

            foreach (var currentAutopart in autoparts)
            {
                currentAutopart.Sold = true;
                YourRoadDataBaseEntities.GetContext().BasketAutopart.AddOrUpdate(currentAutopart);

                try
                {
                    YourRoadDataBaseEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            Good.Visibility = Visibility.Visible;
            MailAddress from = new MailAddress("cardealership2022@mail.ru", "CarDealer");
            MailAddress to = new MailAddress($"{Convert.ToString(user.Email)}");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Покупка";
            m.Body = $"Ваша сумма покупки составила {Convert.ToString(sumMoney)} $<br/>Перейдите по ссылке для завершения " +
                $"оплаты: https://vk.com/prosto_sema" +
                $" <br/><br/>Удачного дня!<br/>С уважением ваш CarDealer.";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("cardealership2022@mail.ru", "dFkbrdv2ap928ER7GF3k");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Reload();
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
            var autoparts = YourRoadDataBaseEntities.GetContext().BasketAutopart.Where(a => a.User_id == MainViewModel.currentUser && a.Sold == false || a.Sold == null).ToList();
            sumMoney = 0;

            foreach (var currentAutopart in autoparts)
            {
                sumMoney += currentAutopart.WarehouseAutopart.Price * currentAutopart.AmountItem;
            }

            Money_TextBlock.Text = $"Сумма: {sumMoney} $";
            ListViewCatalogCars.ItemsSource = autoparts;

            if (autoparts.Count > 0)
            {
                Text_Basket.Visibility = Visibility.Hidden;
                Image_Basket.Visibility = Visibility.Hidden;
                Buy_Button.Visibility = Visibility.Visible;
            }
            else
            {
                Text_Basket.Visibility = Visibility.Visible;
                Image_Basket.Visibility = Visibility.Visible;
                Buy_Button.Visibility = Visibility.Hidden;
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
    }
}
