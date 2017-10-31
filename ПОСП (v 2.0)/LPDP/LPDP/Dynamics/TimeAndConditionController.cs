using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Dynamics
{
    public class TimeAndConditionController
    {
        int ID_event_Counter;

        Executor ParentExecutor;
        FutureTimesTable FTT;
        ConditionsTable CT;
        

        public TimeAndConditionController(LPDP.Executor executor)
        {
            this.ParentExecutor = executor;
            ID_event_Counter = 0;

            this.FTT = new FutureTimesTable();
            this.CT = new ConditionsTable();
        }

        public void AddTimeRecord(double time, int init, int lable, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordFTT NewRec = new RecordFTT(ID_event_Counter, time, init, lable);
            ID_event_Counter++;
            FTT.Add(NewRec);
        }

        public void AddConditionRecord(Phrase phrase, int init, int lable, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordCT NewRec = new RecordCT(ID_event_Counter, phrase, init, lable);
            ID_event_Counter++;
            CT.Add(NewRec);
        }

        public void InsertConditionRecord(Phrase phrase, int init, int lable, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordCT NewRec = new RecordCT(ID_event_Counter, phrase, init, lable);
            ID_event_Counter++;
            CT.Insert(NewRec);
        }

        public void DeleteRecords(int deleted_id, FutureTimesTable FTT, ConditionsTable CT)
        {
            CT.CondTable.RemoveAll(rec => rec.ID == deleted_id);
            FTT.TimesTable.RemoveAll(rec => rec.ID == deleted_id);
        }

        //public int FindNextEvent()
        //{
        //    for
        //}
    }
}
