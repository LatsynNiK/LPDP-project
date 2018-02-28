using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP
{
    public class PhraseNotFound:SyntacticalError
    {
        public PhraseNotFound() { }
        public PhraseNotFound(int start, int len, int line, TextAnalysis.PhraseType type):
            base(start, len, line)
        {
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
        }

        public PhraseNotFound(Exception inner, TextAnalysis.PhraseType type)
            : base(inner)
        {
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
            //base.Start = ((SyntacticalError)inner).Start;
            //base.Length = ((SyntacticalError)inner).Length;
            //base.Line = ((SyntacticalError)inner).Line;
        }
    }
}
