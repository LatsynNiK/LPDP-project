using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class LexicalError:UserError
    {
        //public string Text;
        public LexicalError() { }
        public LexicalError(Exception inner):base(inner)
        {
            this.Text = "Лексическая ошибка";
        }
    }
}
