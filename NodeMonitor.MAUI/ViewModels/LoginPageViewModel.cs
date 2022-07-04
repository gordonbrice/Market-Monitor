using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NodeMonitor.MAUI.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string password1;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string password2;

        [RelayCommand(CanExecute = nameof(CanLoginExecute))]
        async void Login()
        {
            if(Preferences.ContainsKey("LoggedIn"))
            {
                Preferences.Remove("LoggedIn");
            }
                
            Preferences.Set("LoggedIn", true);
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        private bool CanLoginExecute()
            => !string.IsNullOrWhiteSpace(password1) && !string.IsNullOrWhiteSpace(password2);
    }
}
