using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace NodeMonitor.MAUI.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        string password1;

        [ObservableProperty]
        string password2;

        [ICommand]
        async void Login()
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}
