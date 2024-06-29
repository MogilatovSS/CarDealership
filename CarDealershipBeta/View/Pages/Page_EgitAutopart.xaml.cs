using CarDealershipBeta.ViewModel;
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
    /// Логика взаимодействия для Page_EgitAutopart.xaml
    /// </summary>
    public partial class Page_EgitAutopart : Page
    {
        private WarehouseAutopart _warehouseAutopart = new WarehouseAutopart();
        public Page_EgitAutopart(WarehouseAutopart selectedAutopart)
        {
            InitializeComponent();

            if (selectedAutopart != null)
                _warehouseAutopart = selectedAutopart;

            DataContext = _warehouseAutopart;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            _warehouseAutopart = (WarehouseAutopart)DataContext;

            if (_warehouseAutopart.Amount == 0)
                errors.AppendLine("Введите количество");
            if (_warehouseAutopart.Price == 0)
                errors.AppendLine("Введите цену");
            if (string.IsNullOrWhiteSpace(_warehouseAutopart.Name_item))
                errors.AppendLine("Введите наименование товара");
            if (string.IsNullOrWhiteSpace(_warehouseAutopart.Type_item))
                errors.AppendLine("Введите тип");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_warehouseAutopart.Item_id == 0)
                YourRoadDataBaseEntities.GetContext().WarehouseAutopart.AddOrUpdate(_warehouseAutopart);

            try
            {
                YourRoadDataBaseEntities.GetContext().SaveChanges();
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
    }
}
