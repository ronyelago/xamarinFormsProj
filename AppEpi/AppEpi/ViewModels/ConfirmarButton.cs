using System.Diagnostics;
using System;

namespace AppEpi.ViewModels
{
    public class ConfirmarButton : AppEPIButton
    {
        private EventHandler _clickedEventHandler;

        // Constructor
        public ConfirmarButton() : base()
        {
            Text = "Confirmar";
            _clickedEventHandler = new EventHandler(OnClicked);
            Clicked += _clickedEventHandler;
        }


        protected void OnClicked(object sender, EventArgs e)
        {
            // tentativa de evitar duplo clique que deu efeito rebote
            // não sei pq a TwoS usava estrutura semelhante em algumas das páginas
            //Clicked -= _clickedEventHandler;
            //Clicked += _clickedEventHandler;

            try
            {
                if (ParentPage is IConfirmacao)
                {
                    ((IConfirmacao)ParentPage).OnConfirmarClicked();
                }
                else
                {
                    Debug.WriteLine(
                        "Page " + ParentPage.ToString() + " não implementa interface de confirmacao."
                        );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Exceção durante tentativa de confirmação na página " + ParentPage.ToString() + ":" + ex.Message
                    );
            }
        }
    }
}
