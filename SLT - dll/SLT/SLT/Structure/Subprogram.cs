using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Subprogram
    {
        public int ID;
        public List<Operator> Operators;        
        public Unit Unit;

        public Subprogram()
        {            
            this.Operators = new List<Operator>();
        }

        public void AddOperator(Operator oper)
        {            
            oper.ParentSubprogram = this;
            this.Operators.Add(oper);
            
        }
    }
}
