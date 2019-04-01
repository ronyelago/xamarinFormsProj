using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppEpi
{
    public partial class App : Application
    {
        private IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        public App()
        {
            InitializeComponent();
            //MainPage = new AppEpi.MainPage();

            NavigationPage.SetHasNavigationBar(this, false);
            MainPage = new NavigationPage(new Views.Login());

            // Inicialização do módulo de Bluetooth
            _bluetoothController.Init();

            // Conexão automática ao primeiro dispositivo pareado
            _bluetoothController.Connect();
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
