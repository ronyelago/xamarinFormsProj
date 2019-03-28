using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class ConfirmarButton : Button
    {
        public ConfirmarButton()
        {
            Text = "Confirmar";
            FontSize = 18;
            Style = (Style)Application.Current.Resources["BotaoAzul1"];
        }
    }
}
