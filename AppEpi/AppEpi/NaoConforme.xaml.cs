using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NaoConforme : PopupPage
    {
        private RESULTADOMOV id;
        private List<RESULTADOMOV> it;
        public NaoConforme(List<RESULTADOMOV> result)
        {
            InitializeComponent();
            listResultado.ItemsSource = result;
            it = result;

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var result = it.Where(x => x.corAviso == "#ff7f7f").ToList();
            if (result != null)
            {
                if (result.Count > 0)
                {
                    await DisplayAlert("Resultado", "Existe itens em Inconformidades\nQtd Itens:" + result.Count, "OK");
                }
            }
        }

        async private void listResultado_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

              ((ListView)sender).SelectedItem = null;
            id = ((RESULTADOMOV)e.SelectedItem);

            var detailPage = new EnviarNaoConforme(id.EPC);
            await Navigation.PushPopupAsync(detailPage);

        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }

        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }
    }
}