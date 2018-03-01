using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.Structure;
using SLT.Objects;

namespace SLT.Dynamics
{
    public abstract class RecordEvent
    {
        public int ID;
        //public double ActiveTime;
        public Initiator Initiator;
        public Subprogram Subprogram;

        public RecordEvent(int id, Initiator init, Subprogram subpr)
        {
            this.ID = id;
            //this.ActiveTime = time;
            this.Initiator = init;
            this.Subprogram = subpr;
        }
    }
}
