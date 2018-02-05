using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class Error:ApplicationException
    {
        public string Text;

        public Error() {}
        public Error(Exception inner)
            : base("", inner)
        {
            //this.Message = text;
        }

        public string GetErrorStack()
        {
            if (this.InnerException == null)
            {
                return this.Text + ";";
            }
            else
            {
                return this.Text + " -> " + ((Error)base.InnerException).GetErrorStack();
            }
        }
        
    }
}
