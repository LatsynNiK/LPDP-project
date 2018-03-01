using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class ObjectDoesNotExistError:RunTimeError
    {
        public ObjectDoesNotExistError(int start, int len, int line, string name) :
            base(start, len, line)
        {
            //string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не существует объекта: " + name;
        }
        public ObjectDoesNotExistError(int start, int len, int line, Exception inner) :
            base(inner)
        {            
            this.Text = "Не существует объекта";
            ((UserError)inner).Start = start;
            ((UserError)inner).Length = len;
            ((UserError)inner).Line = line;
        }
        ObjectDoesNotExistError(Exception inner) :
            base(inner)
        {
            this.Text = "Не существует объекта";
        }
    }
}