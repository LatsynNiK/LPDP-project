using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class UnknownLexemeError:LexicalError
    {        
        public UnknownLexemeError(char simbol, int start, int len, int line)
        {
            base.Start = start;
            base.Length = len;
            base.Line = line;
            this.Text = String.Format("Неизвестный символ: \"{0}\"", simbol);
        }
    }
}
