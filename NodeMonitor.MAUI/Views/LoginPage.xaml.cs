using NodeMonitor.MAUI.ViewModels;

namespace NodeMonitor.MAUI;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}