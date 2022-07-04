using NodeMonitor.MAUI.Models;
using NodeMonitor.MAUI.ViewModels;

namespace NodeMonitor.MAUI
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;
        LoginPageViewModel loginPageVM;
        public MainPage(MainPageViewModel vm, LoginPageViewModel loginPageVM)
        {
            InitializeComponent();
            BindingContext = vm;
            this.vm = vm;
            this.loginPageVM = loginPageVM;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            vm.Login(loginPageVM.Password1, loginPageVM.Password2);
            loginPageVM.Password1 = String.Empty;
            loginPageVM.Password2 = String.Empty;
        }
    }
}