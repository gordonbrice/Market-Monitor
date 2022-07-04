using NodeMonitor.MAUI.ViewModels;

namespace NodeMonitor.MAUI;

public partial class LoginPage : ContentPage
{
	LoginPageViewModel vm;
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
		this.vm = vm;
	}

	private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
	{
        this.vm.Password1 = String.Empty;
        this.vm.Password2 = String.Empty;
    }
}