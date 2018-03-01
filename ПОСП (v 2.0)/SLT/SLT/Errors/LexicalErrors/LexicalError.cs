using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class LexicalError:UserError
    {
        //public string Text;
        public LexicalError() { }
        public LexicalError(Exception inner):base(inner)
        {
            this.Text = "Лексическая ошибка";
            //base.Start = ((LexicalError)inner).Start;
            //base.Length = ((LexicalError)inner).Length;
            //base.Line = ((LexicalError)inner).Line;
        }
    }
}
