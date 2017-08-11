using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    class TimeAndConditionController
    {
        public FutureTimesTable FTT;
        public ConditionsTable CT;
        int ID_Counter;

        public TimeAndConditionController()
        {
            ID_Counter = 0;
            FTT = new FutureTimesTable();

 
        }

        public void AddTimeRecord(double time, int init, int lable, bool islast)
        {
            if (islast) { ID_Counter--; }
            RecordFTT NewRec = new RecordFTT(ID_Counter, time, init, lable);
            ID_Counter++;
        }
    }
}
