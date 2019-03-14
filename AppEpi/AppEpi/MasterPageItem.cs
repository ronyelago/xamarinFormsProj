using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public class MasterPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }

    public class PersonList : List<MasterPageItem>
    {
        public string Heading { get; set; }
        public List<MasterPageItem> Persons => this;
    }
}
