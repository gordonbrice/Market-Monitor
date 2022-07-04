using NodeMonitor.MAUI.ViewModels;

namespace NodeMonitor.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}