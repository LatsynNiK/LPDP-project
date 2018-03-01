using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT.TextAnalysis
{
    class Word:Lexeme
    {
        public WordType WType;
        public Word(WordType type, Lexeme lex)
        {
            base.Start = lex.Start;
            base.Length = lex.Length;
            base.Line = lex.Line;

            base.Value = lex.Value;
            base.Level = lex.Level;
            //    this.Value.Add(lexeme);

            base.LType = lex.LType;
            base.LValue = lex.LValue;

            this.WType = type;
        }
        public override string ToString()
        {
            return this.LValue;
        }
    }
}
