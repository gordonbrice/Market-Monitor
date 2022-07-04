using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NodeMonitor.MAUI.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        [RelayCommand]
        public async void Logout()
        {
            if (Preferences.ContainsKey("LoggedIn"))
            {
                Preferences.Remove("LoggedIn");
            }

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
