using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Android.Runtime;
using Xamarin.Forms;
using Rg.Plugins.Popup;

namespace AppEpi.Droid
{
    [Activity(Label = "EasyEPIHomolog", Icon = "@drawable/iconhomolog", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Inicialização obrigatória de plugins
            Popup.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            Forms.Init(this, bundle);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            LoadApplication(new App());
            Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.TurnScreenOn);
        }


        // Parte da inicialização do Plugin Plugin.Permissions (vide documentação no GitHub)
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        // Parte da inicialização do Plugin Rg.Plugins.Popup
        public override void OnBackPressed()
        {
            if (Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
    }
}


