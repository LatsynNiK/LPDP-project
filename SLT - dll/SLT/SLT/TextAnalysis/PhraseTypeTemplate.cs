using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
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

        public override string ToString()
        {
            return this.PhType.ToString();
        }
    }
}
