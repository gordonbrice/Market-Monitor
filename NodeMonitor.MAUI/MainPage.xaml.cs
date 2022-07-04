using NodeMonitor.MAUI.ViewModels;

namespace NodeMonitor.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        MainPageViewModel vm;
        LoginPageViewModel loginVM;
        public MainPage(MainPageViewModel vm, LoginPageViewModel loginVM)
        {
            InitializeComponent();
            BindingContext = vm;
            this.vm = vm;
            this.loginVM = loginVM;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            vm.Login(loginVM.Password1, loginVM.Password2);
        }
    }
}