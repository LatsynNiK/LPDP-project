using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Core
{
    class RecordCT
    {
        public int ID;
        public Phrase Condition;
        //public string FromUnit;
        public int Initiator;
        public int Subprogram;

        public RecordCT(int id, Phrase cond, int init, int subp)
        {
            this.ID = id;
            this.Condition = cond;
            this.Initiator = init;
            this.Subprogram = subp;
        }
    }
}
