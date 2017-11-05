using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Dynamics
{
    public class FutureTimesTable
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

        //public void Add(double time, int init, int lable, bool islast)
        //{
        //    RecordFTT NewRec = new RecordFTT(
        //}

        public void Delete(int id_rec)
        {
            RecordFTT rec = this.TimesTable.Find(r => r.ID == id_rec);
            this.TimesTable.Remove(rec);
        }

        public RecordFTT FindNextMinTimeRecord()
        {
            double time = this.TimesTable.Min(rec => rec.ActiveTime);
            RecordFTT result = this.TimesTable.Find(rec => rec.ActiveTime == time);
            return result;
            //if (finded != null)
            //{
            //    return finded;
            //}
            //else
            //{
            //    return -1;
            //}

        }
    }
}
