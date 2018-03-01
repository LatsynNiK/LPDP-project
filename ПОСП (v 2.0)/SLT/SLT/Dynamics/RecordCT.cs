using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.TextAnalysis;
using SLT.Objects;
using SLT.Structure;

namespace SLT.Dynamics
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
