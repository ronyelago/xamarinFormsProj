using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class AppEPIButton : Button
    {
        public Page ParentPage
        {
            get
            {
                var parent = Parent;
                // Busca o pai dos pais até achar uma Page
                while (parent != null)
                {
                    if (parent is Page)
                    {
                        return parent as Page;
                    }
                    parent = parent.Parent;
                }
                return null;
            }
        }

        // Constructor
        public AppEPIButton()
        {
            FontSize = 18;
            Style = (Style)Application.Current.Resources["BotaoAzul1"];
        }
    }
}
