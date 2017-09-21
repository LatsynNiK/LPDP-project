using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public class GlobalVarsTable
    {
        Dictionary<string, List<Object>> Vars;

        public GlobalVarsTable()
        {
            this.Vars = new Dictionary<string, List<Object>>();
        }

        public void AddUnit(string unit_name)
        {
            this.Vars.Add(unit_name, new List<Object>());
        }

        public void AddToUnit(Object obj, string unit_name)
        {
            this.Vars[unit_name].Add(obj);
        }
    }
}
