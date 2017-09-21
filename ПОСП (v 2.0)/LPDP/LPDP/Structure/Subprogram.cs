using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public class Subprogram
    {
        int ID;
        List<Operator> Operators;
        int OperatorIndexCounter;

        public Subprogram(int id)
        {
            this.ID = id;
            this.OperatorIndexCounter = 0;
            this.Operators = new List<Operator>();
        }

        public void AddOperator(int id_op, bool islast, OperatorName name, List<Parameter> Params)
        {
            if (islast)
            {
                OperatorIndexCounter--;
            }
            Operator NewOperator = new Operator(id_op, OperatorIndexCounter, name, Params);
            this.Operators.Add(NewOperator);
            OperatorIndexCounter++;
        }
    }
}
