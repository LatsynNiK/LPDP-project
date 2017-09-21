using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public enum InitiatorType {Aggregate, Flow}
    class Initiator
    {
        int Number; 
        int ID_of_MemoryCell;
        InitiatorType Type;

        public Initiator(int number, int id, InitiatorType type)
        {
            this.Number = number;
            this.ID_of_MemoryCell = id;
            this.Type = type;
        }
    }
}
