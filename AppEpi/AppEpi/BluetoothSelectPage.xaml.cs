using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class BluetoothSelectPage : ContentPage
    {
        public BluetoothSelectPage()
        {
            InitializeComponent();
        }

        async private void btnParear(object sender, EventArgs e)
        {/*
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int coun = 0;



            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (coun > 0)
            {
                var answer = await DisplayAlert("Consulta de Epi", "Confirmar Consulta?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.inspecaoEPIFUNC(listEPCS);
                    var result = wbs.consultEPI(listEPCS);
                    UsuarioLogado.Operacao = "7";
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new ResultadoTrn(result);
                    //await Navigation.PushModalAsync(detailPage);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    //await Navigation.PushModalAsync(detailPage);
                    await Navigation.PushAsync(detailPage);



                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    //var detailPage = new NaoConforme(result);
                    //await Navigation.PushPopupAsync(detailPage);
                    //PushModalAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Consulta de Epi", "Verifique os Campos!", "OK");
            }
            */
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            //epis.Text = "";

        }
    }
}


/*
        <ScrollView x:Name="scrolly">
            <StackLayout Margin="10,10"  x:Name="formLayout"  Spacing="10">
                <StackLayout Padding="5,10" HeightRequest="200">
                    <Label >EPI´S</Label>
                    <Editor x:Name="epis" HeightRequest="200" Text="0E8D4A5AC1200001033303DC"  />
                </StackLayout>
                <Button Text="Parear" FontSize="18" x:Name="btnParear" Clicked="btnParear_Clicked" Style="{StaticResource BotaoAzul1}" />
            </StackLayout>
        </ScrollView>
*/