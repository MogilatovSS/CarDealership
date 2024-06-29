using CarDealershipBeta.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для Page_Edit.xaml
    /// </summary>
    public partial class Page_EditCar : Page
    {
        private Car currentCar = new Car();
        public Page_EditCar(Car selectedCar)
        {
            InitializeComponent();

            if (selectedCar != null)
                currentCar = selectedCar;

            DataContext = currentCar;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            currentCar = (Car)DataContext;
            currentCar.Image = tb_Image.Text;

            if (currentCar.Price == 0)
                errors.AppendLine("Введите цену");
            if (string.IsNullOrWhiteSpace(currentCar.Name))
                errors.AppendLine("Введите марку");
            if (string.IsNullOrWhiteSpace(currentCar.Model))
                errors.AppendLine("Введите модель");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (currentCar.Car_id == 0)
                DataBaseEntities.GetContext().Car.AddOrUpdate(currentCar);

            try
            {
                DataBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void BtnBackPage_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.MainFrame.GoBack();
        }

        private void BtnInsertImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.png; *.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                tb_Image.Text = openFileDialog.FileName;
            }
        }
    }
}
