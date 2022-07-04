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
            if(!string.IsNullOrWhiteSpace(this.loginPageVM.Password1) && !string.IsNullOrWhiteSpace(this.loginPageVM.Password2))
            {
                vm.Login(loginPageVM.Password1, loginPageVM.Password2);
                this.loginPageVM.Password1 = String.Empty;
                this.loginPageVM.Password2 = String.Empty;
            }
        }

    }
}