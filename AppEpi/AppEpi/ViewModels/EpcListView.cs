using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class EpcListView : ListView
    {
        public int Count { get => _epcList.Count; }

        private ObservableCollection<string> _epcList = new ObservableCollection<string>();

        public EpcListView()
        {
            VerticalOptions = LayoutOptions.StartAndExpand;
            ItemsSource = _epcList;

            MessagingCenter.Subscribe<App, string>(this, "EPC", (sender, arg) =>
            {
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
        public string GetFormattedEpcList()
        {
            if (_epcList.Count > 0)
            {
                string wbsFormattedEpcList = "";

                string[] epcList = new string[_epcList.Count];
                _epcList.CopyTo(epcList, 0);

                foreach (string epc in epcList)
                {
                    wbsFormattedEpcList += "|" + epc;
                }

                return wbsFormattedEpcList;
            }
            else
                return null;
        }
    }
}
