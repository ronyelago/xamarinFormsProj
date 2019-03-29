using Xamarin.Forms;
using System.Diagnostics;
using System;

namespace AppEpi.ViewModels
{
    public class ConfirmarButton : Button
    {
        private EventHandler _clickedEHandler;

        public Page ParentPage
        {
            get
            {
                var parent = Parent;
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
        public ConfirmarButton()
        {
            Text = "Confirmar";
            FontSize = 18;
            Style = (Style)Application.Current.Resources["BotaoAzul1"];
            _clickedEHandler = new EventHandler(OnClicked);
            Clicked += _clickedEHandler;
        }


        protected void OnClicked(object sender, EventArgs e)
        {
            // partes comentadas foi uma tentativa de evitar duplo clique que deu efeito rebote
            //Clicked -= _clickedEHandler;
            if (ParentPage is IConfirmacao)
            {
                ((IConfirmacao)ParentPage).OnConfirmarClicked();
            }
            else
                Debug.WriteLine("Page " + ParentPage.ToString() + "não implementa interface de confirmacao.");
            //Clicked += _clickedEHandler;
        }
    }
}
