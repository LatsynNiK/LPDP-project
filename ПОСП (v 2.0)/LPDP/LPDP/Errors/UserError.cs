using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class UserError:Error
    {
        //public string Text;
        public int Line;
        public int Start;
        public int Length;


        public UserError() { }
        public UserError(Exception inner):base(inner)
        {
            this.Text = "Пользовательская ошибка";
            if (inner is ErrorList)
            {
                this.Start = 0;
                this.Length = 0;
                this.Line = 0;
            }
            else
            {
                this.Start = ((UserError)inner).Start;
                this.Length = ((UserError)inner).Length;
                this.Line = ((UserError)inner).Line;
            }
        }

        public UserError(int start, int len, int line)
        {
            //this.Text = text;
            this.Start = start;
            this.Length = len;
            this.Line = line;
        }
        
    }
}
