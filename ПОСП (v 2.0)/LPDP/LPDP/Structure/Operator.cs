using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;
using LPDP.DataSets;

namespace LPDP.Structure
{
    public enum OperatorName
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
    public class Operator
    {
        //public int ID;
        //public int Index;
        public OperatorName Name;
        //List<Parameter> Params;
        public List<Action> Actions;
        public Subprogram ParentSubprogram;
        //public Dictionary<string, object> Params;
        public Selection Position;

        public Operator() 
        {
            //this.Params = new Dictionary<string, object>();
            this.Actions = new List<Action>();
            this.Position = new Selection();
        }

        public void AddAction(Action act)
        {
            act.ParentOperator = this;
            this.Actions.Add(act);
        }

        //public Operator(int id, int ind, OperatorName name, List<Parameter> param_list )
        //{
        //    this.ID = id;
        //    this.Index = ind;
        //    this.Name = name;
        //    //this.Params = param_list;
        //}
        
    }
}
