using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    enum OperatorName
    {
        Assign,
        Transfer,
        Create,
        If,
        Activate,
        Terminate,
        Delete,
        Passivate,
        SimpleWait,
        ComplexWait,
        //WaitTime,
        //WaitUntil,
        //WaitConditions,
        Execute
    }
    class Operator
    {
        public OperatorName Name;        
        public List<Action> Actions;
        public Subprogram ParentSubprogram;        
        public int Start;
        public int Length;

        public Operator() 
        {            
            this.Actions = new List<Action>();            
        }

        public void AddAction(Action act)
        {
            act.ParentOperator = this;
            this.Actions.Add(act);
        }
    }
}
