using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public class Label
    {
        //public int ID;
        public Subprogram Subprogram;
        public string Name;
        public string Unit;
        public bool Visible;

        public Label(Subprogram subp, string name, string unit, bool visible)
        {
            //this.ID = id;
            this.Subprogram = subp;
            this.Name = name;
            this.Unit = unit;
            this.Visible = visible;
        }

        public Label(string name, bool visible)
        {
            this.Name = name;
            this.Visible = visible;
        }

    }
}
