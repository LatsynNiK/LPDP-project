using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class SyntacticalError:UserError
    {
        public SyntacticalError() { }
        public SyntacticalError(int start, int len, int line):
            base(start, len, line){}

        public SyntacticalError(Exception inner)
            : base(inner)
        {
            this.Text = "Синтаксическая ошибка";
        }
    }
}
