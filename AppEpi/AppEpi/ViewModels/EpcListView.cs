using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class EpcListView : ListView
    {
        private int? _itemsLimit = null;
        public int ItemsLimit { get => Math.Abs((int)_itemsLimit); set => _itemsLimit = value; }
        public int Count { get => _epcList.Count; }

        private ObservableCollection<string> _epcList = new ObservableCollection<string>();

        // Constructor
        public EpcListView()
        {
            ItemsSource = _epcList;

            SetBasicLayout();

            MessagingCenter.Subscribe<App, string>(this, "EPC", (sender, arg) =>
                HandleIncoming(arg)
            );
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
                if (_epcList.Count > 1)
                {
                    string wbsFormattedEpcList = "";
                    foreach (string epc in _epcList)
                    {
                        wbsFormattedEpcList += "|" + epc;
                    }

                    return wbsFormattedEpcList;
                }
                else
                    return _epcList.OfType<string>().FirstOrDefault();
            }
            else
                return null;
        }


        private void SetBasicLayout()
        {
            // Definição da altura da lista
            if (_itemsLimit != null)
            {
                // exatamente o necessário caso haja limite de itens
                HeightRequest = RowHeight * (double)ItemsLimit;
            }
            else
                // expansível até onde possível caso não haja
                VerticalOptions = LayoutOptions.StartAndExpand;
        }


        private void HandleIncoming(string epc)
        {
            // itens duplicados são ignorados
            if (_epcList.Contains(epc))
                return;

            else if (_itemsLimit != null)
            {
                if (_epcList.Count >= ItemsLimit)
                {
                    // se houver limite de itens, retira-se o primeiro item da lista para inserir o novo
                    _epcList.RemoveAt(0);
                }
            }
            _epcList.Add(epc);
        }
    }
}
