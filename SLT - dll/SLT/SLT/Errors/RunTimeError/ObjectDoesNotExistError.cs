using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class ObjectNotFoundError:RunTimeError
    {
        public ObjectNotFoundError(int start, int len, int line, string name) :
            base(start, len, line)
        {
            this.Text = "Не существует объекта: " + name;
        }
        public ObjectNotFoundError(int start, int len, int line, Exception inner) :
            base(inner)
        {            
            this.Text = "Не существует объекта";
            ((UserError)inner).Start = start;
            ((UserError)inner).Length = len;
            ((UserError)inner).Line = line;
        }
        ObjectNotFoundError(Exception inner) :
            base(inner)
        {
            this.Text = "Не существует объекта";
        }
    }
}