using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    public class Phrase
    {
        public PhraseType PhType;
        public List<Phrase> Value;

        public int Level;
        public int Start;
        public int Length;
        public int Line;

        public Phrase() { }

        //public Phrase(LPDP_ModelTextRules.PhraseType type, int level, int start, int length, Phrase lexeme)
        //{
        //    this.PhType = type;
        //    this.Level = level;
        //    this.Start = start;
        //    this.Length = length;

        //    this.Value = new List<Phrase>();
        //    this.Value.Add(lexeme);
        //}

        public Phrase(PhraseType type, params Phrase[] ConcatedPhrase)
        {
            this.PhType = type;
            if (ConcatedPhrase.Length == 0)
            {
                this.Level = -1;
            }
            else
            {
                this.Level = FindDeep(ConcatedPhrase);
            }
            if (ConcatedPhrase.Length != 0)
            {
                this.Start = ConcatedPhrase[0].Start;
                this.Line = ConcatedPhrase[0].Line;
            }
            this.Length = FindLength(ConcatedPhrase);


            this.Value = new List<Phrase>();
            foreach (Phrase ph in ConcatedPhrase)
            {
                this.Value.Add(ph);
            }

        }

        int FindDeep(params Phrase[] ConcatedPhrase)
        {
            int max_level = 0;
            foreach (Phrase ph in ConcatedPhrase)
            {
                if (ph.Level > max_level)
                    max_level = ph.Level;
            }
            return max_level + 1;
        }
        int FindLength(params Phrase[] ConcatedPhrase)
        {
            int length = 0;
            foreach (Phrase ph in ConcatedPhrase)
            {
                length += ph.Length;
            }
            return length;
        }

        public override string ToString()
        {
            string result = "";
            foreach (Phrase ph in this.Value)
            {
                if (ph is Lexeme)
                {
                    Lexeme lex = (Lexeme)ph;
                    result += lex.LValue;
                }
                else
                {
                    result += ph.ToString();
                }
            }
            return result;
        }
    }
}
