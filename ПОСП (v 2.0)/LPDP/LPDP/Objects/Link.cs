using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class Link:Object
    {
        int Value;

        public Link(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Link;
            this.Value = -1;
        }
    }
}
