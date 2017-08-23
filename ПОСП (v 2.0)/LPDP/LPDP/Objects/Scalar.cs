using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class Scalar:Object
    {
        string Value;

        public Scalar(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Scalar;
            this.Value = "";
        }
    }
}
