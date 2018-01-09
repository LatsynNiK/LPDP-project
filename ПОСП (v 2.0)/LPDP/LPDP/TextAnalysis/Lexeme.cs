using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    public class Lexeme : Phrase
    {
        public LexemeType LType;
        public string LValue;
        //public int Line;
        //public int Index;

        //public int Start;
        //public int Length;

        public Lexeme()
        {
        }

        public Lexeme(LexemeType type, string value, int line, int start, int length)
            : base(PhraseType.UnknownLexeme)
        {
            base.Start = start;
            base.Length = length;
            base.Line = line;

            base.Value = null;
            base.Level = 0;
            //    this.Value.Add(lexeme);

            this.LType = type;
            this.LValue = value;

        }

        public override string ToString()
        {
            return this.LValue;
        }
    }
}
