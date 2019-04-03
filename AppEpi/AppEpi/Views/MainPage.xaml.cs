using System;
using Xamarin.Forms;
using AppEpi.ViewModels;

namespace AppEpi.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public Color CorThema { get; private set; }


        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            menuLateral.MasterListView.ItemSelected += OnItemSelected;
            CorThema = Color.FromHex("#124284");
            //INICIAR A PRIMEIRA TELA E ADICIONA COR NA BARRA DE TITULO

            Detail = new NavigationPage(new Principal())
            {
                BarBackgroundColor = CorThema,
                Title = "EasyEPI",
                BarTextColor = Color.White,
                Icon = "mnH.png"
            };
        }
        

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = CorThema,
                    BarTextColor = Color.White,
                    Icon = "mnH.png"
                };
                menuLateral.MasterListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
