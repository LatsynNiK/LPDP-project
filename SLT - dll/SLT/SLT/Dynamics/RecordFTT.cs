using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class RecordFTT : RecordEvent
    {        
        public double ActiveTime;        

        public RecordFTT(int id, double time, Initiator init, Subprogram subpr):
            base(id,init,subpr)
        {            
            this.ActiveTime = time;
        }

    }
}
