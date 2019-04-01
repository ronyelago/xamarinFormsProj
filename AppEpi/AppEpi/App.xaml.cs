using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppEpi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppEpi.MainPage();

            NavigationPage.SetHasNavigationBar(this, false);
            MainPage = new NavigationPage(new Views.Login());

            // inicialização do módulo de Bluetooth
            DependencyService.Get<IBluetoothController>().Init();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
