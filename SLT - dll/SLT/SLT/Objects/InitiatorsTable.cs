using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class InitiatorsTable
    {
        public List<Initiator> Initiators;
        int NumberCount;

        public InitiatorsTable()
        {
            this.Initiators = new List<Initiator>();
            this.NumberCount = 1;
        }

        public void Add(Initiator init)
        {
            init.Number = this.NumberCount;
            this.NumberCount++;
            this.Initiators.Add(init);
        }

        public void Delete(int id)
        {
            Initiator init = this.Initiators.Find(i => i.ID_of_MemoryCell == id);
            this.Initiators.Remove(init);
        }
    }
}
