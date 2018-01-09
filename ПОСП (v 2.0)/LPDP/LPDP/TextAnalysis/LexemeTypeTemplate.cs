using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    public class LexemeTypeTemplate
    {
        public LexemeType LType;
        public LexemeType[] LTemplate;
        public int ConcatedLexemes;

        public LexemeTypeTemplate(LexemeType type, params LexemeType[] template)
        {
            this.LType = type;
            this.LTemplate = template;
            this.ConcatedLexemes = template.Length;
        }
    }
}
