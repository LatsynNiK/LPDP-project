using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Dynamics
{
    public class TimeAndConditionController
    {
        int ID_Counter;

        LPDP.Structure.Model ParentModel;

        public TimeAndConditionController(LPDP.Structure.Model model)
        {
            this.ParentModel = model;
            ID_Counter = 0;
        }

        public void AddTimeRecord(double time, int init, int lable, bool islast, FutureTimesTable FTT)
        {
            if (islast) { ID_Counter--; }
            RecordFTT NewRec = new RecordFTT(ID_Counter, time, init, lable);
            ID_Counter++;
            FTT.Add(NewRec);
        }

        public void AddConditionRecord(Phrase phrase, int init, int lable, bool islast, ConditionsTable CT)
        {
            if (islast) { ID_Counter--; }
            RecordCT NewRec = new RecordCT(ID_Counter, phrase, init, lable);
            ID_Counter++;
            CT.Add(NewRec);
        }

        public void DeleteRecords(int deleted_id, FutureTimesTable FTT, ConditionsTable CT)
        {
            CT.CondTable.RemoveAll(rec => rec.ID == deleted_id);
            FTT.TimesTable.RemoveAll(rec => rec.ID == deleted_id);
        }
    }
}
