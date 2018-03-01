using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class ErrorList:Error
    {
        public List<Error> Errors;
        public ErrorList(List<Error> errs)
        {
            this.Errors = errs;
            this.Text = "Список ошибок:";

            foreach (Error e in this.Errors)
            {
                this.Text += "\n" + e.GetErrorStack()+";";
            }
        }

    }    
}
