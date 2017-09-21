using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class Link:Object
    {
        int Value;

        public Link(string name, string unit)
            : base(name, unit)
        {
            base.Type = ObjectType.Link;
            this.Value = -1;
        }

        public Link(string name, string unit, int value)
            : base(name, unit)
        {
            base.Type = ObjectType.Link;
            this.Value = value;
        }


        public Link(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Link;
            this.Value = -1;
        }

        public Link(int id)
            : base(id)
        {
            base.Type = ObjectType.Link;
            this.Value = -1;
        }
    }
}
