using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class Vector:Object
    {
        List<Object> Value;

        public Vector(string name, string unit, List<Object> value)
            : base(name, unit)
        {
            base.Type = ObjectType.Vector;
            this.Value = value;
        }

        public Vector(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Vector;
            this.Value = new List<Object>();
        }

        public Vector(int id)
            : base(id)
        {
            base.Type = ObjectType.Vector;
            this.Value = new List<Object>();
        }

    }
}
