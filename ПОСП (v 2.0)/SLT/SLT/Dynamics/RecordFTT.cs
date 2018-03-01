using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.Objects;
using SLT.Structure;

namespace SLT.Dynamics
{
    public class RecordFTT : RecordEvent
    {
        //public int ID;
        public double ActiveTime;
        //public int Initiator;
        //public int Subprogram;

        public RecordFTT(int id, double time, Initiator init, Subprogram subpr):
            base(id,init,subpr)
        {
            //this.ID = id;
            this.ActiveTime = time;
            //this.Initiator = init;
            //this.Subprogram = subpr;
        }

    }
}
