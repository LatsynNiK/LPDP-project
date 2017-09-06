using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    class GlobalVarsTable
    {
        Dictionary<string, List<Object>> Vars;

        public GlobalVarsTable()
        {
            this.Vars = new Dictionary<string, List<Object>>();
        }
    }
}
