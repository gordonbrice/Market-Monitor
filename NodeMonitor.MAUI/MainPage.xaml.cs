namespace NodeMonitor.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        MainPageViewModel vm;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            this.vm = vm;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count += 5;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            var password1 = await DisplayPromptAsync("Authentication", "What's your first password?");
            var password2 = await DisplayPromptAsync("Authentication", "What's your second password?");

            vm.Login(password1, password2);
            
        }
    }
}