using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public enum InitiatorType {Aggregate, Flow}
    public class Initiator
    {
        public int Number; 
        public int ID_of_MemoryCell;
        InitiatorType Type;

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
    }
}
