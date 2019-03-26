using System;
using Xamarin.Forms;
using Plugin.Geolocator;

namespace AppEpi.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public Color CorThema { get; private set; }


        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            masterPage.ListView.ItemSelected += OnItemSelected;
            CorThema = Color.FromHex("#124284");
            //INICIAR A PRIMEIRA TELA E ADICIONA COR NA BARRA DE TITULO

            Detail = new NavigationPage(new Principal())
            {
                BarBackgroundColor = CorThema,
                Title = "Easy Epi",
                BarTextColor = Color.White,
                Icon = "mnH.png"
            };
        }
        

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = CorThema,
                    BarTextColor = Color.White,
                    Icon = "mnH.png"
                };
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }


        async private void btnLocalizacao_Clicked(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
        }
    }
}
