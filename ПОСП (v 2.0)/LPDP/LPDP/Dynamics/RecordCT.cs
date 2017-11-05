using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;
using LPDP.Objects;
using LPDP.Structure;

namespace LPDP.Dynamics
{
    public class RecordCT : RecordEvent
    {
        //public int ID;
        public Phrase Condition;
        ////public string FromUnit;
        //public int Initiator;
        //public int Subprogram;

        public RecordCT(int id, Phrase cond, Initiator init, Subprogram subp)
            :base(id,init,subp)
        {
            //this.ID = id;
            this.Condition = cond;
            //this.Initiator = init;
            //this.Subprogram = subp;
        }
    }
}
