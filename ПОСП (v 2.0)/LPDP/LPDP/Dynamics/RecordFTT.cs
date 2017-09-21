using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Dynamics
{
    public class RecordFTT
    {
        public int ID;
        public double ActiveTime;
        public int Initiator;
        public int Subprogram;

        public RecordFTT(int id, double time, int init, int subpr)
        {
            this.ID = id;
            this.ActiveTime = time;
            this.Initiator = init;
            this.Subprogram = subpr;
        }
    }
}
