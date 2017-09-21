using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Objects
{
    public class Macro:Object
    {
        public Phrase Code;
        public List<string> Vars;

        public Macro(string name, string unit, Phrase code, List<string> vars) :
            base(name, unit)
        {
            base.Type = ObjectType.Macro;
            this.Code = code;
            this.Vars = vars;
            //this.Vars = new string[];
        }

        //public Macro(int id, string name, string unit)
        //{
        //    this.ID = id;
        //    this.Name = name;
        //    this.Unit = unit;
        //    this.Code = new Phrase();
        //}
    }
}
