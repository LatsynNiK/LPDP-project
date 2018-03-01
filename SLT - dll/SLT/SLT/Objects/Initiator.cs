using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SLT.Structure;

namespace SLT
{
    enum InitiatorType { Aggregate, Flow, Description, RunTimeError}
    class Initiator
    {
        public int Number; 
        public int ID_of_MemoryCell;
        public InitiatorType Type;

        public Operator NextOperator;

        public Initiator(InitiatorType type)
        {
            this.Type = type;
        }

        public int GetID()
        {
            return this.ID_of_MemoryCell;
        }

        public override string ToString()
        {            
            return this.Number.ToString();
        }
    }
}
