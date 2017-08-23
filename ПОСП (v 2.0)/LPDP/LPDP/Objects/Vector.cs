using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class Vector:Object
    {
        List<Object> Value;

        public Vector(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Vector;
            this.Value = new List<Object>();
        }

    }
}
