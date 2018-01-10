using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPDP.Structure;

namespace LPDP.Objects
{
    public enum InitiatorType { Aggregate, Flow, Description}
    public class Initiator
    {
        public int Number; 
        public int ID_of_MemoryCell;
        public InitiatorType Type;

        public Operator NextOperator;

        public Initiator(InitiatorType type)
        {
            //this.Number = number;
            //this.ID_of_MemoryCell = id;
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
