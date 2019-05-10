using AppEpi.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmarDistribuicao : ContentPage
    {
        private readonly List<DistribuicaoViewModel> _distribuicaoViewModels;

        public ConfirmarDistribuicao(List<DistribuicaoViewModel> distribuicaoViewModels)
        {
            InitializeComponent();
            _distribuicaoViewModels = distribuicaoViewModels;
            BindingContext = _distribuicaoViewModels;
            itensListView.ItemsSource = _distribuicaoViewModels;
        }

        private void ItensListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}