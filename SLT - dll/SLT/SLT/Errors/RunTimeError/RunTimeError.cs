using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class RunTimeError:UserError
    {
        public RunTimeError() { }
        public RunTimeError(int start, int len, int line) :
            base(start, len, line)
        {            
            this.Text = "Ошибка времени выполнения";
        }
        public RunTimeError(Exception inner)
            : base(inner)
        {
            this.Text = "Ошибка времени выполнения";
        }
    }
}
