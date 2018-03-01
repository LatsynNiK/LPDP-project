using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class PhraseNotFound:SyntacticalError
    {
        public PhraseType Type;
        public PhraseNotFound() { }
        public PhraseNotFound(int start, int len, int line, PhraseType type):
            base(start, len, line)
        {
            this.Type = type;
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
        }

        public PhraseNotFound(Exception inner, PhraseType type)
            : base(inner)
        {
            this.Type = type;
            string type_text = ModelTextRules.PhraseTypeCommonNames[type];
            this.Text = "Не найдено: " + type_text;
        }
    }
}
