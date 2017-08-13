using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    class LableTable
    {
        List<RecordLabel> Table;

        public LableTable()
        {
            this.Table = new List<RecordLabel>();
        }

        //???
        public void Add(RecordLabel rec)
        {
            this.Table.Add(rec);
        }

        public void Delete(int id_rec)
        {
            RecordLabel rec = this.Table.Find(r => r.ID == id_rec);
            this.Table.Remove(rec);
        }
    }
}
