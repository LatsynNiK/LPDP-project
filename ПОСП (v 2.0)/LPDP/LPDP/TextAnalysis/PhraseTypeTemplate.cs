using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    class PhraseTypeTemplate
    {
        public PhraseType PhType;
        public PhraseType[] PhTemplate;

        public int ConcatedPhrases;

        public PhraseTypeTemplate(PhraseType type, params PhraseType[] template)
        {
            this.PhType = type;
            this.PhTemplate = template;
            this.ConcatedPhrases = template.Length;
        }
    }
}
