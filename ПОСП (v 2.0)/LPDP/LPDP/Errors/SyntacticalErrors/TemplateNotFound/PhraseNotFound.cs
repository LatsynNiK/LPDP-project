using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class PhraseNotFound:SyntacticalError
    {
        public PhraseNotFound() { }
        public PhraseNotFound(int start, int len, int line, TextAnalysis.PhraseType type):
            base(start, len, line)
        {
            this.Text = "Не найдено: " + type.ToString();
        }

        public PhraseNotFound(Exception inner, TextAnalysis.PhraseType type)
            : base(inner)
        {
            this.Text = "Не найдено: " + type.ToString();
            //base.Start = ((SyntacticalError)inner).Start;
            //base.Length = ((SyntacticalError)inner).Length;
            //base.Line = ((SyntacticalError)inner).Line;
        }
    }
}
