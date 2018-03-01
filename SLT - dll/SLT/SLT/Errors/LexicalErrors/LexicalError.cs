using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class LexicalError:UserError
    {
        public LexicalError() { }
        public LexicalError(Exception inner):base(inner)
        {
            this.Text = "Лексическая ошибка";
        }
    }
}
