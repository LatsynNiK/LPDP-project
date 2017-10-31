using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Structure
{
    public enum ActionName
    {
        Write_to_FTT,//time, to label, to unit
        Write_to_CT,//condition, link name ("true" if initiator),to the begining ("true" or "false"), to label, to unit
        Assign,//var, value
        Create,
        Delete
    }

    public class Action
    {
        public ActionName Name;
        public List<object> Parameters;
        public Operator ParentOperator;

        public Action()
        {
            this.Parameters = new List<object>();
        }

        //public void AddParam(object par)
        //{
        //    this.Parameters.Add(par);
        //}
    }
}
