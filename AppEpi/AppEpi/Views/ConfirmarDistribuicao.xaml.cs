using AppEpi.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmarDistribuicao : ContentPage
    {
        private readonly DistribuicaoViewModel _distribuicaoViewModels;

        public ConfirmarDistribuicao(DistribuicaoViewModel distribuicaoEpiViewModels)
        {
            InitializeComponent();
            _distribuicaoViewModels = distribuicaoEpiViewModels;
            BindingContext = _distribuicaoViewModels;
            crachaListView.ItemsSource = _distribuicaoViewModels.ListaCrachas;
        }

        private void BtnRemover_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var cracha = button?.BindingContext as ItemDistribuicaoViewModel;

            var vm = BindingContext as DistribuicaoViewModel;
            vm.CommandRemove.Execute(cracha);
        }
    }
}