using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    class FutureTimesTable
    {
        List<RecordFTT> TimesTable;


        public FutureTimesTable()
        {
            TimesTable = new List<RecordFTT>();
        }

        public void Add(RecordFTT rec)
        {
            TimesTable.Add(rec);
        }
        public void Delete(int id_rec)
        {
            RecordFTT rec = TimesTable.Find(r => r.ID == id_rec);
            TimesTable.Remove(rec);
        }
    }
}
