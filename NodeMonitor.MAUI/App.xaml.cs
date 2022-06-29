namespace NodeMonitor.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            //window.Created += (s, e) =>
            //{
            //    var result = window.Page.DisplayPromptAsync("Password 1", "What's your first password?");
            //};

            return window;
        }

    }
}