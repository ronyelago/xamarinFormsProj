using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class EpcListView : ListView
    {
        private int? _itemsLimit = null;
        public int? ItemsLimit { get => _itemsLimit; set => _itemsLimit = value; }

        public int Count { get => _epcList.Count; }

        private ObservableCollection<string> _epcList = new ObservableCollection<string>();

        public EpcListView()
        {
            ItemsSource = _epcList;

            // Definição da altura da lista
            if (ItemsLimit != null)
            {
                // exatamente o necessário caso haja limite de itens
                HeightRequest = RowHeight * (double)ItemsLimit;
            }
            else
                // expansível até onde possível caso não haja
                VerticalOptions = LayoutOptions.StartAndExpand;

            MessagingCenter.Subscribe<App, string>(this, "EPC", (sender, arg) =>
            {
                if (ItemsLimit != null)
                {
                    if (_epcList.Count >= ItemsLimit)
                    {
                        // se houver limite de itens, retira-se o primeiro item da lista para inserir o novo
                        _epcList.RemoveAt(0);
                    }
                }
                _epcList.Add(arg);
            });
        }


        // remove todos os epcs da lista
        public void Clear()
        {
            _epcList.Clear();
        }


        // retorna uma lista dos EPCs sendo exibido do modo como exige o WebService
        // retorna null se não houverem itens
        // retorna o EPC sem alteração se houver apenas um 
        public string GetFormattedEpcList()
        {
            if (_epcList.Count > 0)
            {
                string[] epcList = new string[_epcList.Count];
                _epcList.CopyTo(epcList, 0);

                if (_epcList.Count > 1)
                {
                    string wbsFormattedEpcList = "";
                    foreach (string epc in epcList)
                    {
                        wbsFormattedEpcList += "|" + epc;
                    }

                    return wbsFormattedEpcList;
                }
                else
                    return epcList[0];
            }
            else
                return null;
        }
    }
}
