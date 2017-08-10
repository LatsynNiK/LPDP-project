using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    class Error
    {
        public ErrorType Type;
        public string ErrorText;
        public int Line;
        public int Start;
        public int Length;


        public Error(ErrorType type, string err_txt, int line, int start, int len)
        {
            this.Type = type;
            this.ErrorText = err_txt;
            this.Line = line;
            this.Start = start;
            this.Length = len;
        }
    }
}
