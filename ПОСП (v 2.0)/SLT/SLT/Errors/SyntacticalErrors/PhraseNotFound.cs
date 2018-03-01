using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.TextAnalysis;

namespace SLT
{
    public class PhraseNotFound:SyntacticalError
    {
        public PhraseType Type;
        public PhraseNotFound() { }
        public PhraseNotFound(int start, int len, int line, TextAnalysis.PhraseType type):
            base(start, len, line)
        {
            this.Type = type;
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
        }

        public PhraseNotFound(Exception inner, TextAnalysis.PhraseType type)
            : base(inner)
        {
            this.Type = type;
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
            //base.Start = ((SyntacticalError)inner).Start;
            //base.Length = ((SyntacticalError)inner).Length;
            //base.Line = ((SyntacticalError)inner).Line;
        }
    }
}
