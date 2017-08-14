using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    enum OperatorName
    {
        Assign,
        Transfer,
        Create,
        If,
        Activate,
        Terminate,
        Passivate,
        Wait,
        //WaitTime,
        //WaitUntil,
        //WaitConditions,
        Execute
    }
    class Operator
    {
        int ID;
        int Index;
        OperatorName Name;
        List<Parameter> Params;

        public Operator(int id, int ind, OperatorName name, List<Parameter> param_list )
        {
            this.ID = id;
            this.Index = ind;
            this.Name = name;
            this.Params = param_list;
        }
    }
}
