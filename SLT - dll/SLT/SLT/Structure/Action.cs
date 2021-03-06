﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public enum ActionName
    {
        Write_to_FTT,//time, to label, to unit, islast
        Write_to_CT,//condition, link name ("true" if initiator),to the begining ("true" or "false"), to label, to unit, islast
        Assign,//var, value
        Create,
        Delete,
        Terminate
    }

    class Action
    {
        public ActionName Name;
        public List<object> Parameters;
        public Operator ParentOperator;

        public Action()
        {
            this.Parameters = new List<object>();
        }
    }
}
