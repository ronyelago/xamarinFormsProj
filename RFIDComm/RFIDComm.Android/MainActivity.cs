﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Bluetooth;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AppEpi.Droid
{
    [Activity(Label = "RFIDComm", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        const string Tag = "MainActivity";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

