using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class MacrosTable
    {
        List<Macro> Macros;

        public MacrosTable()
        {
            this.Macros = new List<Macro>();
        }

        //???
        public void Add(Macro rec)
        {
            this.Macros.Add(rec);
        }
    }
}
