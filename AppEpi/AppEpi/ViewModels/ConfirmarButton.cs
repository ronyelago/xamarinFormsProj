using Xamarin.Forms;
using System.Diagnostics;
using System;

namespace AppEpi.ViewModels
{
    public class ConfirmarButton : Button
    {
        public ConfirmarButton()
        {
            Text = "Confirmar";
            FontSize = 18;
            Style = (Style)Application.Current.Resources["BotaoAzul1"];
            Clicked += new EventHandler(OnClicked);
        }


        private Page GetParentPage()
        {
            VisualElement element = this;

            if (element != null)
            {
                var parent = element.Parent;
                while (parent != null)
                {
                    if (parent is Page)
                    {
                        return parent as Page;
                    }
                    parent = parent.Parent;
                }
            }
            return null;
        }

        protected void OnClicked(object sender, EventArgs e)
        {
            // usar para evitar duplos cliques:
            // btnConfirmar.Clicked -= btnConfirmar_Clicked;
            // Parent.Confirmar();
            // btnConfirmar.Clicked += btnConfirmar_Clicked;

            Debug.WriteLine("REGISTREI UM CLICAO!");
        }
    }
}
