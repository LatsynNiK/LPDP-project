using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    abstract class RecordEvent
    {
        public int ID;        
        public Initiator Initiator;
        public Subprogram Subprogram;

        public RecordEvent(int id, Initiator init, Subprogram subpr)
        {
            this.ID = id;            
            this.Initiator = init;
            this.Subprogram = subpr;
        }
    }
}
