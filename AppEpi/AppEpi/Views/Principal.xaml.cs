﻿using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Principal : ContentPage
    {
        public Principal()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                return false;
            });
        }
    }
}