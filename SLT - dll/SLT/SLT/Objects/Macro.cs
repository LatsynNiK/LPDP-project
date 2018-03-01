using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Macro:Object
    {
        public Phrase Code;
        public List<string> Vars;

        public Macro(string name, string unit, Phrase code, List<string> vars) :
            base(name, unit)
        {
            base.Type = ObjectType.Macro;
            this.Code = code;
            this.Vars = vars;            
        }

        public override void SetValue(object value)
        {
            //!!!ЗАГЛУШКА
            this.Code = (Phrase)value;
        }
        public override object GetValue()
        {
            //!!!ЗАГЛУШКА
            return this.Code;
        }
    }
}
