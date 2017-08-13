using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    class RecordLabel
    {
        public int ID;
        public int SubprogramID;
        public string Name;
        public string Unit;
        public bool Visible;

        public RecordLabel(int id, int subp, string name, string unit, bool visible)
        {
            this.ID = id;
            this.SubprogramID = subp;
            this.Name = name;
            this.Unit = unit;
            this.Visible = visible;
        }
    }
}
