using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPDP.TextAnalysis;

namespace LPDP.Objects
{
    class Macro:Object
    {
        Phrase Code;

        public Macro(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Macro;
            //this.Value
        }
    }
}
