using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class SystemError:Error
    {
        public SystemError() { }
        public SystemError(Exception inner)
            : base(inner)
        {
            this.Text = "Системная ошибка";
        }
    }
}
