using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppEpi.ViewModels
{
    public class AtribuicaoCrachaViewModel : INotifyPropertyChanged
    {
        private string _matriculaFuncionario;
        private string _numeroCracha;

        public string MatriculaFuncionario
        {
            get { return this._matriculaFuncionario; }
            set
            {
                this._matriculaFuncionario = value;
                OnPropertyChanged();
            }
        }

        public string NumeroCracha
        {
            get { return this._numeroCracha; }
            set
            {
                this._numeroCracha = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
