using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class DistribuicaoViewModel
    {
        public ObservableCollection<ItemDistribuicaoViewModel> ListaCrachas { get; set; }
        public ObservableCollection<ItemDistribuicaoViewModel> ListaEpis { get; set; }

        public Command<ItemDistribuicaoViewModel> CommandRemove
        {
            get
            {
                return new Command<ItemDistribuicaoViewModel>((cracha) =>
                {
                    ListaCrachas.Remove(cracha);
                });
            }
        }
    }
}
