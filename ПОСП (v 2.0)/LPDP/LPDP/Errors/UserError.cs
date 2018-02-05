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
        }

        //public UserError(int start, int len, int line):base(text)
        //{
        //    this.Text = text;
        //    this.Start = start;
        //    this.Length = len;
        //    this.Line = line;
        //}
        
    }
}
