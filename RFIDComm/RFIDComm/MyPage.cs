using System;

using Xamarin.Forms;

namespace AppEpi
{
    public class MyPage : ContentPage
    {
        public MyPage()
        {

            this.BindingContext = new MyPageViewModel();

            Picker pickerBluetoothDevices = new Picker() { Title = "Select a bth device" };
            pickerBluetoothDevices.SetBinding(Picker.ItemsSourceProperty, "ListOfDevices");
            pickerBluetoothDevices.SetBinding(Picker.SelectedItemProperty, "SelectedBthDevice");
            pickerBluetoothDevices.SetBinding(VisualElement.IsEnabledProperty, "IsPickerEnabled");

            Button buttonConnect = new Button() { Text = "Connect" };
            buttonConnect.SetBinding(Button.CommandProperty, "ConnectCommand");
            buttonConnect.SetBinding(VisualElement.IsEnabledProperty, "IsConnectEnabled");

            Button buttonDisconnect = new Button() { Text = "Disconnect" };
            buttonDisconnect.SetBinding(Button.CommandProperty, "DisconnectCommand");
            buttonDisconnect.SetBinding(VisualElement.IsEnabledProperty, "IsDisconnectEnabled");

            Button buttonAttributes = new Button() { Text = "Attributes" };
            buttonAttributes.SetBinding(Button.CommandProperty, "AttributesCommand");
            buttonAttributes.SetBinding(VisualElement.IsEnabledProperty, "IsAttributesEnabled");

            StackLayout slButtons = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { buttonDisconnect, buttonConnect, buttonAttributes } };

            ListView lv = new ListView();
            lv.SetBinding(ListView.ItemsSourceProperty, "ListOfBarcodes");
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, ".");

            int topPadding = 0;
            if (Device.RuntimePlatform == Device.Android)
                topPadding = 20;

            StackLayout sl = new StackLayout { Children = { pickerBluetoothDevices, slButtons, lv }, Padding = new Thickness(0, topPadding, 0, 0) };
            Content = sl;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

