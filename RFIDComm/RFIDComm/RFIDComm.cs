using System;
using Xamarin.Forms;

namespace RFIDComm
{
    public partial class App : Application
    {
        public App()
        {
            MainPage = new MyPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send<App>(this, "Sleep"); // When app sleep, send a message so I can "Cancel" the connection
        }

        protected override void OnResume()
        {
            MessagingCenter.Send<App>(this, "Resume"); // When app resume, send a message so I can "Resume" the connection
        }
    }
}
