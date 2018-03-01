using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class RecordCT : RecordEvent
    {        
        public Phrase Condition;       

        public RecordCT(int id, Phrase cond, Initiator init, Subprogram subp)
            :base(id,init,subp)
        {
            this.Condition = cond;
        }
    }
}
