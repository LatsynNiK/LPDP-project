using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public class MacrosTable
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

        //public void Delete(int id_rec)
        //{
        //    Macro rec = this.Table.Find(r => r.ID == id_rec);
        //    this.Table.Remove(rec);
        //}
    }
}
