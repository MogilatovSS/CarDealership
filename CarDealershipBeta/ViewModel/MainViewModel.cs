using CarDealershipBeta.View.Pages;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarDealershipBeta.ViewModel
{
    internal class MainViewModel : ViewModedBase
    {
        private Page Main = new Page_MaimWindow();
        private Page Cataloge = new Page_Cataloge();
        private Page Inf = new Page_Inf();
        private Page Service = new Page_Service();
        private Page _CurPage = new Page_MaimWindow();
        private Page Warehouse = new Page_Warehouse();
        private Page LogIn = new Page_LogIn();
        private Page Basket = new Page_Basket();
        private Page Profile = new Page_Profile();
        private Page Saved = new Page_Saved();

        public static Frame MainFrame { get; set; }
        public static int currentUser { get; set; }
        public Page CurPage
        {
            get => _CurPage;
            set => Set(ref _CurPage, value);
        }

        public ICommand Click_Home => new RelayCommand(() => CurPage = Main);
        public ICommand Click_Cataloge => new RelayCommand(() => CurPage = Cataloge);
        public ICommand Click_Inf => new RelayCommand(() => CurPage = Inf);
        public ICommand Click_Service => new RelayCommand(() => CurPage = Service);
        public ICommand Click_Warehouse => new RelayCommand(() => CurPage = Warehouse);
        public ICommand Click_LogIn => new RelayCommand(() => CurPage = LogIn);
        public ICommand Click_Basket => new RelayCommand(() => CurPage = Basket);
        public ICommand Click_Profile => new RelayCommand(() => CurPage = Profile);
        public ICommand Click_Saved => new RelayCommand(() => CurPage = Saved);


    }
}
