using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Phrase
    {
        public PhraseType PhType;
        public List<Phrase> Value;

        public int Level;
        public int Start;
        public int Length;
        public int Line;

        public Phrase() { }

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
            if (ConcatedPhrase.Length > 0)
            {
                int start = ConcatedPhrase[0].Start;
                int finish = ConcatedPhrase[ConcatedPhrase.Length - 1].Start + ConcatedPhrase[ConcatedPhrase.Length - 1].Length;
                return finish - start;
            }
            else
                return 0;
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
