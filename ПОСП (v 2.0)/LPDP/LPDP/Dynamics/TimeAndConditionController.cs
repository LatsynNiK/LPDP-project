using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;
using LPDP.Structure;
using LPDP.Objects;

namespace LPDP.Dynamics
{
    public class TimeAndConditionController
    {
        int ID_event_Counter;

        Executor ParentExecutor;
        FutureTimesTable FTT;
        ConditionsTable CT;

        // Getters
        public List<RecordFTT> FutureTimesTable
        {
            get{ return this.FTT.TimesTable;}
        }
        public List<RecordCT> ConditionsTable 
        {
            get { return this.CT.CondTable; }
        }
        

        public TimeAndConditionController(LPDP.Executor executor)
        {
            this.ParentExecutor = executor;
            ID_event_Counter = 0;

            this.FTT = new FutureTimesTable();
            this.CT = new ConditionsTable();
        }

        public void AddTimeRecord(double time, Initiator init, Subprogram subp/*int lable*/, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordFTT NewRec = new RecordFTT(ID_event_Counter, time, init, subp);
            ID_event_Counter++;
            FTT.Add(NewRec);
        }

        public void AddConditionRecord(Phrase phrase, Initiator init, Subprogram subp/*int lable*/, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordCT NewRec = new RecordCT(ID_event_Counter, phrase, init, subp);
            ID_event_Counter++;
            CT.Add(NewRec);
        }

        public void InsertConditionRecord(Phrase phrase, Initiator init, Subprogram subp/*int lable*/, bool islast)
        {
            if (islast) { ID_event_Counter--; }
            RecordCT NewRec = new RecordCT(ID_event_Counter, phrase, init, subp);
            ID_event_Counter++;
            CT.Insert(NewRec);
        }

        public void DeleteRecords(int deleted_id)
        {
            this.CT.CondTable.RemoveAll(rec => rec.ID == deleted_id);
            this.FTT.TimesTable.RemoveAll(rec => rec.ID == deleted_id);
        }

        public RecordEvent FindNextEvent()
        {
            RecordEvent result;
            foreach (RecordCT rec_ct in this.CT.CondTable)
            {
                bool right_cond = this.ParentExecutor.ComputeLogicExpression(rec_ct.Condition);
                if (right_cond)
                {
                    result = rec_ct;
                    return result;
                }
            }
            RecordFTT rec = this.FTT.FindNextMinTimeRecord();
            if (rec != null)
            {
                result = rec;
                return result;
            }
            return null; // модель остановлена

        }

        
    }
}
