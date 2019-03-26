using System.Collections.Generic;
using Xamarin.Forms;

namespace AppEpi
{
    public class MasterPageCS : ContentPage
    {
        private ListView _listView;

        public MasterPageCS()
        {
            var masterPageItems = new List<MasterPageItem>();

            _listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var imageCell = new ImageCell();
                    imageCell.SetBinding(TextCell.TextProperty, "Title");
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
                    return imageCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None
            };

            Padding = new Thickness(0, 40, 0, 0);
            Title = "Menu";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    _listView
                }
            };
        }
    }
}
