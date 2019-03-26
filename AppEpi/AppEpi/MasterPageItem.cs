using System;
using System.Collections.Generic;

namespace AppEpi
{
    public class MasterPageItem
    {
        public string Title;
        public string IconSource;
        public Type TargetType;
    }

    public class PersonList : List<MasterPageItem>
    {
        public string Heading;
        public List<MasterPageItem> Persons => this;
    }
}
