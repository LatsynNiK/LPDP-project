using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
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
                return this.Text;// + ";";
            }
            else
            {
                string inner_text = ((Error)base.InnerException).Text;
                if (inner_text == this.Text)
                {
                    return ((Error)base.InnerException).GetErrorStack();
                }
                else
                {
                    return this.Text + " -> " + ((Error)base.InnerException).GetErrorStack();
                }
            }
        }
        
    }
}
