using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class FutureTimesTable
    {
        public List<RecordFTT> TimesTable;

        public FutureTimesTable()
        {
            this.TimesTable = new List<RecordFTT>();
        }

        public void Add(RecordFTT rec)
        {
            this.TimesTable.Add(rec);
        }

        public void Delete(int id_rec)
        {
            RecordFTT rec = this.TimesTable.Find(r => r.ID == id_rec);
            this.TimesTable.Remove(rec);
        }

        public RecordFTT FindNextMinTimeRecord()
        {
            if (this.TimesTable.Count > 0)
            {
                double time = this.TimesTable.Min(rec => rec.ActiveTime);
                RecordFTT result = this.TimesTable.Find(rec => rec.ActiveTime == time);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
