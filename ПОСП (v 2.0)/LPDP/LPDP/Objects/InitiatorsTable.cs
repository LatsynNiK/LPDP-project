using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public class InitiatorsTable
    {
        public List<Initiator> Initiators;
        int NumberCount;

        public InitiatorsTable()
        {
            this.Initiators = new List<Initiator>();
            this.NumberCount = 0;
        }

        public void Add(Initiator init)
        {
            init.Number = this.NumberCount;
            this.NumberCount++;
            this.Initiators.Add(init);
        }
    }
}
