using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLT.Structure;

namespace SLT.Objects
{
    public enum InitiatorType { Aggregate, Flow, Description, RunTimeError}
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
